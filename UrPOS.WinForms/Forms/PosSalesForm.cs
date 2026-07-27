using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;

namespace UrPOS.WinForms.Forms
{
    public partial class PosSalesForm : Form
    {
        private readonly IProductService _productService;
        private readonly IInvoiceService _invoiceService;
        private readonly BindingList<SalesInvoiceItem> _cartItems = new();

        private string _qtyBuffer = "1";
        private bool _qtyBufferTouched;
        private bool _isBusy;

        public PosSalesForm(IProductService productService, IInvoiceService invoiceService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));

            InitializeComponent();
            WireNumpad();
            BindCart();
            RefreshTotals();
            UpdateQtyDisplay();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            txtBarcode.Focus();
        }

        private void BindCart()
        {
            dgvCart.AutoGenerateColumns = false;
            dgvCart.DataSource = _cartItems;
        }

        private void WireNumpad()
        {
            foreach (Control control in tblNumpad.Controls)
            {
                if (control is Button button && button.Tag is string tag)
                {
                    button.Click += NumpadButton_Click;
                }
            }
        }

        private async void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await SearchAndAddProductAsync();
            }
        }

        private async void btnSearch_Click(object? sender, EventArgs e)
        {
            await SearchAndAddProductAsync();
        }

        private void btnNewInvoice_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                this,
                "واجهة فاتورة جديدة متعددة السلال قيد التجهيز (UI فقط — بدون منطق خلفي).",
                "+ فاتورة جديدة",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private void btnParkedInvoices_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                this,
                "واجهة الفواتير المعلقة (Parked Orders) قيد التجهيز (UI فقط — بدون منطق خلفي).",
                "الفواتير المعلقة",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private async Task SearchAndAddProductAsync()
        {
            if (_isBusy)
            {
                return;
            }

            var query = txtBarcode.Text.Trim();
            if (string.IsNullOrWhiteSpace(query))
            {
                ShowAlert("يرجى إدخال الباركود أو اسم المنتج.", isError: true);
                txtBarcode.Focus();
                return;
            }

            try
            {
                SetBusy(true);

                // البحث عبر طبقة الخدمة (تغلف IProductRepository.GetByBarcodeAsync)
                Product? product = await _productService.GetByBarcodeAsync(query);

                if (product is null)
                {
                    var matches = (await _productService.SearchByNameAsync(query)).ToList();
                    if (matches.Count == 1)
                    {
                        product = matches[0];
                    }
                    else if (matches.Count > 1)
                    {
                        ShowAlert("وُجدت عدة منتجات بهذا الاسم. حدّد البحث أو استخدم الباركود.", isError: true);
                        return;
                    }
                }

                if (product is null)
                {
                    ShowAlert("المنتج غير موجود.", isError: true);
                    txtBarcode.SelectAll();
                    return;
                }

                if (product.CurrentStock <= 0)
                {
                    ShowAlert($"المنتج ({product.ProductName}) غير متوفر في المخزن.", isError: true);
                    return;
                }

                var qty = ParseQtyBuffer();
                if (qty <= 0)
                {
                    ShowAlert("الكمية يجب أن تكون أكبر من صفر.", isError: true);
                    ResetQtyBuffer();
                    return;
                }

                if (qty > product.CurrentStock)
                {
                    ShowAlert($"الكمية المطلوبة غير متاحة. المتوفر: {product.CurrentStock}", isError: true);
                    return;
                }

                AddOrIncrementCartItem(product, qty);
                txtBarcode.Clear();
                ResetQtyBuffer();
                HideAlert();
                RefreshTotals();
                txtBarcode.Focus();
            }
            catch (Exception)
            {
                ShowAlert("تعذر البحث عن المنتج. حاول مرة أخرى.", isError: true);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void AddOrIncrementCartItem(Product product, int quantity)
        {
            var existing = _cartItems.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing is not null)
            {
                var newQty = existing.Quantity + quantity;
                if (newQty > product.CurrentStock)
                {
                    ShowAlert($"تجاوزت الكمية المتوفرة للمنتج ({product.ProductName}).", isError: true);
                    return;
                }

                existing.Quantity = newQty;
                existing.TotalPrice = existing.Quantity * existing.SalePrice;
                RefreshCartRow(existing);
                return;
            }

            _cartItems.Add(new SalesInvoiceItem
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                Barcode = product.Barcode,
                Quantity = quantity,
                SalePrice = product.SalePrice,
                TotalPrice = quantity * product.SalePrice
            });
        }

        private void RefreshCartRow(SalesInvoiceItem item)
        {
            var index = _cartItems.IndexOf(item);
            if (index >= 0)
            {
                _cartItems.ResetItem(index);
            }
        }

        private void NumpadButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button button || button.Tag is not string key)
            {
                return;
            }

            switch (key)
            {
                case "C":
                    ResetQtyBuffer();
                    break;
                case "BS":
                    _qtyBuffer = _qtyBuffer.Length <= 1 ? "0" : _qtyBuffer[..^1];
                    UpdateQtyDisplay();
                    break;
                case "+":
                    AdjustSelectedQuantity(1);
                    break;
                case "-":
                    AdjustSelectedQuantity(-1);
                    break;
                case "OK":
                    _ = SearchAndAddProductAsync();
                    break;
                default:
                    if (_qtyBuffer == "0" || (_qtyBuffer == "1" && !_qtyBufferTouched))
                    {
                        _qtyBuffer = key;
                        _qtyBufferTouched = true;
                    }
                    else if (_qtyBuffer.Length < 6)
                    {
                        _qtyBuffer += key;
                        _qtyBufferTouched = true;
                    }
                    UpdateQtyDisplay();
                    break;
            }
        }

        private void AdjustSelectedQuantity(int delta)
        {
            if (dgvCart.CurrentRow?.DataBoundItem is not SalesInvoiceItem item)
            {
                ShowAlert("حدّد صنفاً من السلة لتعديل الكمية.", isError: true);
                return;
            }

            var newQty = item.Quantity + delta;
            if (newQty <= 0)
            {
                _cartItems.Remove(item);
            }
            else
            {
                item.Quantity = newQty;
                item.TotalPrice = item.Quantity * item.SalePrice;
                RefreshCartRow(item);
            }

            RefreshTotals();
            HideAlert();
        }

        private void btnRemoveItem_Click(object? sender, EventArgs e)
        {
            if (dgvCart.CurrentRow?.DataBoundItem is not SalesInvoiceItem item)
            {
                ShowAlert("حدّد صنفاً من السلة للحذف.", isError: true);
                return;
            }

            _cartItems.Remove(item);
            RefreshTotals();
            HideAlert();
            txtBarcode.Focus();
        }

        private void btnClearCart_Click(object? sender, EventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                return;
            }

            var confirm = MessageBox.Show(
                this,
                "هل تريد تفريغ سلة المبيعات؟",
                "تأكيد",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            _cartItems.Clear();
            RefreshTotals();
            HideAlert();
            txtBarcode.Focus();
        }

        private async void btnCheckout_Click(object? sender, EventArgs e)
        {
            if (UserSession.Instance.IsGuest)
            {
                MessageBox.Show(
                    "أنت حالياً في وضع الضيف التجريبي.\nيمكنك تجربة السلة والبحث فقط، ولا يمكن حفظ المبيعات الفعلية.",
                    "وضع التجربة",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            await CheckoutAsync();
        }

        private async Task CheckoutAsync()
        {
            if (_isBusy)
            {
                return;
            }

            if (_cartItems.Count == 0)
            {
                ShowAlert("السلة فارغة. أضف منتجات قبل إتمام البيع.", isError: true);
                return;
            }

            try
            {
                SetBusy(true);

                var total = _cartItems.Sum(i => i.TotalPrice);
                var discount = 0m;

                var invoice = new SalesInvoice
                {
                    InvoiceNumber = $"INV-{DateTime.Now:yyyyMMddHHmmss}",
                    Discount = discount,
                    TotalAmount = total,
                    NetAmount = total - discount,
                    PaymentType = "CASH",
                    CreatedAt = DateTime.Now,
                    CustomerId = 0,
                    Items = _cartItems.Select(i => new SalesInvoiceItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        SalePrice = i.SalePrice,
                        TotalPrice = i.TotalPrice,
                        ProductName = i.ProductName,
                        Barcode = i.Barcode
                    }).ToList()
                };

                var result = await _invoiceService.ProcessSalesInvoiceAsync(invoice);

                if (!result.isSuccess)
                {
                    ShowAlert(string.IsNullOrWhiteSpace(result.ErrorMessage)
                        ? "فشل حفظ فاتورة المبيعات."
                        : result.ErrorMessage, isError: true);
                    return;
                }

                ShowAlert($"تم إتمام البيع بنجاح. رقم الفاتورة: {result.Data}", isError: false);
                _cartItems.Clear();
                RefreshTotals();
                ResetQtyBuffer();
                txtBarcode.Clear();
                txtBarcode.Focus();
            }
            catch (Exception)
            {
                ShowAlert("حدث خطأ أثناء إتمام البيع. حاول مرة أخرى.", isError: true);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void RefreshTotals()
        {
            var total = _cartItems.Sum(i => i.TotalPrice);
            lblNetAmount.Text = total.ToString("N2", CultureInfo.InvariantCulture);
            lblItemsCount.Text = $"عدد الأصناف: {_cartItems.Count}";
            btnCheckout.Enabled = _cartItems.Count > 0 && !_isBusy;
        }

        private int ParseQtyBuffer()
        {
            return int.TryParse(_qtyBuffer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var qty)
                ? qty
                : 1;
        }

        private void ResetQtyBuffer()
        {
            _qtyBuffer = "1";
            _qtyBufferTouched = false;
            UpdateQtyDisplay();
        }

        private void UpdateQtyDisplay()
        {
            lblQtyValue.Text = _qtyBuffer;
        }

        private void SetBusy(bool isBusy)
        {
            _isBusy = isBusy;
            txtBarcode.Enabled = !isBusy;
            btnSearch.Enabled = !isBusy;
            btnCheckout.Enabled = !isBusy && _cartItems.Count > 0;
            btnRemoveItem.Enabled = !isBusy;
            btnClearCart.Enabled = !isBusy;
            tblNumpad.Enabled = !isBusy;
            UseWaitCursor = isBusy;
        }

        private void ShowAlert(string message, bool isError)
        {
            lblAlert.Text = message;
            lblAlert.BackColor = isError
                ? Color.FromArgb(254, 226, 226)
                : Color.FromArgb(220, 252, 231);
            lblAlert.ForeColor = isError
                ? Color.FromArgb(153, 27, 27)
                : Color.FromArgb(22, 101, 52);
            lblAlert.Visible = true;
        }

        private void HideAlert()
        {
            lblAlert.Visible = false;
            lblAlert.Text = string.Empty;
        }

        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
