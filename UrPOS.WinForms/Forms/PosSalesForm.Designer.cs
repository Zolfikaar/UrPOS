using System.Drawing;
using System.Windows.Forms;

namespace UrPOS.WinForms.Forms
{
    partial class PosSalesForm
    {
        private System.ComponentModel.IContainer components = null;

        private SplitContainer splitMain;
        private Panel pnlLeft;
        private Panel pnlRight;
        private Panel pnlMultiCart;
        private Button btnNewInvoice;
        private Button btnParkedInvoices;
        private Label lblCartTitle;
        private DataGridView dgvCart;
        private Panel pnlCartActions;
        private Button btnRemoveItem;
        private Button btnClearCart;
        private Panel pnlSearch;
        private Label lblSearchTitle;
        private TextBox txtBarcode;
        private Button btnSearch;
        private Label lblQtyCaption;
        private Label lblQtyValue;
        private TableLayoutPanel tblNumpad;
        private Panel pnlTotals;
        private Label lblNetCaption;
        private Label lblNetAmount;
        private Label lblItemsCount;
        private Button btnCheckout;
        private Label lblAlert;
        private Label lblCurrency;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            splitMain = new SplitContainer();
            pnlLeft = new Panel();
            pnlRight = new Panel();
            pnlMultiCart = new Panel();
            btnNewInvoice = new Button();
            btnParkedInvoices = new Button();
            lblCartTitle = new Label();
            dgvCart = new DataGridView();
            pnlCartActions = new Panel();
            btnRemoveItem = new Button();
            btnClearCart = new Button();
            pnlSearch = new Panel();
            lblSearchTitle = new Label();
            txtBarcode = new TextBox();
            btnSearch = new Button();
            lblQtyCaption = new Label();
            lblQtyValue = new Label();
            tblNumpad = new TableLayoutPanel();
            pnlTotals = new Panel();
            lblNetCaption = new Label();
            lblNetAmount = new Label();
            lblItemsCount = new Label();
            btnCheckout = new Button();
            lblAlert = new Label();
            lblCurrency = new Label();

            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            pnlLeft.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlMultiCart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            pnlCartActions.SuspendLayout();
            pnlSearch.SuspendLayout();
            tblNumpad.SuspendLayout();
            pnlTotals.SuspendLayout();
            SuspendLayout();

            // PosSalesForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1280, 800);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            MinimumSize = new Size(1100, 700);
            Name = "PosSalesForm";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "UrPOS — شاشة الكاشير";
            WindowState = FormWindowState.Maximized;

            // splitMain
            splitMain.Dock = DockStyle.Fill;
            splitMain.SplitterDistance = 720;
            splitMain.SplitterWidth = 8;

            // ===================== LEFT: Cart =====================
            pnlLeft.BackColor = Color.FromArgb(248, 250, 252);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Padding = new Padding(16);
            pnlLeft.RightToLeft = RightToLeft.Yes;

            // Multi-cart / parked-orders toolbar (UI placeholders)
            pnlMultiCart.Dock = DockStyle.Top;
            pnlMultiCart.Height = 52;
            pnlMultiCart.Name = "pnlMultiCart";
            pnlMultiCart.Padding = new Padding(0, 0, 0, 8);
            pnlMultiCart.RightToLeft = RightToLeft.Yes;

            btnNewInvoice.BackColor = Color.FromArgb(13, 148, 136);
            btnNewInvoice.Cursor = Cursors.Hand;
            btnNewInvoice.Dock = DockStyle.Right;
            btnNewInvoice.FlatAppearance.BorderSize = 0;
            btnNewInvoice.FlatStyle = FlatStyle.Flat;
            btnNewInvoice.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnNewInvoice.ForeColor = Color.White;
            btnNewInvoice.Name = "btnNewInvoice";
            btnNewInvoice.Size = new Size(160, 40);
            btnNewInvoice.Text = "+ فاتورة جديدة";
            btnNewInvoice.UseVisualStyleBackColor = false;
            btnNewInvoice.Click += btnNewInvoice_Click;

            var multiCartSpacer = new Panel
            {
                Dock = DockStyle.Right,
                Width = 8,
                Name = "spMultiCart"
            };

            btnParkedInvoices.BackColor = Color.FromArgb(51, 65, 85);
            btnParkedInvoices.Cursor = Cursors.Hand;
            btnParkedInvoices.Dock = DockStyle.Right;
            btnParkedInvoices.FlatAppearance.BorderSize = 0;
            btnParkedInvoices.FlatStyle = FlatStyle.Flat;
            btnParkedInvoices.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnParkedInvoices.ForeColor = Color.White;
            btnParkedInvoices.Name = "btnParkedInvoices";
            btnParkedInvoices.Size = new Size(180, 40);
            btnParkedInvoices.Text = "الفواتير المعلقة (0)";
            btnParkedInvoices.UseVisualStyleBackColor = false;
            btnParkedInvoices.Click += btnParkedInvoices_Click;

            pnlMultiCart.Controls.Add(btnParkedInvoices);
            pnlMultiCart.Controls.Add(multiCartSpacer);
            pnlMultiCart.Controls.Add(btnNewInvoice);

            lblCartTitle.Dock = DockStyle.Top;
            lblCartTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblCartTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblCartTitle.Height = 40;
            lblCartTitle.Name = "lblCartTitle";
            lblCartTitle.Text = "سلة المبيعات الحالية";
            lblCartTitle.TextAlign = ContentAlignment.MiddleRight;

            lblAlert.Dock = DockStyle.Top;
            lblAlert.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblAlert.Height = 40;
            lblAlert.Name = "lblAlert";
            lblAlert.Padding = new Padding(10, 8, 10, 8);
            lblAlert.TextAlign = ContentAlignment.MiddleCenter;
            lblAlert.Visible = false;

            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.AllowUserToResizeRows = false;
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCart.BackgroundColor = Color.White;
            dgvCart.BorderStyle = BorderStyle.None;
            dgvCart.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCart.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCart.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = Color.FromArgb(15, 23, 42),
                SelectionForeColor = Color.White,
                Padding = new Padding(4)
            };
            dgvCart.ColumnHeadersHeight = 42;
            dgvCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCart.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 41, 59),
                Font = new Font("Segoe UI", 11F),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = Color.FromArgb(204, 251, 241),
                SelectionForeColor = Color.FromArgb(15, 23, 42),
                Padding = new Padding(4)
            };
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.GridColor = Color.FromArgb(226, 232, 240);
            dgvCart.MultiSelect = false;
            dgvCart.Name = "dgvCart";
            dgvCart.ReadOnly = true;
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowTemplate.Height = 40;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvCart.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ProductName",
                    HeaderText = "المنتج",
                    Name = "colProduct",
                    FillWeight = 40
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Quantity",
                    HeaderText = "الكمية",
                    Name = "colQty",
                    FillWeight = 15
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "SalePrice",
                    HeaderText = "سعر الوحدة",
                    Name = "colUnitPrice",
                    FillWeight = 20,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "TotalPrice",
                    HeaderText = "الإجمالي",
                    Name = "colTotal",
                    FillWeight = 20,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
                });

            pnlCartActions.Dock = DockStyle.Bottom;
            pnlCartActions.Height = 56;
            pnlCartActions.Padding = new Padding(0, 8, 0, 0);

            btnRemoveItem.BackColor = Color.FromArgb(239, 68, 68);
            btnRemoveItem.Cursor = Cursors.Hand;
            btnRemoveItem.Dock = DockStyle.Right;
            btnRemoveItem.FlatAppearance.BorderSize = 0;
            btnRemoveItem.FlatStyle = FlatStyle.Flat;
            btnRemoveItem.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnRemoveItem.ForeColor = Color.White;
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(140, 40);
            btnRemoveItem.Text = "حذف الصنف";
            btnRemoveItem.UseVisualStyleBackColor = false;
            btnRemoveItem.Click += btnRemoveItem_Click;

            btnClearCart.BackColor = Color.FromArgb(100, 116, 139);
            btnClearCart.Cursor = Cursors.Hand;
            btnClearCart.Dock = DockStyle.Right;
            btnClearCart.FlatAppearance.BorderSize = 0;
            btnClearCart.FlatStyle = FlatStyle.Flat;
            btnClearCart.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnClearCart.ForeColor = Color.White;
            btnClearCart.Margin = new Padding(8, 0, 0, 0);
            btnClearCart.Name = "btnClearCart";
            btnClearCart.Size = new Size(140, 40);
            btnClearCart.Text = "تفريغ السلة";
            btnClearCart.UseVisualStyleBackColor = false;
            btnClearCart.Click += btnClearCart_Click;

            var cartBtnSpacer = new Panel { Dock = DockStyle.Right, Width = 8 };
            pnlCartActions.Controls.Add(btnRemoveItem);
            pnlCartActions.Controls.Add(cartBtnSpacer);
            pnlCartActions.Controls.Add(btnClearCart);

            // Dock order: Fill first, then Bottom, then Top (last Top is highest)
            pnlLeft.Controls.Add(dgvCart);
            pnlLeft.Controls.Add(pnlCartActions);
            pnlLeft.Controls.Add(lblAlert);
            pnlLeft.Controls.Add(lblCartTitle);
            pnlLeft.Controls.Add(pnlMultiCart);

            // ===================== RIGHT: Search + Numpad + Totals =====================
            pnlRight.BackColor = Color.FromArgb(255, 255, 255);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Padding = new Padding(16);

            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Height = 130;

            lblSearchTitle.Dock = DockStyle.Top;
            lblSearchTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblSearchTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblSearchTitle.Height = 32;
            lblSearchTitle.Name = "lblSearchTitle";
            lblSearchTitle.Text = "بحث سريع (باركود / اسم)";
            lblSearchTitle.TextAlign = ContentAlignment.MiddleRight;

            var searchRow = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                Name = "pnlSearchRow"
            };

            txtBarcode.BorderStyle = BorderStyle.FixedSingle;
            txtBarcode.Dock = DockStyle.Fill;
            txtBarcode.Font = new Font("Segoe UI", 14F);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.PlaceholderText = "امسح الباركود أو اكتب اسم المنتج ثم Enter";
            txtBarcode.RightToLeft = RightToLeft.Yes;
            txtBarcode.TabIndex = 0;
            txtBarcode.TextAlign = HorizontalAlignment.Right;
            txtBarcode.KeyDown += txtBarcode_KeyDown;

            btnSearch.BackColor = Color.FromArgb(13, 148, 136);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.Dock = DockStyle.Left;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(100, 44);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "بحث";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;

            searchRow.Controls.Add(txtBarcode);
            searchRow.Controls.Add(btnSearch);

            var qtyRow = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(0, 8, 0, 0),
                Name = "pnlQtyRow"
            };

            lblQtyCaption.AutoSize = true;
            lblQtyCaption.Font = new Font("Segoe UI", 10F);
            lblQtyCaption.ForeColor = Color.FromArgb(71, 85, 105);
            lblQtyCaption.Location = new Point(8, 12);
            lblQtyCaption.Name = "lblQtyCaption";
            lblQtyCaption.Text = "الكمية القادمة:";

            lblQtyValue.AutoSize = true;
            lblQtyValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblQtyValue.ForeColor = Color.FromArgb(13, 148, 136);
            lblQtyValue.Location = new Point(130, 6);
            lblQtyValue.Name = "lblQtyValue";
            lblQtyValue.Text = "1";

            qtyRow.Controls.Add(lblQtyValue);
            qtyRow.Controls.Add(lblQtyCaption);

            pnlSearch.Controls.Add(qtyRow);
            pnlSearch.Controls.Add(searchRow);
            pnlSearch.Controls.Add(lblSearchTitle);

            // Numpad
            tblNumpad.ColumnCount = 3;
            tblNumpad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblNumpad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblNumpad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblNumpad.Dock = DockStyle.Fill;
            tblNumpad.Name = "tblNumpad";
            tblNumpad.Padding = new Padding(0, 8, 0, 8);
            tblNumpad.RowCount = 5;
            tblNumpad.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblNumpad.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblNumpad.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblNumpad.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblNumpad.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

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

            // Totals panel
            pnlTotals.BackColor = Color.FromArgb(15, 23, 42);
            pnlTotals.Dock = DockStyle.Bottom;
            pnlTotals.Height = 200;
            pnlTotals.Padding = new Padding(20);

            lblNetCaption.Dock = DockStyle.Top;
            lblNetCaption.Font = new Font("Segoe UI", 11F);
            lblNetCaption.ForeColor = Color.FromArgb(148, 163, 184);
            lblNetCaption.Height = 28;
            lblNetCaption.Name = "lblNetCaption";
            lblNetCaption.Text = "صافي المبلغ";
            lblNetCaption.TextAlign = ContentAlignment.MiddleCenter;

            lblNetAmount.Dock = DockStyle.Top;
            lblNetAmount.Font = new Font("Segoe UI Semibold", 32F, FontStyle.Bold);
            lblNetAmount.ForeColor = Color.White;
            lblNetAmount.Height = 56;
            lblNetAmount.Name = "lblNetAmount";
            lblNetAmount.Text = "0.00";
            lblNetAmount.TextAlign = ContentAlignment.MiddleCenter;

            lblCurrency.Dock = DockStyle.Top;
            lblCurrency.Font = new Font("Segoe UI", 10F);
            lblCurrency.ForeColor = Color.FromArgb(148, 163, 184);
            lblCurrency.Height = 22;
            lblCurrency.Name = "lblCurrency";
            lblCurrency.Text = "د.ع";
            lblCurrency.TextAlign = ContentAlignment.MiddleCenter;

            lblItemsCount.Dock = DockStyle.Top;
            lblItemsCount.Font = new Font("Segoe UI", 10F);
            lblItemsCount.ForeColor = Color.FromArgb(203, 213, 225);
            lblItemsCount.Height = 28;
            lblItemsCount.Name = "lblItemsCount";
            lblItemsCount.Text = "عدد الأصناف: 0";
            lblItemsCount.TextAlign = ContentAlignment.MiddleCenter;

            btnCheckout.BackColor = Color.FromArgb(13, 148, 136);
            btnCheckout.Cursor = Cursors.Hand;
            btnCheckout.Dock = DockStyle.Bottom;
            btnCheckout.Enabled = false;
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            btnCheckout.ForeColor = Color.White;
            btnCheckout.Height = 52;
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Text = "إتمام البيع";
            btnCheckout.UseVisualStyleBackColor = false;
            btnCheckout.Click += btnCheckout_Click;

            // Dock order for Top: add bottom-first
            pnlTotals.Controls.Add(btnCheckout);
            pnlTotals.Controls.Add(lblItemsCount);
            pnlTotals.Controls.Add(lblCurrency);
            pnlTotals.Controls.Add(lblNetAmount);
            pnlTotals.Controls.Add(lblNetCaption);

            pnlRight.Controls.Add(tblNumpad);
            pnlRight.Controls.Add(pnlTotals);
            pnlRight.Controls.Add(pnlSearch);

            splitMain.Panel1.Controls.Add(pnlLeft);
            splitMain.Panel2.Controls.Add(pnlRight);
            Controls.Add(splitMain);

            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            pnlCartActions.ResumeLayout(false);
            pnlMultiCart.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            tblNumpad.ResumeLayout(false);
            pnlTotals.ResumeLayout(false);
            pnlLeft.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ResumeLayout(false);
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
            tblNumpad.Controls.Add(button, column, row);
        }
    }
}
