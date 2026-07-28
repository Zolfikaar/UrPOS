using System.Drawing;
using System.Windows.Forms;

namespace UrPOS.WinForms.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblBrand;
        private Panel pnlUserInfo;
        private Label lblWelcome;
        private Label lblRole;
        private Button btnLogout;
        private Panel pnlSidebar;
        private Label lblSidebarTitle;
        private Button btnNavPos;
        private Button btnNavProducts;
        private Button btnNavInvoices;
        private Button btnNavSettings;
        private Button btnNavDashboard;
        private Panel pnlContentHost;

        // Dashboard widgets (kept as fields for navigation)
        private Panel pnlDashboard;
        private Label lblTitle;
        private Label lblHint;
        private Panel pnlWelcomeCard;
        private Label lblWelcomeCardTitle;
        private Label lblWelcomeCardBody;
        private FlowLayoutPanel pnlQuickActions;
        private Button btnQuickPos;
        private Button btnQuickAddProduct;
        private Button btnQuickBackup;

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
            pnlUserInfo = new Panel();
            lblWelcome = new Label();
            lblRole = new Label();
            btnLogout = new Button();
            pnlSidebar = new Panel();
            lblSidebarTitle = new Label();
            btnNavDashboard = new Button();
            btnNavPos = new Button();
            btnNavProducts = new Button();
            btnNavInvoices = new Button();
            btnNavSettings = new Button();
            pnlContentHost = new Panel();
            pnlDashboard = new Panel();
            lblTitle = new Label();
            lblHint = new Label();
            pnlWelcomeCard = new Panel();
            lblWelcomeCardTitle = new Label();
            lblWelcomeCardBody = new Label();
            pnlQuickActions = new FlowLayoutPanel();
            btnQuickPos = new Button();
            btnQuickAddProduct = new Button();
            btnQuickBackup = new Button();

            SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlUserInfo.SuspendLayout();
            pnlSidebar.SuspendLayout();
            pnlContentHost.SuspendLayout();
            pnlDashboard.SuspendLayout();
            pnlWelcomeCard.SuspendLayout();
            pnlQuickActions.SuspendLayout();

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
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(20, 0, 20, 0);

            // Inside a Top-docked header panel, Dock is NOT re-mirrored:
            // Left = screen left, Right = screen right.
            btnLogout.Dock = DockStyle.Left;
            btnLogout.BackColor = Color.FromArgb(51, 65, 85);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnLogout.ForeColor = Color.White;
            btnLogout.Margin = new Padding(8);
            btnLogout.Name = "btnLogout";
            btnLogout.Text = "خروج";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Width = 100;
            btnLogout.Click += btnLogout_Click;

            lblBrand.Dock = DockStyle.Right;
            lblBrand.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lblBrand.ForeColor = Color.White;
            lblBrand.Name = "lblBrand";
            lblBrand.Padding = new Padding(8, 0, 8, 0);
            lblBrand.Text = "UrPOS";
            lblBrand.TextAlign = ContentAlignment.MiddleCenter;
            lblBrand.Width = 120;

            pnlUserInfo.Dock = DockStyle.Left;
            pnlUserInfo.Name = "pnlUserInfo";
            pnlUserInfo.Padding = new Padding(12, 10, 8, 10);
            pnlUserInfo.Width = 280;

            lblWelcome.Dock = DockStyle.Top;
            lblWelcome.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblWelcome.ForeColor = Color.FromArgb(226, 232, 240);
            lblWelcome.Height = 26;
            lblWelcome.Name = "lblWelcome";
            lblWelcome.RightToLeft = RightToLeft.Yes;
            lblWelcome.Text = "مرحباً";
            lblWelcome.TextAlign = ContentAlignment.MiddleLeft;

            lblRole.Dock = DockStyle.Top;
            lblRole.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblRole.ForeColor = Color.FromArgb(148, 163, 184);
            lblRole.Height = 22;
            lblRole.Name = "lblRole";
            lblRole.RightToLeft = RightToLeft.Yes;
            lblRole.Text = string.Empty;
            lblRole.TextAlign = ContentAlignment.MiddleLeft;

            pnlUserInfo.Controls.Add(lblRole);
            pnlUserInfo.Controls.Add(lblWelcome);

            // Desired: [خروج] [user info] ........ [UrPOS]
            // Last Dock.Left added is laid out first (outermost left)
            pnlHeader.Controls.Add(pnlUserInfo);
            pnlHeader.Controls.Add(btnLogout);
            pnlHeader.Controls.Add(lblBrand);

            // ===================== Sidebar (visual right) =====================
            pnlSidebar.BackColor = Color.FromArgb(30, 41, 59);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Padding = new Padding(14, 18, 14, 18);
            pnlSidebar.Width = 280;

            lblSidebarTitle.Dock = DockStyle.Top;
            lblSidebarTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblSidebarTitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblSidebarTitle.Height = 36;
            lblSidebarTitle.Name = "lblSidebarTitle";
            lblSidebarTitle.Padding = new Padding(8, 0, 8, 0);
            lblSidebarTitle.Text = "القائمة الرئيسية";
            lblSidebarTitle.TextAlign = ContentAlignment.MiddleLeft;

            var spNavTop = new Panel { Dock = DockStyle.Top, Height = 8, Name = "spNavTop" };

            btnNavDashboard = CreateSidebarButton("btnNavDashboard", "لوحة التحكم", Color.FromArgb(51, 65, 85));
            btnNavDashboard.Click += btnNavDashboard_Click;

            var sp0 = new Panel { Dock = DockStyle.Top, Height = 10, Name = "spNav0" };

            btnNavPos = CreateSidebarButton("btnNavPos", "شاشة الكاشير", Color.FromArgb(51, 65, 85));
            btnNavPos.Click += btnOpenPos_Click;

            var sp1 = new Panel { Dock = DockStyle.Top, Height = 10, Name = "spNav1" };

            btnNavProducts = CreateSidebarButton("btnNavProducts", "إدارة المنتجات", Color.FromArgb(51, 65, 85));
            btnNavProducts.Click += btnNavProducts_Click;

            var sp2 = new Panel { Dock = DockStyle.Top, Height = 10, Name = "spNav2" };

            btnNavInvoices = CreateSidebarButton("btnNavInvoices", "سجل الفواتير", Color.FromArgb(51, 65, 85));
            btnNavInvoices.Click += btnNavInvoices_Click;

            var sp3 = new Panel { Dock = DockStyle.Top, Height = 10, Name = "spNav3" };

            btnNavSettings = CreateSidebarButton("btnNavSettings", "الإعدادات والأمان", Color.FromArgb(51, 65, 85));
            btnNavSettings.Click += btnNavSettings_Click;

            pnlSidebar.Controls.Add(btnNavSettings);
            pnlSidebar.Controls.Add(sp3);
            pnlSidebar.Controls.Add(btnNavInvoices);
            pnlSidebar.Controls.Add(sp2);
            pnlSidebar.Controls.Add(btnNavProducts);
            pnlSidebar.Controls.Add(sp1);
            pnlSidebar.Controls.Add(btnNavPos);
            pnlSidebar.Controls.Add(sp0);
            pnlSidebar.Controls.Add(btnNavDashboard);
            pnlSidebar.Controls.Add(spNavTop);
            pnlSidebar.Controls.Add(lblSidebarTitle);

            // ===================== Content host =====================
            pnlContentHost.BackColor = Color.FromArgb(241, 245, 249);
            pnlContentHost.Dock = DockStyle.Fill;
            pnlContentHost.Name = "pnlContentHost";
            pnlContentHost.Padding = new Padding(32, 28, 32, 28);

            BuildDashboard();

            Controls.Add(pnlContentHost);
            Controls.Add(pnlSidebar);
            Controls.Add(pnlHeader);

            pnlQuickActions.ResumeLayout(false);
            pnlWelcomeCard.ResumeLayout(false);
            pnlDashboard.ResumeLayout(false);
            pnlContentHost.ResumeLayout(false);
            pnlUserInfo.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlSidebar.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void BuildDashboard()
        {
            pnlDashboard.BackColor = Color.FromArgb(241, 245, 249);
            pnlDashboard.Dock = DockStyle.Fill;
            pnlDashboard.Name = "pnlDashboard";
            pnlDashboard.RightToLeft = RightToLeft.Yes;

            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Height = 44;
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "لوحة التحكم";
            // Under form RTL, MiddleLeft renders on the visual right
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblHint.Dock = DockStyle.Top;
            lblHint.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblHint.ForeColor = Color.FromArgb(71, 85, 105);
            lblHint.Height = 32;
            lblHint.Name = "lblHint";
            lblHint.RightToLeft = RightToLeft.Yes;
            lblHint.Text = "اختر قسماً من الشريط الجانبي أو استخدم الاختصارات السريعة أدناه.";
            lblHint.TextAlign = ContentAlignment.MiddleLeft;

            var sp = new Panel { Dock = DockStyle.Top, Height = 16, Name = "spDash" };

            pnlWelcomeCard.BackColor = Color.White;
            pnlWelcomeCard.Dock = DockStyle.Top;
            pnlWelcomeCard.Height = 140;
            pnlWelcomeCard.Name = "pnlWelcomeCard";
            pnlWelcomeCard.Padding = new Padding(28, 20, 28, 20);
            pnlWelcomeCard.RightToLeft = RightToLeft.Yes;

            lblWelcomeCardTitle.Dock = DockStyle.Top;
            lblWelcomeCardTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblWelcomeCardTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblWelcomeCardTitle.Height = 32;
            lblWelcomeCardTitle.Name = "lblWelcomeCardTitle";
            lblWelcomeCardTitle.RightToLeft = RightToLeft.Yes;
            lblWelcomeCardTitle.Text = "مرحباً بك في UrPOS";
            lblWelcomeCardTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblWelcomeCardBody.Dock = DockStyle.Fill;
            lblWelcomeCardBody.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblWelcomeCardBody.ForeColor = Color.FromArgb(71, 85, 105);
            lblWelcomeCardBody.Name = "lblWelcomeCardBody";
            lblWelcomeCardBody.RightToLeft = RightToLeft.Yes;
            lblWelcomeCardBody.Text =
                "نظام نقاط بيع يعمل دون اتصال مع دعم كامل للواجهة العربية (RTL).\r\n" +
                "استخدم الاختصارات السريعة للوصول المباشر إلى البيع أو الإعدادات.";
            lblWelcomeCardBody.TextAlign = ContentAlignment.TopLeft;

            pnlWelcomeCard.Controls.Add(lblWelcomeCardBody);
            pnlWelcomeCard.Controls.Add(lblWelcomeCardTitle);

            var sp2 = new Panel { Dock = DockStyle.Top, Height = 20, Name = "spQuick" };

            var lblQuick = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Height = 32,
                Name = "lblQuickActions",
                RightToLeft = RightToLeft.Yes,
                Text = "اختصارات سريعة",
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlQuickActions.AutoSize = true;
            pnlQuickActions.Dock = DockStyle.Top;
            // With RightToLeft=Yes, LeftToRight flow places the first control at the visual right
            pnlQuickActions.FlowDirection = FlowDirection.LeftToRight;
            pnlQuickActions.Name = "pnlQuickActions";
            pnlQuickActions.Padding = new Padding(0, 8, 0, 8);
            pnlQuickActions.RightToLeft = RightToLeft.Yes;
            pnlQuickActions.WrapContents = true;

            btnQuickPos = CreateQuickButton("btnQuickPos", "شاشة البيع السريعة", Color.FromArgb(13, 148, 136));
            btnQuickPos.Click += btnOpenPos_Click;

            btnQuickAddProduct = CreateQuickButton("btnQuickAddProduct", "إضافة منتج جديد", Color.FromArgb(37, 99, 235));
            btnQuickAddProduct.Click += btnNavProducts_Click;

            btnQuickBackup = CreateQuickButton("btnQuickBackup", "النسخ الاحتياطي", Color.FromArgb(51, 65, 85));
            btnQuickBackup.Click += btnQuickBackup_Click;

            pnlQuickActions.Controls.Add(btnQuickPos);
            pnlQuickActions.Controls.Add(btnQuickAddProduct);
            pnlQuickActions.Controls.Add(btnQuickBackup);

            // Top dock: add bottom-first
            pnlDashboard.Controls.Add(pnlQuickActions);
            pnlDashboard.Controls.Add(lblQuick);
            pnlDashboard.Controls.Add(sp2);
            pnlDashboard.Controls.Add(pnlWelcomeCard);
            pnlDashboard.Controls.Add(sp);
            pnlDashboard.Controls.Add(lblHint);
            pnlDashboard.Controls.Add(lblTitle);

            pnlContentHost.Controls.Add(pnlDashboard);
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
                Padding = new Padding(16, 0, 16, 0),
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private static Button CreateQuickButton(string name, string text, Color backColor)
        {
            var button = new Button
            {
                BackColor = backColor,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                Margin = new Padding(0, 0, 12, 12),
                MinimumSize = new Size(200, 56),
                Name = name,
                Padding = new Padding(20, 0, 20, 0),
                Size = new Size(220, 56),
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }
    }
}
