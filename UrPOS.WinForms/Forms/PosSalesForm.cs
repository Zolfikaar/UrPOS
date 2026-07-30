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
        private readonly IProductService _productService;
        private readonly IInvoiceService _invoiceService;
        private readonly BindingList<SalesInvoiceItem> _cartItems = new();
        private readonly ParkedInvoiceSession _parkedSession = ParkedInvoiceSession.Instance;

        private string _qtyBuffer = "1";
        private bool _qtyBufferTouched;
        private bool _isBusy;
        private Button? _btnManualItem;
        private int? _pendingParkedResumeIndex;

        public PosSalesForm(IProductService productService, IInvoiceService invoiceService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));

            InitializeComponent();
            BuildManualEntryButton();
            BuildNumpad();
            BindCart();
            RefreshTotals();
            UpdateQtyDisplay();
            UpdateParkedButtonText();
            FormClosing += PosSalesForm_FormClosing;
            Shown += PosSalesForm_ShownResumeParked;
        }

        /// <summary>
        /// Queues a parked draft to be restored when the form is shown (used from invoice history).
        /// </summary>
        public void ResumeParkedDraft(int draftIndex)
        {
            _pendingParkedResumeIndex = draftIndex;
        }

        private void PosSalesForm_ShownResumeParked(object? sender, EventArgs e)
        {
            if (_pendingParkedResumeIndex is not int index)
            {
                return;
            }

            _pendingParkedResumeIndex = null;
            RestoreParkedInvoice(index);
        }

        private void BuildManualEntryButton()
        {
            _btnManualItem = new Button
            {
                BackColor = Color.FromArgb(37, 99, 235),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Name = "btnManualItem",
                Size = new Size(160, 44),
                Text = "صنف يدوي",
                UseVisualStyleBackColor = false
            };
            _btnManualItem.FlatAppearance.BorderSize = 0;
            _btnManualItem.Click += (_, _) => ShowManualProductEntryDialog();

            var spacer = new Panel
            {
                Dock = DockStyle.Right,
                Width = 8,
                Name = "manualBtnSpacer"
            };

            // Dock.Right: last added is outermost right in LTR; with RTL layout keep next to existing buttons
            pnlMultiCart.Controls.Add(spacer);
            pnlMultiCart.Controls.Add(_btnManualItem);
        }

        private void PosSalesForm_Load(object? sender, EventArgs e)
        {
            try
            {
                // Keep ~360px for numpad side (RTL + maximize)
                var target = Math.Max(360, splitMain.Width - 880);
                if (splitMain.Width > 700)
                {
                    splitMain.SplitterDistance = Math.Max(500, splitMain.Width - target);
                }
            }
            catch
            {
                // Ignore invalid splitter distance during early layout
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            txtBarcode.Focus();
        }

        private void BindCart()
        {
            dgvCart.AutoGenerateColumns = false;

            // Columns are built here (not in the Designer) so Visual Studio
            // regenerations cannot wipe DataPropertyName bindings again.
            if (dgvCart.Columns.Count == 0)
            {
                var priceStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Format = "N2"
                };

                dgvCart.Columns.AddRange(
                    new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = nameof(SalesInvoiceItem.ProductName),
                        FillWeight = 42F,
                        HeaderText = "اسم المنتج",
                        MinimumWidth = 120,
                        Name = "colProduct",
                        ReadOnly = true
                    },
                    new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = nameof(SalesInvoiceItem.Quantity),
                        FillWeight = 16F,
                        HeaderText = "الكمية",
                        MinimumWidth = 70,
                        Name = "colQty",
                        ReadOnly = true
                    },
                    new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = nameof(SalesInvoiceItem.SalePrice),
                        DefaultCellStyle = priceStyle,
                        FillWeight = 20F,
                        HeaderText = "السعر المفرد",
                        MinimumWidth = 90,
                        Name = "colUnitPrice",
                        ReadOnly = true
                    },
                    new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = nameof(SalesInvoiceItem.TotalPrice),
                        DefaultCellStyle = priceStyle,
                        FillWeight = 22F,
                        HeaderText = "الإجمالي",
                        MinimumWidth = 90,
                        Name = "colTotal",
                        ReadOnly = true
                    });
            }

            dgvCart.DataSource = _cartItems;
        }

        private void BuildNumpad()
        {
            tblNumpad.SuspendLayout();
            tblNumpad.Controls.Clear();

            AddNumpadButton("7", 0, 0);
            AddNumpadButton("8", 1, 0);
            AddNumpadButton("9", 2, 0);
            AddNumpadButton("4", 0, 1);
            AddNumpadButton("5", 1, 1);
            AddNumpadButton("6", 2, 1);
            AddNumpadButton("1", 0, 2);
            AddNumpadButton("2", 1, 2);
            AddNumpadButton("3", 2, 2);
            AddNumpadButton("C", 0, 3, Color.FromArgb(245, 158, 11));
            AddNumpadButton("0", 1, 3);
            AddNumpadButton("BS", 2, 3, Color.FromArgb(100, 116, 139), "⌫");
            AddNumpadButton("+", 0, 4, Color.FromArgb(13, 148, 136), "+ كمية");
            AddNumpadButton("-", 1, 4, Color.FromArgb(239, 68, 68), "- كمية");
            AddNumpadButton("OK", 2, 4, Color.FromArgb(37, 99, 235), "إضافة");

            tblNumpad.ResumeLayout(true);
        }

        private void AddNumpadButton(string tag, int column, int row)
        {
            AddNumpadButton(tag, column, row, Color.FromArgb(241, 245, 249), tag, colored: false);
        }

        private void AddNumpadButton(string tag, int column, int row, Color backColor)
        {
            AddNumpadButton(tag, column, row, backColor, tag, colored: true);
        }

        private void AddNumpadButton(string tag, int column, int row, Color backColor, string text)
        {
            AddNumpadButton(tag, column, row, backColor, text, colored: true);
        }

        private void AddNumpadButton(string tag, int column, int row, Color backColor, string text, bool colored)
        {
            var button = new Button
            {
                BackColor = backColor,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = colored ? Color.White : Color.FromArgb(30, 41, 59),
                Margin = new Padding(4),
                Name = $"btnNum_{tag}_{column}_{row}",
                Tag = tag,
                Text = text,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            button.FlatAppearance.BorderSize = colored ? 0 : 1;
            button.Click += NumpadButton_Click;
            tblNumpad.Controls.Add(button, column, row);
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
            ShowAlert($"تم حفظ الفاتورة كمسودة. الفواتير المعلقة: {_parkedSession.Count}", isError: false);
            txtBarcode.Focus();
        }

        private void btnParkedInvoices_Click(object? sender, EventArgs e)
        {
            if (_parkedSession.Count == 0)
            {
                // Same rule as new invoice: if current cart has items, park it as draft
                if (_cartItems.Count > 0)
                {
                    ParkCurrentCartAsDraft();
                    BeginFreshInvoice();
                    ShowAlert($"تم تعليق الفاتورة الحالية. الفواتير المعلقة: {_parkedSession.Count}", isError: false);
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
            if (_cartItems.Count == 0)
            {
                return;
            }

            _parkedSession.Park(_cartItems);
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
            btnParkedInvoices.Text = $"الفواتير المعلقة ({_parkedSession.Count})";
        }

        private void PosSalesForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                return;
            }

            // Leaving POS with items in the cart → auto-park; empty cart → nothing.
            ParkCurrentCartAsDraft();
            _cartItems.Clear();
        }

        private void ShowParkedInvoicesPicker()
        {
            var parked = _parkedSession.Drafts;
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

            for (var i = 0; i < parked.Count; i++)
            {
                var d = parked[i];
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
            // Same logic as new invoice: park current open cart before switching
            if (_cartItems.Count > 0)
            {
                ParkCurrentCartAsDraft();
            }

            var draft = _parkedSession.Take(index);
            if (draft is null)
            {
                return;
            }

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
            catch (Exception ex)
            {
                ShowAlert($"تعذر البحث عن المنتج: {ex.Message}", isError: true);
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
            catch (Exception ex)
            {
                ShowAlert($"حدث خطأ أثناء إتمام البيع: {ex.Message}", isError: true);
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
            if (_btnManualItem is not null)
            {
                _btnManualItem.Enabled = !isBusy;
            }

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

        private void ShowManualProductEntryDialog()
        {
            using var dialog = new Form
            {
                Text = "إضافة صنف يدوي (بدون باركود)",
                RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ClientSize = new Size(440, 320),
                BackColor = Color.FromArgb(248, 250, 252),
                Font = new Font("Segoe UI", 10F)
            };

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(20),
                RightToLeft = RightToLeft.Yes,
                WrapContents = false
            };

            var txtName = CreateDialogTextBox("اسم الصنف");
            var numPrice = new NumericUpDown
            {
                DecimalPlaces = 2,
                Maximum = 999999999,
                Minimum = 0,
                Font = new Font("Segoe UI", 12F),
                Size = new Size(380, 32),
                Margin = new Padding(0, 0, 0, 12),
                ThousandsSeparator = true
            };
            var numQty = new NumericUpDown
            {
                Maximum = 99999,
                Minimum = 1,
                Value = ParseQtyBuffer() > 0 ? ParseQtyBuffer() : 1,
                Font = new Font("Segoe UI", 12F),
                Size = new Size(380, 32),
                Margin = new Padding(0, 0, 0, 12)
            };
            var cboUnit = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11F),
                Size = new Size(380, 32),
                Margin = new Padding(0, 0, 0, 12),
                RightToLeft = RightToLeft.Yes
            };
            cboUnit.Items.AddRange(ProductUnits.Common);
            cboUnit.SelectedIndex = 0;

            flow.Controls.Add(CreateDialogLabel("اسم الصنف / الوصف"));
            flow.Controls.Add(txtName);
            flow.Controls.Add(CreateDialogLabel("سعر الوحدة"));
            flow.Controls.Add(numPrice);
            flow.Controls.Add(CreateDialogLabel("الكمية"));
            flow.Controls.Add(numQty);
            flow.Controls.Add(CreateDialogLabel("وحدة القياس"));
            flow.Controls.Add(cboUnit);

            var buttons = new Panel { Dock = DockStyle.Bottom, Height = 56, Padding = new Padding(16, 8, 16, 8) };
            var btnOk = new Button
            {
                BackColor = Color.FromArgb(13, 148, 136),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Dock = DockStyle.Right,
                Width = 120,
                Text = "إضافة للسلة",
                DialogResult = DialogResult.None
            };
            btnOk.FlatAppearance.BorderSize = 0;

            var btnCancel = new Button
            {
                BackColor = Color.FromArgb(100, 116, 139),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Dock = DockStyle.Right,
                Width = 90,
                Text = "إلغاء",
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            buttons.Controls.Add(btnOk);
            buttons.Controls.Add(new Panel { Dock = DockStyle.Right, Width = 8 });
            buttons.Controls.Add(btnCancel);

            btnOk.Click += async (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show(dialog, "اسم الصنف مطلوب.", "تحقق", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    return;
                }

                if (numPrice.Value <= 0)
                {
                    MessageBox.Show(dialog, "أدخل سعراً أكبر من صفر.", "تحقق", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    return;
                }

                btnOk.Enabled = false;
                try
                {
                    var unit = cboUnit.SelectedItem?.ToString() ?? "قطعة";
                    var name = txtName.Text.Trim();
                    var qty = (int)numQty.Value;
                    var price = numPrice.Value;
                    var barcode = await GenerateUniqueManualBarcodeAsync();

                    var createResult = await _productService.CreateProductAsync(new Product
                    {
                        Barcode = barcode,
                        ProductName = name,
                        CostPrice = price,
                        SalePrice = price,
                        CurrentStock = Math.Max(qty, 1),
                        MinStockLevel = 0,
                        UnitOfMeasure = unit
                    });

                    if (!createResult.isSuccess)
                    {
                        MessageBox.Show(dialog,
                            string.IsNullOrWhiteSpace(createResult.ErrorMessage)
                                ? "تعذر حفظ الصنف اليدوي."
                                : createResult.ErrorMessage,
                            "فشل",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        return;
                    }

                    var product = await _productService.GetByBarcodeAsync(barcode);
                    if (product is null)
                    {
                        MessageBox.Show(dialog, "تم إنشاء الصنف لكن تعذر جلبه.", "فشل",
                            MessageBoxButtons.OK, MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        return;
                    }

                    AddOrIncrementCartItem(product, qty);
                    RefreshTotals();
                    HideAlert();
                    ShowAlert($"تمت إضافة الصنف اليدوي: {name} ({unit})", isError: false);
                    dialog.DialogResult = DialogResult.OK;
                    dialog.Close();
                    txtBarcode.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(dialog, $"حدث خطأ: {ex.Message}", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                }
                finally
                {
                    btnOk.Enabled = true;
                }
            };

            dialog.Controls.Add(flow);
            dialog.Controls.Add(buttons);
            dialog.AcceptButton = btnOk;
            dialog.CancelButton = btnCancel;
            dialog.ShowDialog(this);
        }

        private async Task<string> GenerateUniqueManualBarcodeAsync()
        {
            for (var attempt = 0; attempt < 12; attempt++)
            {
                var candidate = $"M{DateTime.Now:yyMMddHHmmss}{Random.Shared.Next(10, 99)}";
                var existing = await _productService.GetByBarcodeAsync(candidate);
                if (existing is null)
                {
                    return candidate;
                }

                await Task.Delay(15);
            }

            return $"M{Guid.NewGuid():N}"[..16];
        }

        private static Label CreateDialogLabel(string text) => new()
        {
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 65, 85),
            Margin = new Padding(0, 4, 0, 2),
            Text = text
        };

        private static TextBox CreateDialogTextBox(string placeholder) => new()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 12F),
            Margin = new Padding(0, 0, 0, 12),
            PlaceholderText = placeholder,
            RightToLeft = RightToLeft.Yes,
            Size = new Size(380, 32),
            TextAlign = HorizontalAlignment.Right
        };
    }
}
