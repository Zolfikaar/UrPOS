using System.Drawing;
using System.Windows.Forms;

namespace UrPOS.WinForms.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblBrand;
        private Label lblWelcome;
        private Label lblRole;
        private Button btnLogout;
        private Panel pnlSidebar;
        private Label lblSidebarTitle;
        private Button btnNavPos;
        private Button btnNavProducts;
        private Button btnNavInvoices;
        private Button btnNavSettings;
        private Panel pnlContent;
        private Label lblTitle;
        private Label lblHint;
        private Panel pnlWelcomeCard;
        private Label lblWelcomeCardTitle;
        private Label lblWelcomeCardBody;

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

            pnlHeader = new Panel();
            lblBrand = new Label();
            lblWelcome = new Label();
            lblRole = new Label();
            btnLogout = new Button();
            pnlSidebar = new Panel();
            lblSidebarTitle = new Label();
            btnNavPos = new Button();
            btnNavProducts = new Button();
            btnNavInvoices = new Button();
            btnNavSettings = new Button();
            pnlContent = new Panel();
            pnlWelcomeCard = new Panel();
            lblTitle = new Label();
            lblHint = new Label();
            lblWelcomeCardTitle = new Label();
            lblWelcomeCardBody = new Label();

            SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlSidebar.SuspendLayout();
            pnlContent.SuspendLayout();
            pnlWelcomeCard.SuspendLayout();

            // MainForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1100, 700);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            MinimumSize = new Size(900, 600);
            Name = "MainForm";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UrPOS — الشاشة الرئيسية";
            WindowState = FormWindowState.Maximized;

            // ===================== Header =====================
            pnlHeader.BackColor = Color.FromArgb(15, 23, 42);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 72;
            pnlHeader.Padding = new Padding(24, 12, 24, 12);
            pnlHeader.Name = "pnlHeader";

            lblBrand.AutoSize = true;
            lblBrand.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(24, 18);
            lblBrand.Name = "lblBrand";
            lblBrand.Text = "UrPOS";

            lblWelcome.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblWelcome.ForeColor = Color.FromArgb(226, 232, 240);
            lblWelcome.Location = new Point(820, 12);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.RightToLeft = RightToLeft.Yes;
            lblWelcome.Text = "مرحباً";
            lblWelcome.TextAlign = ContentAlignment.MiddleRight;

            lblRole.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblRole.ForeColor = Color.FromArgb(148, 163, 184);
            lblRole.Location = new Point(820, 38);
            lblRole.Name = "lblRole";
            lblRole.RightToLeft = RightToLeft.Yes;
            lblRole.Text = string.Empty;
            lblRole.TextAlign = ContentAlignment.MiddleRight;

            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnLogout.BackColor = Color.FromArgb(51, 65, 85);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(980, 18);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(100, 36);
            btnLogout.Text = "خروج";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;

            pnlHeader.Controls.Add(btnLogout);
            pnlHeader.Controls.Add(lblRole);
            pnlHeader.Controls.Add(lblWelcome);
            pnlHeader.Controls.Add(lblBrand);

            // ===================== Sidebar (visual right in RTL) =====================
            // With RightToLeftLayout=true, DockStyle.Left docks to the visual right edge.
            pnlSidebar.BackColor = Color.FromArgb(30, 41, 59);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Padding = new Padding(16, 20, 16, 20);
            pnlSidebar.Width = 260;
            pnlSidebar.Name = "pnlSidebar";

            lblSidebarTitle.Dock = DockStyle.Top;
            lblSidebarTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblSidebarTitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblSidebarTitle.Height = 36;
            lblSidebarTitle.Name = "lblSidebarTitle";
            lblSidebarTitle.Text = "القائمة الرئيسية";
            lblSidebarTitle.TextAlign = ContentAlignment.MiddleRight;

            var spNavTop = new Panel { Dock = DockStyle.Top, Height = 8, Name = "spNavTop" };

            btnNavPos = CreateSidebarButton("btnNavPos", "شاشة الكاشير (POS)", Color.FromArgb(13, 148, 136));
            btnNavPos.Click += btnOpenPos_Click;

            var spNav1 = new Panel { Dock = DockStyle.Top, Height = 10, Name = "spNav1" };

            btnNavProducts = CreateSidebarButton("btnNavProducts", "إدارة المنتجات", Color.FromArgb(51, 65, 85));
            btnNavProducts.Click += btnNavProducts_Click;

            var spNav2 = new Panel { Dock = DockStyle.Top, Height = 10, Name = "spNav2" };

            btnNavInvoices = CreateSidebarButton("btnNavInvoices", "سجل الفواتير", Color.FromArgb(51, 65, 85));
            btnNavInvoices.Click += btnNavInvoices_Click;

            var spNav3 = new Panel { Dock = DockStyle.Top, Height = 10, Name = "spNav3" };

            btnNavSettings = CreateSidebarButton("btnNavSettings", "الإعدادات والأمان", Color.FromArgb(51, 65, 85));
            btnNavSettings.Click += btnNavSettings_Click;

            // Dock Top order: add bottom-first
            pnlSidebar.Controls.Add(btnNavSettings);
            pnlSidebar.Controls.Add(spNav3);
            pnlSidebar.Controls.Add(btnNavInvoices);
            pnlSidebar.Controls.Add(spNav2);
            pnlSidebar.Controls.Add(btnNavProducts);
            pnlSidebar.Controls.Add(spNav1);
            pnlSidebar.Controls.Add(btnNavPos);
            pnlSidebar.Controls.Add(spNavTop);
            pnlSidebar.Controls.Add(lblSidebarTitle);

            // ===================== Content =====================
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Padding = new Padding(40, 40, 40, 40);
            pnlContent.Name = "pnlContent";

            lblTitle.AutoSize = true;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Height = 48;
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "لوحة التحكم";
            lblTitle.TextAlign = ContentAlignment.MiddleRight;

            lblHint.AutoSize = false;
            lblHint.Dock = DockStyle.Top;
            lblHint.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblHint.ForeColor = Color.FromArgb(71, 85, 105);
            lblHint.Height = 36;
            lblHint.Name = "lblHint";
            lblHint.Text = "اختر قسماً من الشريط الجانبي للبدء. شاشة الكاشير جاهزة للاستخدام.";
            lblHint.TextAlign = ContentAlignment.MiddleRight;

            var spContent = new Panel { Dock = DockStyle.Top, Height = 24, Name = "spContent" };

            pnlWelcomeCard.BackColor = Color.White;
            pnlWelcomeCard.Dock = DockStyle.Top;
            pnlWelcomeCard.Height = 180;
            pnlWelcomeCard.Padding = new Padding(28, 24, 28, 24);
            pnlWelcomeCard.Name = "pnlWelcomeCard";

            lblWelcomeCardTitle.Dock = DockStyle.Top;
            lblWelcomeCardTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblWelcomeCardTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblWelcomeCardTitle.Height = 36;
            lblWelcomeCardTitle.Name = "lblWelcomeCardTitle";
            lblWelcomeCardTitle.Text = "مرحباً بك في UrPOS";
            lblWelcomeCardTitle.TextAlign = ContentAlignment.MiddleRight;

            lblWelcomeCardBody.Dock = DockStyle.Fill;
            lblWelcomeCardBody.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblWelcomeCardBody.ForeColor = Color.FromArgb(71, 85, 105);
            lblWelcomeCardBody.Name = "lblWelcomeCardBody";
            lblWelcomeCardBody.Text =
                "• شاشة الكاشير: بدء البيع والمسح الضوئي للمنتجات\r\n" +
                "• إدارة المنتجات / سجل الفواتير / الإعدادات: واجهات قيد التجهيز (placeholders)";
            lblWelcomeCardBody.TextAlign = ContentAlignment.TopRight;

            pnlWelcomeCard.Controls.Add(lblWelcomeCardBody);
            pnlWelcomeCard.Controls.Add(lblWelcomeCardTitle);

            pnlContent.Controls.Add(pnlWelcomeCard);
            pnlContent.Controls.Add(spContent);
            pnlContent.Controls.Add(lblHint);
            pnlContent.Controls.Add(lblTitle);

            // Add order: Fill first, then Left (sidebar), then Top (header)
            Controls.Add(pnlContent);
            Controls.Add(pnlSidebar);
            Controls.Add(pnlHeader);

            Load += (_, _) => LayoutHeaderActions();
            Resize += (_, _) => LayoutHeaderActions();

            pnlWelcomeCard.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlSidebar.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            ResumeLayout(false);
        }

        private static Button CreateSidebarButton(string name, string text, Color backColor)
        {
            var button = new Button
            {
                BackColor = backColor,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                Height = 52,
                Name = name,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void LayoutHeaderActions()
        {
            btnLogout.Left = 24;
            btnLogout.Top = 18;
            lblWelcome.Left = Math.Max(160, ClientSize.Width - 300);
            lblRole.Left = Math.Max(160, ClientSize.Width - 300);
        }
    }
}
