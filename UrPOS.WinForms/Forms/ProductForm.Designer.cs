using System.Drawing;
using System.Windows.Forms;

namespace UrPOS.WinForms.Forms
{
    partial class ProductForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlAccent;
        private Panel pnlBody;
        private Label lblFormTitle;
        private Label lblFormSubtitle;
        private Panel pnlFields;
        private FlowLayoutPanel flowFields;
        private Panel pnlBarcodeField;
        private Label lblBarcode;
        private TextBox txtBarcode;
        private FlowLayoutPanel pnlBarcodeActions;
        private Button btnGenerateBarcode;
        private Button btnClearBarcode;
        private Panel pnlNameField;
        private Label lblProductName;
        private TextBox txtProductName;
        private Panel pnlCostField;
        private Label lblCostPrice;
        private NumericUpDown numCostPrice;
        private Panel pnlSaleField;
        private Label lblSalePrice;
        private NumericUpDown numSalePrice;
        private Panel pnlStockField;
        private Label lblInitialStock;
        private NumericUpDown numInitialStock;
        private Panel pnlMinStockField;
        private Label lblMinStock;
        private NumericUpDown numMinStock;
        private Label lblValidation;
        private Panel pnlActions;
        private Button btnSave;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            pnlAccent = new Panel();
            pnlBody = new Panel();
            lblFormTitle = new Label();
            lblFormSubtitle = new Label();
            pnlFields = new Panel();
            flowFields = new FlowLayoutPanel();
            pnlBarcodeField = new Panel();
            lblBarcode = new Label();
            txtBarcode = new TextBox();
            pnlBarcodeActions = new FlowLayoutPanel();
            btnGenerateBarcode = new Button();
            btnClearBarcode = new Button();
            pnlNameField = new Panel();
            lblProductName = new Label();
            txtProductName = new TextBox();
            pnlCostField = new Panel();
            lblCostPrice = new Label();
            numCostPrice = new NumericUpDown();
            pnlSaleField = new Panel();
            lblSalePrice = new Label();
            numSalePrice = new NumericUpDown();
            pnlStockField = new Panel();
            lblInitialStock = new Label();
            numInitialStock = new NumericUpDown();
            pnlMinStockField = new Panel();
            lblMinStock = new Label();
            numMinStock = new NumericUpDown();
            lblValidation = new Label();
            pnlActions = new Panel();
            btnSave = new Button();
            btnCancel = new Button();

            ((System.ComponentModel.ISupportInitialize)numCostPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSalePrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numInitialStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinStock).BeginInit();
            SuspendLayout();
            pnlBody.SuspendLayout();
            pnlFields.SuspendLayout();
            flowFields.SuspendLayout();
            pnlBarcodeField.SuspendLayout();
            pnlBarcodeActions.SuspendLayout();
            pnlNameField.SuspendLayout();
            pnlCostField.SuspendLayout();
            pnlSaleField.SuspendLayout();
            pnlStockField.SuspendLayout();
            pnlMinStockField.SuspendLayout();
            pnlActions.SuspendLayout();

            // 
            // pnlAccent
            // 
            pnlAccent.BackColor = Color.FromArgb(13, 148, 136);
            pnlAccent.Dock = DockStyle.Top;
            pnlAccent.Location = new Point(0, 0);
            pnlAccent.Name = "pnlAccent";
            pnlAccent.Size = new Size(480, 6);
            pnlAccent.TabIndex = 0;

            // 
            // pnlBody
            // 
            pnlBody.Controls.Add(pnlFields);
            pnlBody.Controls.Add(lblValidation);
            pnlBody.Controls.Add(pnlActions);
            pnlBody.Controls.Add(lblFormSubtitle);
            pnlBody.Controls.Add(lblFormTitle);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 6);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(28, 20, 28, 16);
            pnlBody.RightToLeft = RightToLeft.Yes;
            pnlBody.Size = new Size(480, 614);
            pnlBody.TabIndex = 1;

            // 
            // lblFormTitle
            // 
            lblFormTitle.Dock = DockStyle.Top;
            lblFormTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblFormTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblFormTitle.Location = new Point(28, 20);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.Size = new Size(424, 36);
            lblFormTitle.TabIndex = 0;
            lblFormTitle.Text = "إضافة منتج جديد";
            lblFormTitle.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // lblFormSubtitle
            // 
            lblFormSubtitle.Dock = DockStyle.Top;
            lblFormSubtitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblFormSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblFormSubtitle.Location = new Point(28, 56);
            lblFormSubtitle.Name = "lblFormSubtitle";
            lblFormSubtitle.Size = new Size(424, 28);
            lblFormSubtitle.TabIndex = 1;
            lblFormSubtitle.Text = "الحقول المطلوبة: الباركود، الاسم، سعر الشراء، سعر البيع.";
            lblFormSubtitle.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // pnlFields
            // 
            pnlFields.Controls.Add(flowFields);
            pnlFields.Dock = DockStyle.Fill;
            pnlFields.Location = new Point(28, 84);
            pnlFields.Name = "pnlFields";
            pnlFields.RightToLeft = RightToLeft.Yes;
            pnlFields.Size = new Size(424, 422);
            pnlFields.TabIndex = 2;

            // 
            // flowFields
            // 
            flowFields.AutoScroll = true;
            flowFields.Controls.Add(pnlBarcodeField);
            flowFields.Controls.Add(pnlBarcodeActions);
            flowFields.Controls.Add(pnlNameField);
            flowFields.Controls.Add(pnlCostField);
            flowFields.Controls.Add(pnlSaleField);
            flowFields.Controls.Add(pnlStockField);
            flowFields.Controls.Add(pnlMinStockField);
            flowFields.Dock = DockStyle.Fill;
            flowFields.FlowDirection = FlowDirection.TopDown;
            flowFields.Location = new Point(0, 0);
            flowFields.Name = "flowFields";
            flowFields.RightToLeft = RightToLeft.Yes;
            flowFields.Size = new Size(424, 422);
            flowFields.TabIndex = 0;
            flowFields.WrapContents = false;

            // 
            // pnlBarcodeField
            // 
            pnlBarcodeField.Controls.Add(txtBarcode);
            pnlBarcodeField.Controls.Add(lblBarcode);
            pnlBarcodeField.Location = new Point(12, 3);
            pnlBarcodeField.Margin = new Padding(0, 0, 0, 4);
            pnlBarcodeField.Name = "pnlBarcodeField";
            pnlBarcodeField.RightToLeft = RightToLeft.Yes;
            pnlBarcodeField.Size = new Size(400, 68);
            pnlBarcodeField.TabIndex = 0;

            // 
            // lblBarcode
            // 
            lblBarcode.Dock = DockStyle.Top;
            lblBarcode.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblBarcode.ForeColor = Color.FromArgb(51, 65, 85);
            lblBarcode.Location = new Point(0, 0);
            lblBarcode.Name = "lblBarcode";
            lblBarcode.Size = new Size(400, 24);
            lblBarcode.TabIndex = 0;
            lblBarcode.Text = "الباركود *";
            lblBarcode.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // txtBarcode
            // 
            txtBarcode.BorderStyle = BorderStyle.FixedSingle;
            txtBarcode.Dock = DockStyle.Top;
            txtBarcode.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtBarcode.Location = new Point(0, 24);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.PlaceholderText = "أدخل الباركود أو ولّده تلقائياً";
            txtBarcode.RightToLeft = RightToLeft.Yes;
            txtBarcode.Size = new Size(400, 30);
            txtBarcode.TabIndex = 1;
            txtBarcode.TextAlign = HorizontalAlignment.Right;

            // 
            // pnlBarcodeActions
            // 
            pnlBarcodeActions.AutoSize = true;
            pnlBarcodeActions.Controls.Add(btnGenerateBarcode);
            pnlBarcodeActions.Controls.Add(btnClearBarcode);
            pnlBarcodeActions.FlowDirection = FlowDirection.RightToLeft;
            pnlBarcodeActions.Location = new Point(12, 75);
            pnlBarcodeActions.Margin = new Padding(0, 0, 0, 8);
            pnlBarcodeActions.Name = "pnlBarcodeActions";
            pnlBarcodeActions.RightToLeft = RightToLeft.Yes;
            pnlBarcodeActions.Size = new Size(400, 38);
            pnlBarcodeActions.TabIndex = 1;
            pnlBarcodeActions.WrapContents = false;

            // 
            // btnGenerateBarcode
            // 
            btnGenerateBarcode.AutoSize = true;
            btnGenerateBarcode.BackColor = Color.FromArgb(226, 232, 240);
            btnGenerateBarcode.Cursor = Cursors.Hand;
            btnGenerateBarcode.FlatAppearance.BorderSize = 0;
            btnGenerateBarcode.FlatStyle = FlatStyle.Flat;
            btnGenerateBarcode.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnGenerateBarcode.ForeColor = Color.FromArgb(30, 41, 59);
            btnGenerateBarcode.Location = new Point(292, 0);
            btnGenerateBarcode.Margin = new Padding(0, 0, 8, 0);
            btnGenerateBarcode.MinimumSize = new Size(100, 32);
            btnGenerateBarcode.Name = "btnGenerateBarcode";
            btnGenerateBarcode.Size = new Size(100, 32);
            btnGenerateBarcode.TabIndex = 0;
            btnGenerateBarcode.Text = "توليد تلقائي";
            btnGenerateBarcode.UseVisualStyleBackColor = false;
            btnGenerateBarcode.Click += btnGenerateBarcode_Click;

            // 
            // btnClearBarcode
            // 
            btnClearBarcode.AutoSize = true;
            btnClearBarcode.BackColor = Color.FromArgb(226, 232, 240);
            btnClearBarcode.Cursor = Cursors.Hand;
            btnClearBarcode.FlatAppearance.BorderSize = 0;
            btnClearBarcode.FlatStyle = FlatStyle.Flat;
            btnClearBarcode.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnClearBarcode.ForeColor = Color.FromArgb(30, 41, 59);
            btnClearBarcode.Location = new Point(184, 0);
            btnClearBarcode.Margin = new Padding(0, 0, 8, 0);
            btnClearBarcode.MinimumSize = new Size(100, 32);
            btnClearBarcode.Name = "btnClearBarcode";
            btnClearBarcode.Size = new Size(100, 32);
            btnClearBarcode.TabIndex = 1;
            btnClearBarcode.Text = "مسح";
            btnClearBarcode.UseVisualStyleBackColor = false;
            btnClearBarcode.Click += btnClearBarcode_Click;

            // 
            // pnlNameField
            // 
            pnlNameField.Controls.Add(txtProductName);
            pnlNameField.Controls.Add(lblProductName);
            pnlNameField.Location = new Point(12, 121);
            pnlNameField.Margin = new Padding(0, 0, 0, 4);
            pnlNameField.Name = "pnlNameField";
            pnlNameField.RightToLeft = RightToLeft.Yes;
            pnlNameField.Size = new Size(400, 68);
            pnlNameField.TabIndex = 2;

            // 
            // lblProductName
            // 
            lblProductName.Dock = DockStyle.Top;
            lblProductName.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblProductName.ForeColor = Color.FromArgb(51, 65, 85);
            lblProductName.Location = new Point(0, 0);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(400, 24);
            lblProductName.TabIndex = 0;
            lblProductName.Text = "اسم المنتج *";
            lblProductName.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // txtProductName
            // 
            txtProductName.BorderStyle = BorderStyle.FixedSingle;
            txtProductName.Dock = DockStyle.Top;
            txtProductName.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtProductName.Location = new Point(0, 24);
            txtProductName.Name = "txtProductName";
            txtProductName.PlaceholderText = "اسم المنتج كما يظهر على الإيصال";
            txtProductName.RightToLeft = RightToLeft.Yes;
            txtProductName.Size = new Size(400, 30);
            txtProductName.TabIndex = 1;
            txtProductName.TextAlign = HorizontalAlignment.Right;

            // 
            // pnlCostField
            // 
            pnlCostField.Controls.Add(numCostPrice);
            pnlCostField.Controls.Add(lblCostPrice);
            pnlCostField.Location = new Point(12, 193);
            pnlCostField.Margin = new Padding(0, 0, 0, 4);
            pnlCostField.Name = "pnlCostField";
            pnlCostField.RightToLeft = RightToLeft.Yes;
            pnlCostField.Size = new Size(400, 68);
            pnlCostField.TabIndex = 3;

            // 
            // lblCostPrice
            // 
            lblCostPrice.Dock = DockStyle.Top;
            lblCostPrice.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblCostPrice.ForeColor = Color.FromArgb(51, 65, 85);
            lblCostPrice.Location = new Point(0, 0);
            lblCostPrice.Name = "lblCostPrice";
            lblCostPrice.Size = new Size(400, 24);
            lblCostPrice.TabIndex = 0;
            lblCostPrice.Text = "سعر الشراء *";
            lblCostPrice.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // numCostPrice
            // 
            numCostPrice.BorderStyle = BorderStyle.FixedSingle;
            numCostPrice.DecimalPlaces = 2;
            numCostPrice.Dock = DockStyle.Top;
            numCostPrice.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numCostPrice.Location = new Point(0, 24);
            numCostPrice.Maximum = new decimal(new int[] { 99999999, 0, 0, 131072 });
            numCostPrice.Name = "numCostPrice";
            numCostPrice.RightToLeft = RightToLeft.Yes;
            numCostPrice.Size = new Size(400, 30);
            numCostPrice.TabIndex = 1;
            numCostPrice.TextAlign = HorizontalAlignment.Right;
            numCostPrice.ThousandsSeparator = true;

            // 
            // pnlSaleField
            // 
            pnlSaleField.Controls.Add(numSalePrice);
            pnlSaleField.Controls.Add(lblSalePrice);
            pnlSaleField.Location = new Point(12, 265);
            pnlSaleField.Margin = new Padding(0, 0, 0, 4);
            pnlSaleField.Name = "pnlSaleField";
            pnlSaleField.RightToLeft = RightToLeft.Yes;
            pnlSaleField.Size = new Size(400, 68);
            pnlSaleField.TabIndex = 4;

            // 
            // lblSalePrice
            // 
            lblSalePrice.Dock = DockStyle.Top;
            lblSalePrice.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblSalePrice.ForeColor = Color.FromArgb(51, 65, 85);
            lblSalePrice.Location = new Point(0, 0);
            lblSalePrice.Name = "lblSalePrice";
            lblSalePrice.Size = new Size(400, 24);
            lblSalePrice.TabIndex = 0;
            lblSalePrice.Text = "سعر البيع *";
            lblSalePrice.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // numSalePrice
            // 
            numSalePrice.BorderStyle = BorderStyle.FixedSingle;
            numSalePrice.DecimalPlaces = 2;
            numSalePrice.Dock = DockStyle.Top;
            numSalePrice.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numSalePrice.Location = new Point(0, 24);
            numSalePrice.Maximum = new decimal(new int[] { 99999999, 0, 0, 131072 });
            numSalePrice.Name = "numSalePrice";
            numSalePrice.RightToLeft = RightToLeft.Yes;
            numSalePrice.Size = new Size(400, 30);
            numSalePrice.TabIndex = 1;
            numSalePrice.TextAlign = HorizontalAlignment.Right;
            numSalePrice.ThousandsSeparator = true;

            // 
            // pnlStockField
            // 
            pnlStockField.Controls.Add(numInitialStock);
            pnlStockField.Controls.Add(lblInitialStock);
            pnlStockField.Location = new Point(12, 337);
            pnlStockField.Margin = new Padding(0, 0, 0, 4);
            pnlStockField.Name = "pnlStockField";
            pnlStockField.RightToLeft = RightToLeft.Yes;
            pnlStockField.Size = new Size(400, 68);
            pnlStockField.TabIndex = 5;

            // 
            // lblInitialStock
            // 
            lblInitialStock.Dock = DockStyle.Top;
            lblInitialStock.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblInitialStock.ForeColor = Color.FromArgb(51, 65, 85);
            lblInitialStock.Location = new Point(0, 0);
            lblInitialStock.Name = "lblInitialStock";
            lblInitialStock.Size = new Size(400, 24);
            lblInitialStock.TabIndex = 0;
            lblInitialStock.Text = "الكمية الابتدائية بالمخزون";
            lblInitialStock.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // numInitialStock
            // 
            numInitialStock.BorderStyle = BorderStyle.FixedSingle;
            numInitialStock.Dock = DockStyle.Top;
            numInitialStock.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numInitialStock.Location = new Point(0, 24);
            numInitialStock.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numInitialStock.Name = "numInitialStock";
            numInitialStock.RightToLeft = RightToLeft.Yes;
            numInitialStock.Size = new Size(400, 30);
            numInitialStock.TabIndex = 1;
            numInitialStock.TextAlign = HorizontalAlignment.Right;
            numInitialStock.ThousandsSeparator = true;

            // 
            // pnlMinStockField
            // 
            pnlMinStockField.Controls.Add(numMinStock);
            pnlMinStockField.Controls.Add(lblMinStock);
            pnlMinStockField.Location = new Point(12, 409);
            pnlMinStockField.Margin = new Padding(0, 0, 0, 4);
            pnlMinStockField.Name = "pnlMinStockField";
            pnlMinStockField.RightToLeft = RightToLeft.Yes;
            pnlMinStockField.Size = new Size(400, 68);
            pnlMinStockField.TabIndex = 6;

            // 
            // lblMinStock
            // 
            lblMinStock.Dock = DockStyle.Top;
            lblMinStock.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblMinStock.ForeColor = Color.FromArgb(51, 65, 85);
            lblMinStock.Location = new Point(0, 0);
            lblMinStock.Name = "lblMinStock";
            lblMinStock.Size = new Size(400, 24);
            lblMinStock.TabIndex = 0;
            lblMinStock.Text = "تنبيه أصل المخزون";
            lblMinStock.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // numMinStock
            // 
            numMinStock.BorderStyle = BorderStyle.FixedSingle;
            numMinStock.Dock = DockStyle.Top;
            numMinStock.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numMinStock.Location = new Point(0, 24);
            numMinStock.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numMinStock.Name = "numMinStock";
            numMinStock.RightToLeft = RightToLeft.Yes;
            numMinStock.Size = new Size(400, 30);
            numMinStock.TabIndex = 1;
            numMinStock.TextAlign = HorizontalAlignment.Right;
            numMinStock.ThousandsSeparator = true;
            numMinStock.Value = new decimal(new int[] { 5, 0, 0, 0 });

            // 
            // lblValidation
            // 
            lblValidation.Dock = DockStyle.Bottom;
            lblValidation.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblValidation.ForeColor = Color.FromArgb(220, 38, 38);
            lblValidation.Location = new Point(28, 506);
            lblValidation.Name = "lblValidation";
            lblValidation.RightToLeft = RightToLeft.Yes;
            lblValidation.Size = new Size(424, 36);
            lblValidation.TabIndex = 3;
            lblValidation.TextAlign = ContentAlignment.MiddleLeft;
            lblValidation.Visible = false;

            // 
            // pnlActions
            // 
            pnlActions.Controls.Add(btnCancel);
            pnlActions.Controls.Add(btnSave);
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Location = new Point(28, 542);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new Padding(0, 8, 0, 0);
            pnlActions.RightToLeft = RightToLeft.Yes;
            pnlActions.Size = new Size(424, 56);
            pnlActions.TabIndex = 4;

            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(13, 148, 136);
            btnSave.Cursor = Cursors.Hand;
            btnSave.Dock = DockStyle.Left;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(184, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 48);
            btnSave.TabIndex = 0;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(100, 116, 139);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Dock = DockStyle.Left;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(304, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 48);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;

            // 
            // ProductForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(248, 250, 252);
            CancelButton = btnCancel;
            ClientSize = new Size(480, 620);
            Controls.Add(pnlBody);
            Controls.Add(pnlAccent);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProductForm";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "UrPOS — منتج";

            pnlBarcodeField.ResumeLayout(false);
            pnlBarcodeField.PerformLayout();
            pnlBarcodeActions.ResumeLayout(false);
            pnlBarcodeActions.PerformLayout();
            pnlNameField.ResumeLayout(false);
            pnlNameField.PerformLayout();
            pnlCostField.ResumeLayout(false);
            pnlSaleField.ResumeLayout(false);
            pnlStockField.ResumeLayout(false);
            pnlMinStockField.ResumeLayout(false);
            flowFields.ResumeLayout(false);
            flowFields.PerformLayout();
            pnlFields.ResumeLayout(false);
            pnlActions.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numCostPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSalePrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numInitialStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinStock).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
