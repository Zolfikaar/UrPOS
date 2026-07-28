using System.Drawing;
using System.Windows.Forms;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;

namespace UrPOS.WinForms.Forms
{
    /// <summary>
    /// Add / Edit product modal (Arabic RTL).
    /// Edit mode locks stock by default; adjustments require an explicit reason.
    /// </summary>
    public partial class ProductForm : Form
    {
        public sealed class ProductSavedEventArgs : EventArgs
        {
            public Product Product { get; }
            public bool IsEdit { get; }

            public ProductSavedEventArgs(Product product, bool isEdit)
            {
                Product = product;
                IsEdit = isEdit;
            }
        }

        private static readonly string[] StockAdjustmentReasons =
        [
            "خطأ في الإدخال الأولي",
            "تعديل جردي / تسوية",
            "تلف / بضاعة منتهية الصلاحية"
        ];

        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly int? _editingProductId;
        private int _originalStock;

        private CheckBox chkAdjustStock = null!;
        private Panel pnlStockReasonField = null!;
        private Label lblStockReason = null!;
        private ComboBox cboStockReason = null!;

        public bool IsEdit => _editingProductId.HasValue;

        /// <summary>Raised after a successful save so the parent can update the grid in-memory.</summary>
        public event EventHandler<ProductSavedEventArgs>? ProductSaved;

        public ProductForm(IProductRepository productRepository, IInventoryRepository inventoryRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
            InitializeComponent();
            BuildStockAdjustmentControls();

            numInitialStock.Enabled = true;
            SetStockAdjustmentUiVisible(false);
            lblFormTitle.Text = "إضافة منتج جديد";
            Text = "UrPOS — إضافة منتج جديد";
            numMinStock.Value = 5;
        }

        public ProductForm(
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            Product productToEdit)
            : this(productRepository, inventoryRepository)
        {
            ArgumentNullException.ThrowIfNull(productToEdit);

            _editingProductId = productToEdit.Id;
            FillFormFields(productToEdit);
            ConfigureEditStockLock();
        }

        private void BuildStockAdjustmentControls()
        {
            chkAdjustStock = new CheckBox
            {
                AutoSize = false,
                Checked = false,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(15, 23, 42),
                Margin = new Padding(0, 0, 0, 4),
                Name = "chkAdjustStock",
                RightToLeft = RightToLeft.Yes,
                Size = new Size(400, 28),
                Text = "تعديل الرصيد",
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false
            };
            chkAdjustStock.CheckedChanged += ChkAdjustStock_CheckedChanged;

            lblStockReason = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(51, 65, 85),
                Name = "lblStockReason",
                Size = new Size(400, 24),
                Text = "سبب التعديل المخزني",
                TextAlign = ContentAlignment.MiddleLeft
            };

            cboStockReason = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                Name = "cboStockReason",
                RightToLeft = RightToLeft.Yes,
                Size = new Size(400, 30)
            };
            cboStockReason.Items.AddRange(StockAdjustmentReasons);

            pnlStockReasonField = new Panel
            {
                Margin = new Padding(0, 0, 0, 4),
                Name = "pnlStockReasonField",
                RightToLeft = RightToLeft.Yes,
                Size = new Size(400, 68),
                Visible = false
            };
            pnlStockReasonField.Controls.Add(cboStockReason);
            pnlStockReasonField.Controls.Add(lblStockReason);

            var stockIndex = flowFields.Controls.GetChildIndex(pnlStockField);
            flowFields.Controls.Add(chkAdjustStock);
            flowFields.Controls.Add(pnlStockReasonField);
            flowFields.Controls.SetChildIndex(chkAdjustStock, stockIndex + 1);
            flowFields.Controls.SetChildIndex(pnlStockReasonField, stockIndex + 2);
        }

        private void FillFormFields(Product product)
        {
            txtBarcode.Text = product.Barcode;
            txtProductName.Text = product.ProductName;
            numCostPrice.Value = ClampDecimal(product.CostPrice, numCostPrice);
            numSalePrice.Value = ClampDecimal(product.SalePrice, numSalePrice);
            numInitialStock.Value = ClampInt(product.CurrentStock, numInitialStock);
            numMinStock.Value = ClampInt(product.MinStockLevel, numMinStock);
            _originalStock = (int)numInitialStock.Value;

            lblInitialStock.Text = "الكمية بالمخزون";
            lblFormTitle.Text = "تعديل منتج";
            Text = "UrPOS — تعديل منتج";
        }

        private void ConfigureEditStockLock()
        {
            numInitialStock.Enabled = false;
            chkAdjustStock.Visible = true;
            chkAdjustStock.Checked = false;
            pnlStockReasonField.Visible = false;
            cboStockReason.SelectedIndex = -1;
        }

        private void SetStockAdjustmentUiVisible(bool visible)
        {
            chkAdjustStock.Visible = visible;
            if (!visible)
            {
                pnlStockReasonField.Visible = false;
                chkAdjustStock.Checked = false;
            }
        }

        private void ChkAdjustStock_CheckedChanged(object? sender, EventArgs e)
        {
            if (!IsEdit)
            {
                return;
            }

            if (chkAdjustStock.Checked)
            {
                numInitialStock.Enabled = true;
                pnlStockReasonField.Visible = true;
                if (cboStockReason.SelectedIndex < 0)
                {
                    cboStockReason.SelectedIndex = -1;
                }

                lblValidation.Text = "فعّل تعديل الرصيد واختر سبب التعديل المخزني قبل تغيير الكمية.";
                lblValidation.ForeColor = Color.FromArgb(180, 83, 9);
                lblValidation.Visible = true;
            }
            else
            {
                numInitialStock.Value = ClampInt(_originalStock, numInitialStock);
                numInitialStock.Enabled = false;
                pnlStockReasonField.Visible = false;
                cboStockReason.SelectedIndex = -1;
                HideValidationError();
            }
        }

        private void btnGenerateBarcode_Click(object? sender, EventArgs e)
        {
            txtBarcode.Text = GenerateBarcodeStub();
            HideValidationError();
        }

        private void btnClearBarcode_Click(object? sender, EventArgs e)
        {
            txtBarcode.Clear();
            txtBarcode.Focus();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!TryValidate(out var message))
            {
                ShowValidationError(message);
                MessageBox.Show(
                    this,
                    message,
                    "تحقق من البيانات",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }

            HideValidationError();

            var newStock = (int)numInitialStock.Value;
            var stockChanged = IsEdit && chkAdjustStock.Checked && newStock != _originalStock;
            var adjustmentReason = stockChanged
                ? cboStockReason.SelectedItem?.ToString()?.Trim() ?? string.Empty
                : string.Empty;

            if (stockChanged)
            {
                var confirm = MessageBox.Show(
                    this,
                    $"سيتم تعديل كمية المخزون من {_originalStock} إلى {newStock}.\nالسبب: {adjustmentReason}\n\nهل تريد المتابعة؟",
                    "تأكيد تعديل المخزون",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

                if (confirm != DialogResult.Yes)
                {
                    return;
                }
            }

            var product = new Product
            {
                Id = _editingProductId ?? 0,
                Barcode = txtBarcode.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                CostPrice = numCostPrice.Value,
                SalePrice = numSalePrice.Value,
                CurrentStock = newStock,
                MinStockLevel = (int)numMinStock.Value
            };

            btnSave.Enabled = false;
            try
            {
                if (IsEdit)
                {
                    var updated = await _productRepository.UpdateAsync(product);
                    if (!updated)
                    {
                        ShowSaveFailure("تعذر تحديث المنتج. ربما تم حذفه.");
                        return;
                    }

                    if (stockChanged)
                    {
                        // AddMovementAsync records the reason and applies the stock delta atomically.
                        await _inventoryRepository.AddMovementAsync(new InventoryMovement
                        {
                            ProductId = product.Id,
                            MovementType = "ADJUST",
                            Quantity = newStock - _originalStock,
                            Notes = adjustmentReason
                        });
                    }
                }
                else
                {
                    product.Id = await _productRepository.AddAsync(product);
                }

                ProductSaved?.Invoke(this, new ProductSavedEventArgs(product, IsEdit));
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowSaveFailure($"حدث خطأ أثناء الحفظ:\n{ex.Message}");
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void ShowSaveFailure(string message)
        {
            ShowValidationError(message);
            MessageBox.Show(
                this,
                message,
                "فشل الحفظ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private void ShowValidationError(string message)
        {
            lblValidation.Text = message;
            lblValidation.ForeColor = Color.FromArgb(220, 38, 38);
            lblValidation.Visible = true;
        }

        private void HideValidationError()
        {
            lblValidation.Visible = false;
            lblValidation.ForeColor = Color.FromArgb(220, 38, 38);
        }

        private bool TryValidate(out string message)
        {
            if (string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                message = "الباركود مطلوب.";
                txtBarcode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                message = "اسم المنتج مطلوب.";
                txtProductName.Focus();
                return false;
            }

            if (numCostPrice.Value < 0)
            {
                message = "سعر الشراء غير صالح.";
                numCostPrice.Focus();
                return false;
            }

            if (numSalePrice.Value < 0)
            {
                message = "سعر البيع غير صالح.";
                numSalePrice.Focus();
                return false;
            }

            if (numSalePrice.Value < numCostPrice.Value)
            {
                message = "سعر البيع لا يمكن أن يكون أقل من سعر الشراء.";
                numSalePrice.Focus();
                return false;
            }

            if (numInitialStock.Value < 0)
            {
                message = "كمية المخزون غير صالحة.";
                numInitialStock.Focus();
                return false;
            }

            if (numMinStock.Value < 0)
            {
                message = "حد التنبيه للمخزون غير صالح.";
                numMinStock.Focus();
                return false;
            }

            if (IsEdit && chkAdjustStock.Checked)
            {
                var newStock = (int)numInitialStock.Value;
                if (newStock != _originalStock)
                {
                    if (cboStockReason.SelectedIndex < 0 ||
                        string.IsNullOrWhiteSpace(cboStockReason.SelectedItem?.ToString()))
                    {
                        message = "يجب اختيار سبب التعديل المخزني قبل حفظ تغيير الكمية.";
                        cboStockReason.Focus();
                        return false;
                    }
                }
            }

            message = string.Empty;
            return true;
        }

        private static string GenerateBarcodeStub()
        {
            var stamp = DateTime.Now.ToString("yyMMddHHmmss");
            return $"628{stamp}";
        }

        private static decimal ClampDecimal(decimal value, NumericUpDown control)
        {
            if (value < control.Minimum) return control.Minimum;
            if (value > control.Maximum) return control.Maximum;
            return value;
        }

        private static int ClampInt(int value, NumericUpDown control)
        {
            if (value < control.Minimum) return (int)control.Minimum;
            if (value > control.Maximum) return (int)control.Maximum;
            return value;
        }
    }
}
