using System;
using System.Collections.Generic;
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
        private sealed class ParkedInvoiceDraft
        {
            public string Title { get; set; } = string.Empty;
            public DateTime ParkedAt { get; set; }
            public List<SalesInvoiceItem> Items { get; set; } = new();
        }

        private readonly IProductService _productService;
        private readonly IInvoiceService _invoiceService;
        private readonly BindingList<SalesInvoiceItem> _cartItems = new();
        private readonly List<ParkedInvoiceDraft> _parkedInvoices = new();

        private string _qtyBuffer = "1";
        private bool _qtyBufferTouched;
        private bool _isBusy;
        private int _draftSequence;

        public PosSalesForm(IProductService productService, IInvoiceService invoiceService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));

            InitializeComponent();
            WireNumpad();
            BindCart();
            RefreshTotals();
            UpdateQtyDisplay();
            UpdateParkedButtonText();
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
            if (_cartItems.Count == 0)
            {
                ShowAlert("السلة فارغة — أضف أصنافاً أولاً قبل فتح فاتورة جديدة.", isError: true);
                return;
            }

            ParkCurrentCartAsDraft();
            BeginFreshInvoice();
            ShowAlert($"تم حفظ الفاتورة كمسودة. الفواتير المعلقة: {_parkedInvoices.Count}", isError: false);
            txtBarcode.Focus();
        }

        private void btnParkedInvoices_Click(object? sender, EventArgs e)
        {
            if (_parkedInvoices.Count == 0)
            {
                // Same rule as new invoice: if current cart has items, park it as draft
                if (_cartItems.Count > 0)
                {
                    ParkCurrentCartAsDraft();
                    BeginFreshInvoice();
                    ShowAlert($"تم تعليق الفاتورة الحالية. الفواتير المعلقة: {_parkedInvoices.Count}", isError: false);
                    txtBarcode.Focus();
                    return;
                }

                ShowAlert("لا توجد فواتير معلقة حالياً.", isError: true);
                return;
            }

            ShowParkedInvoicesPicker();
        }

        private void ParkCurrentCartAsDraft()
        {
            _draftSequence++;
            var draft = new ParkedInvoiceDraft
            {
                Title = $"مسودة #{_draftSequence} — {_cartItems.Count} صنف",
                ParkedAt = DateTime.Now,
                Items = _cartItems.Select(CloneCartItem).ToList()
            };
            _parkedInvoices.Add(draft);
            UpdateParkedButtonText();
        }

        private void BeginFreshInvoice()
        {
            _cartItems.Clear();
            ResetQtyBuffer();
            RefreshTotals();
            HideAlert();
        }

        private static SalesInvoiceItem CloneCartItem(SalesInvoiceItem item)
        {
            return new SalesInvoiceItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Barcode = item.Barcode,
                Quantity = item.Quantity,
                SalePrice = item.SalePrice,
                TotalPrice = item.TotalPrice
            };
        }

        private void UpdateParkedButtonText()
        {
            btnParkedInvoices.Text = $"الفواتير المعلقة ({_parkedInvoices.Count})";
        }

        private void ShowParkedInvoicesPicker()
        {
            using var picker = new Form
            {
                Text = "الفواتير المعلقة",
                RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ClientSize = new Size(480, 360),
                BackColor = Color.FromArgb(241, 245, 249),
                Font = new Font("Segoe UI", 10F)
            };

            var list = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11F),
                IntegralHeight = false,
                RightToLeft = RightToLeft.Yes
            };

            for (var i = 0; i < _parkedInvoices.Count; i++)
            {
                var d = _parkedInvoices[i];
                list.Items.Add($"{i + 1}. {d.Title} — {d.ParkedAt:HH:mm}");
            }

            if (list.Items.Count > 0)
            {
                list.SelectedIndex = 0;
            }

            var buttons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 56,
                Padding = new Padding(12)
            };

            var btnOpen = new Button
            {
                BackColor = Color.FromArgb(13, 148, 136),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Dock = DockStyle.Right,
                Width = 140,
                Text = "فتح المسودة",
                DialogResult = DialogResult.OK
            };
            btnOpen.FlatAppearance.BorderSize = 0;

            var btnCancel = new Button
            {
                BackColor = Color.FromArgb(100, 116, 139),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Dock = DockStyle.Right,
                Width = 100,
                Text = "إلغاء",
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            var spacer = new Panel { Dock = DockStyle.Right, Width = 8 };
            buttons.Controls.Add(btnOpen);
            buttons.Controls.Add(spacer);
            buttons.Controls.Add(btnCancel);

            var hint = new Label
            {
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(12, 8, 12, 0),
                Text = "اختر مسودة لفتحها. إذا كانت السلة الحالية غير فارغة سيتم تعليقها أولاً.",
                TextAlign = ContentAlignment.MiddleLeft,
                RightToLeft = RightToLeft.Yes
            };

            picker.Controls.Add(list);
            picker.Controls.Add(buttons);
            picker.Controls.Add(hint);
            picker.AcceptButton = btnOpen;
            picker.CancelButton = btnCancel;

            if (picker.ShowDialog(this) != DialogResult.OK || list.SelectedIndex < 0)
            {
                return;
            }

            RestoreParkedInvoice(list.SelectedIndex);
        }

        private void RestoreParkedInvoice(int index)
        {
            if (index < 0 || index >= _parkedInvoices.Count)
            {
                return;
            }

            // Same logic as new invoice: park current open cart before switching
            if (_cartItems.Count > 0)
            {
                ParkCurrentCartAsDraft();
            }

            var draft = _parkedInvoices[index];
            _parkedInvoices.RemoveAt(index);

            _cartItems.Clear();
            foreach (var item in draft.Items)
            {
                _cartItems.Add(CloneCartItem(item));
            }

            ResetQtyBuffer();
            RefreshTotals();
            UpdateParkedButtonText();
            ShowAlert($"تم استعادة: {draft.Title}", isError: false);
            txtBarcode.Focus();
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
    }
}
