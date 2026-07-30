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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            splitMain = new SplitContainer();
            pnlLeft = new Panel();
            dgvCart = new DataGridView();
            pnlCartActions = new Panel();
            btnRemoveItem = new Button();
            cartBtnSpacer = new Panel();
            btnClearCart = new Button();
            lblAlert = new Label();
            lblCartTitle = new Label();
            pnlMultiCart = new Panel();
            btnParkedInvoices = new Button();
            multiCartSpacer = new Panel();
            btnNewInvoice = new Button();
            pnlRight = new Panel();
            tblNumpad = new TableLayoutPanel();
            pnlTotals = new Panel();
            btnCheckout = new Button();
            lblItemsCount = new Label();
            lblCurrency = new Label();
            lblNetAmount = new Label();
            lblNetCaption = new Label();
            pnlSearch = new Panel();
            qtyRow = new Panel();
            lblQtyCaption = new Label();
            lblQtyValue = new Label();
            searchRow = new Panel();
            txtBarcode = new TextBox();
            btnSearch = new Button();
            lblSearchTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            pnlCartActions.SuspendLayout();
            pnlMultiCart.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlTotals.SuspendLayout();
            pnlSearch.SuspendLayout();
            qtyRow.SuspendLayout();
            searchRow.SuspendLayout();
            SuspendLayout();
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.FixedPanel = FixedPanel.Panel2;
            splitMain.Location = new Point(0, 0);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(pnlLeft);
            splitMain.Panel1.RightToLeft = RightToLeft.Yes;
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(pnlRight);
            splitMain.Panel2.RightToLeft = RightToLeft.Yes;
            splitMain.Size = new Size(1280, 800);
            splitMain.SplitterDistance = 732;
            splitMain.SplitterWidth = 8;
            splitMain.TabIndex = 0;
            // 
            // pnlLeft
            // 
            pnlLeft.BackColor = Color.FromArgb(248, 250, 252);
            pnlLeft.Controls.Add(dgvCart);
            pnlLeft.Controls.Add(pnlCartActions);
            pnlLeft.Controls.Add(lblAlert);
            pnlLeft.Controls.Add(lblCartTitle);
            pnlLeft.Controls.Add(pnlMultiCart);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Padding = new Padding(16);
            pnlLeft.RightToLeft = RightToLeft.Yes;
            pnlLeft.Size = new Size(732, 800);
            pnlLeft.TabIndex = 0;
            // 
            // dgvCart
            // 
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.AllowUserToResizeRows = false;
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCart.BackgroundColor = Color.White;
            dgvCart.BorderStyle = BorderStyle.None;
            dgvCart.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCart.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvCart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvCart.ColumnHeadersHeight = 42;
            dgvCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.5F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(204, 251, 241);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvCart.DefaultCellStyle = dataGridViewCellStyle2;
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.GridColor = Color.FromArgb(226, 232, 240);
            dgvCart.Location = new Point(16, 148);
            dgvCart.MultiSelect = false;
            dgvCart.Name = "dgvCart";
            dgvCart.ReadOnly = true;
            dgvCart.RightToLeft = RightToLeft.Yes;
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowHeadersWidth = 51;
            dgvCart.RowTemplate.Height = 40;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.Size = new Size(700, 580);
            dgvCart.TabIndex = 0;
            // 
            // pnlCartActions
            // 
            pnlCartActions.Controls.Add(btnRemoveItem);
            pnlCartActions.Controls.Add(cartBtnSpacer);
            pnlCartActions.Controls.Add(btnClearCart);
            pnlCartActions.Dock = DockStyle.Bottom;
            pnlCartActions.Location = new Point(16, 728);
            pnlCartActions.Name = "pnlCartActions";
            pnlCartActions.Padding = new Padding(0, 8, 0, 0);
            pnlCartActions.Size = new Size(700, 56);
            pnlCartActions.TabIndex = 1;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.BackColor = Color.FromArgb(239, 68, 68);
            btnRemoveItem.Cursor = Cursors.Hand;
            btnRemoveItem.Dock = DockStyle.Right;
            btnRemoveItem.FlatAppearance.BorderSize = 0;
            btnRemoveItem.FlatStyle = FlatStyle.Flat;
            btnRemoveItem.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnRemoveItem.ForeColor = Color.White;
            btnRemoveItem.Location = new Point(412, 8);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(140, 48);
            btnRemoveItem.TabIndex = 0;
            btnRemoveItem.Text = "حذف الصنف";
            btnRemoveItem.UseVisualStyleBackColor = false;
            btnRemoveItem.Click += btnRemoveItem_Click;
            // 
            // cartBtnSpacer
            // 
            cartBtnSpacer.Dock = DockStyle.Right;
            cartBtnSpacer.Location = new Point(552, 8);
            cartBtnSpacer.Name = "cartBtnSpacer";
            cartBtnSpacer.Size = new Size(8, 48);
            cartBtnSpacer.TabIndex = 1;
            // 
            // btnClearCart
            // 
            btnClearCart.BackColor = Color.FromArgb(100, 116, 139);
            btnClearCart.Cursor = Cursors.Hand;
            btnClearCart.Dock = DockStyle.Right;
            btnClearCart.FlatAppearance.BorderSize = 0;
            btnClearCart.FlatStyle = FlatStyle.Flat;
            btnClearCart.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnClearCart.ForeColor = Color.White;
            btnClearCart.Location = new Point(560, 8);
            btnClearCart.Name = "btnClearCart";
            btnClearCart.Size = new Size(140, 48);
            btnClearCart.TabIndex = 2;
            btnClearCart.Text = "تفريغ السلة";
            btnClearCart.UseVisualStyleBackColor = false;
            btnClearCart.Click += btnClearCart_Click;
            // 
            // lblAlert
            // 
            lblAlert.Dock = DockStyle.Top;
            lblAlert.Font = new Font("Segoe UI", 10F);
            lblAlert.Location = new Point(16, 108);
            lblAlert.Name = "lblAlert";
            lblAlert.Padding = new Padding(10, 8, 10, 8);
            lblAlert.Size = new Size(700, 40);
            lblAlert.TabIndex = 2;
            lblAlert.TextAlign = ContentAlignment.MiddleCenter;
            lblAlert.Visible = false;
            // 
            // lblCartTitle
            // 
            lblCartTitle.Dock = DockStyle.Top;
            lblCartTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblCartTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblCartTitle.Location = new Point(16, 68);
            lblCartTitle.Name = "lblCartTitle";
            lblCartTitle.Size = new Size(700, 40);
            lblCartTitle.TabIndex = 3;
            lblCartTitle.Text = "سلة المبيعات الحالية";
            lblCartTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlMultiCart
            // 
            pnlMultiCart.Controls.Add(btnParkedInvoices);
            pnlMultiCart.Controls.Add(multiCartSpacer);
            pnlMultiCart.Controls.Add(btnNewInvoice);
            pnlMultiCart.Dock = DockStyle.Top;
            pnlMultiCart.Location = new Point(16, 16);
            pnlMultiCart.Name = "pnlMultiCart";
            pnlMultiCart.Padding = new Padding(0, 0, 0, 8);
            pnlMultiCart.RightToLeft = RightToLeft.Yes;
            pnlMultiCart.Size = new Size(700, 52);
            pnlMultiCart.TabIndex = 4;
            // 
            // btnParkedInvoices
            // 
            btnParkedInvoices.BackColor = Color.FromArgb(51, 65, 85);
            btnParkedInvoices.Cursor = Cursors.Hand;
            btnParkedInvoices.Dock = DockStyle.Right;
            btnParkedInvoices.FlatAppearance.BorderSize = 0;
            btnParkedInvoices.FlatStyle = FlatStyle.Flat;
            btnParkedInvoices.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnParkedInvoices.ForeColor = Color.White;
            btnParkedInvoices.Location = new Point(352, 0);
            btnParkedInvoices.Name = "btnParkedInvoices";
            btnParkedInvoices.Size = new Size(180, 44);
            btnParkedInvoices.TabIndex = 0;
            btnParkedInvoices.Text = "الفواتير المعلقة (0)";
            btnParkedInvoices.UseVisualStyleBackColor = false;
            btnParkedInvoices.Click += btnParkedInvoices_Click;
            // 
            // multiCartSpacer
            // 
            multiCartSpacer.Dock = DockStyle.Right;
            multiCartSpacer.Location = new Point(532, 0);
            multiCartSpacer.Name = "multiCartSpacer";
            multiCartSpacer.Size = new Size(8, 44);
            multiCartSpacer.TabIndex = 1;
            // 
            // btnNewInvoice
            // 
            btnNewInvoice.BackColor = Color.FromArgb(13, 148, 136);
            btnNewInvoice.Cursor = Cursors.Hand;
            btnNewInvoice.Dock = DockStyle.Right;
            btnNewInvoice.FlatAppearance.BorderSize = 0;
            btnNewInvoice.FlatStyle = FlatStyle.Flat;
            btnNewInvoice.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnNewInvoice.ForeColor = Color.White;
            btnNewInvoice.Location = new Point(540, 0);
            btnNewInvoice.Name = "btnNewInvoice";
            btnNewInvoice.Size = new Size(160, 44);
            btnNewInvoice.TabIndex = 2;
            btnNewInvoice.Text = "+ فاتورة جديدة";
            btnNewInvoice.UseVisualStyleBackColor = false;
            btnNewInvoice.Click += btnNewInvoice_Click;
            // 
            // pnlRight
            // 
            pnlRight.BackColor = Color.White;
            pnlRight.Controls.Add(tblNumpad);
            pnlRight.Controls.Add(pnlTotals);
            pnlRight.Controls.Add(pnlSearch);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(0, 0);
            pnlRight.MinimumSize = new Size(320, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(16);
            pnlRight.RightToLeft = RightToLeft.Yes;
            pnlRight.Size = new Size(540, 800);
            pnlRight.TabIndex = 0;
            // 
            // tblNumpad
            // 
            tblNumpad.ColumnCount = 3;
            tblNumpad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblNumpad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblNumpad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblNumpad.Dock = DockStyle.Fill;
            tblNumpad.Location = new Point(16, 144);
            tblNumpad.Name = "tblNumpad";
            tblNumpad.Padding = new Padding(0, 8, 0, 8);
            tblNumpad.RowCount = 5;
            tblNumpad.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblNumpad.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblNumpad.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblNumpad.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblNumpad.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblNumpad.Size = new Size(508, 430);
            tblNumpad.TabIndex = 0;
            // 
            // pnlTotals
            // 
            pnlTotals.BackColor = Color.FromArgb(15, 23, 42);
            pnlTotals.Controls.Add(btnCheckout);
            pnlTotals.Controls.Add(lblItemsCount);
            pnlTotals.Controls.Add(lblCurrency);
            pnlTotals.Controls.Add(lblNetAmount);
            pnlTotals.Controls.Add(lblNetCaption);
            pnlTotals.Dock = DockStyle.Bottom;
            pnlTotals.Location = new Point(16, 574);
            pnlTotals.Name = "pnlTotals";
            pnlTotals.Padding = new Padding(16, 14, 16, 14);
            pnlTotals.RightToLeft = RightToLeft.Yes;
            pnlTotals.Size = new Size(508, 210);
            pnlTotals.TabIndex = 1;
            // 
            // btnCheckout
            // 
            btnCheckout.BackColor = Color.FromArgb(13, 148, 136);
            btnCheckout.Cursor = Cursors.Hand;
            btnCheckout.Dock = DockStyle.Bottom;
            btnCheckout.Enabled = false;
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            btnCheckout.ForeColor = Color.White;
            btnCheckout.Location = new Point(16, 144);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(476, 52);
            btnCheckout.TabIndex = 0;
            btnCheckout.Text = "إتمام البيع";
            btnCheckout.UseVisualStyleBackColor = false;
            btnCheckout.Click += btnCheckout_Click;
            // 
            // lblItemsCount
            // 
            lblItemsCount.Dock = DockStyle.Top;
            lblItemsCount.Font = new Font("Segoe UI", 10F);
            lblItemsCount.ForeColor = Color.FromArgb(203, 213, 225);
            lblItemsCount.Location = new Point(16, 114);
            lblItemsCount.Name = "lblItemsCount";
            lblItemsCount.Size = new Size(476, 28);
            lblItemsCount.TabIndex = 1;
            lblItemsCount.Text = "عدد الأصناف: 0";
            lblItemsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCurrency
            // 
            lblCurrency.Dock = DockStyle.Top;
            lblCurrency.Font = new Font("Segoe UI", 10F);
            lblCurrency.ForeColor = Color.FromArgb(148, 163, 184);
            lblCurrency.Location = new Point(16, 92);
            lblCurrency.Name = "lblCurrency";
            lblCurrency.Size = new Size(476, 22);
            lblCurrency.TabIndex = 2;
            lblCurrency.Text = "د.ع";
            lblCurrency.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNetAmount
            // 
            lblNetAmount.Dock = DockStyle.Top;
            lblNetAmount.Font = new Font("Segoe UI Semibold", 30F, FontStyle.Bold);
            lblNetAmount.ForeColor = Color.White;
            lblNetAmount.Location = new Point(16, 40);
            lblNetAmount.Name = "lblNetAmount";
            lblNetAmount.Size = new Size(476, 52);
            lblNetAmount.TabIndex = 3;
            lblNetAmount.Text = "0.00";
            lblNetAmount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNetCaption
            // 
            lblNetCaption.Dock = DockStyle.Top;
            lblNetCaption.Font = new Font("Segoe UI", 11F);
            lblNetCaption.ForeColor = Color.FromArgb(148, 163, 184);
            lblNetCaption.Location = new Point(16, 14);
            lblNetCaption.Name = "lblNetCaption";
            lblNetCaption.Size = new Size(476, 26);
            lblNetCaption.TabIndex = 4;
            lblNetCaption.Text = "صافي المبلغ";
            lblNetCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlSearch
            // 
            pnlSearch.Controls.Add(qtyRow);
            pnlSearch.Controls.Add(searchRow);
            pnlSearch.Controls.Add(lblSearchTitle);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(16, 16);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.RightToLeft = RightToLeft.Yes;
            pnlSearch.Size = new Size(508, 128);
            pnlSearch.TabIndex = 2;
            // 
            // qtyRow
            // 
            qtyRow.Controls.Add(lblQtyCaption);
            qtyRow.Controls.Add(lblQtyValue);
            qtyRow.Dock = DockStyle.Top;
            qtyRow.Location = new Point(0, 74);
            qtyRow.Name = "qtyRow";
            qtyRow.Padding = new Padding(0, 6, 0, 0);
            qtyRow.Size = new Size(508, 46);
            qtyRow.TabIndex = 0;
            // 
            // lblQtyCaption
            // 
            lblQtyCaption.Dock = DockStyle.Fill;
            lblQtyCaption.Font = new Font("Segoe UI", 10F);
            lblQtyCaption.ForeColor = Color.FromArgb(71, 85, 105);
            lblQtyCaption.Location = new Point(64, 6);
            lblQtyCaption.Name = "lblQtyCaption";
            lblQtyCaption.Size = new Size(444, 40);
            lblQtyCaption.TabIndex = 0;
            lblQtyCaption.Text = "الكمية القادمة:";
            lblQtyCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblQtyValue
            // 
            lblQtyValue.Dock = DockStyle.Left;
            lblQtyValue.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblQtyValue.ForeColor = Color.FromArgb(13, 148, 136);
            lblQtyValue.Location = new Point(0, 6);
            lblQtyValue.Name = "lblQtyValue";
            lblQtyValue.Size = new Size(64, 40);
            lblQtyValue.TabIndex = 1;
            lblQtyValue.Text = "1";
            lblQtyValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // searchRow
            // 
            searchRow.Controls.Add(txtBarcode);
            searchRow.Controls.Add(btnSearch);
            searchRow.Dock = DockStyle.Top;
            searchRow.Location = new Point(0, 30);
            searchRow.Name = "searchRow";
            searchRow.Size = new Size(508, 44);
            searchRow.TabIndex = 1;
            // 
            // txtBarcode
            // 
            txtBarcode.BorderStyle = BorderStyle.FixedSingle;
            txtBarcode.Dock = DockStyle.Fill;
            txtBarcode.Font = new Font("Segoe UI", 14F);
            txtBarcode.Location = new Point(90, 0);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.PlaceholderText = "امسح الباركود أو اكتب اسم المنتج ثم Enter";
            txtBarcode.RightToLeft = RightToLeft.Yes;
            txtBarcode.Size = new Size(418, 39);
            txtBarcode.TabIndex = 0;
            txtBarcode.TextAlign = HorizontalAlignment.Right;
            txtBarcode.KeyDown += txtBarcode_KeyDown;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(13, 148, 136);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.Dock = DockStyle.Left;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(0, 0);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(90, 44);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "بحث";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // lblSearchTitle
            // 
            lblSearchTitle.Dock = DockStyle.Top;
            lblSearchTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblSearchTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblSearchTitle.Location = new Point(0, 0);
            lblSearchTitle.Name = "lblSearchTitle";
            lblSearchTitle.Size = new Size(508, 30);
            lblSearchTitle.TabIndex = 2;
            lblSearchTitle.Text = "بحث سريع (باركود / اسم)";
            lblSearchTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // PosSalesForm
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1280, 800);
            Controls.Add(splitMain);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1100, 700);
            Name = "PosSalesForm";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "UrPOS — شاشة الكاشير";
            WindowState = FormWindowState.Maximized;
            Load += PosSalesForm_Load;
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            pnlCartActions.ResumeLayout(false);
            pnlMultiCart.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            pnlTotals.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            qtyRow.ResumeLayout(false);
            searchRow.ResumeLayout(false);
            searchRow.PerformLayout();
            ResumeLayout(false);
        }

        private Panel cartBtnSpacer;
        private Panel multiCartSpacer;
        private Panel qtyRow;
        private Panel searchRow;
    }
}
