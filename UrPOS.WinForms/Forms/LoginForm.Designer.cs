using System.Drawing;
using System.Windows.Forms;

namespace UrPOS.WinForms.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlCard;
        private Panel pnlAccent;
        private Panel pnlBody;
        private Label lblBrand;
        private Label lblSubtitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnCreateGuest;
        private Label lblError;
        private Label lblFooter;

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

            pnlCard = new Panel();
            pnlAccent = new Panel();
            pnlBody = new Panel();
            lblBrand = new Label();
            lblSubtitle = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnCreateGuest = new Button();
            lblError = new Label();
            lblFooter = new Label();

            SuspendLayout();
            pnlCard.SuspendLayout();
            pnlBody.SuspendLayout();

            // Compact dialog — card fills the form (no TableLayoutPanel shrink bug)
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(15, 23, 42);
            ClientSize = new Size(440, 560);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UrPOS — تسجيل الدخول";

            pnlCard.BackColor = Color.FromArgb(248, 250, 252);
            pnlCard.Dock = DockStyle.Fill;
            pnlCard.Name = "pnlCard";
            pnlCard.RightToLeft = RightToLeft.Yes;

            pnlAccent.BackColor = Color.FromArgb(13, 148, 136);
            pnlAccent.Dock = DockStyle.Top;
            pnlAccent.Height = 6;
            pnlAccent.Name = "pnlAccent";

            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(36, 24, 36, 16);
            pnlBody.RightToLeft = RightToLeft.Yes;

            lblBrand.Dock = DockStyle.Top;
            lblBrand.Font = new Font("Segoe UI Semibold", 28F, FontStyle.Bold, GraphicsUnit.Point);
            lblBrand.ForeColor = Color.FromArgb(15, 23, 42);
            lblBrand.Height = 48;
            lblBrand.Name = "lblBrand";
            lblBrand.Text = "UrPOS";
            lblBrand.TextAlign = ContentAlignment.MiddleCenter;

            lblSubtitle.Dock = DockStyle.Top;
            lblSubtitle.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Height = 36;
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Text = "نظام نقاط البيع — تسجيل الدخول";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            lblUsername.Dock = DockStyle.Top;
            lblUsername.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblUsername.ForeColor = Color.FromArgb(51, 65, 85);
            lblUsername.Height = 24;
            lblUsername.Name = "lblUsername";
            lblUsername.RightToLeft = RightToLeft.Yes;
            lblUsername.Text = "اسم المستخدم";
            lblUsername.TextAlign = ContentAlignment.MiddleLeft;

            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Dock = DockStyle.Top;
            txtUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "أدخل اسم المستخدم";
            txtUsername.RightToLeft = RightToLeft.Yes;
            txtUsername.TabIndex = 0;
            txtUsername.TextAlign = HorizontalAlignment.Right;
            txtUsername.KeyDown += txtUsername_KeyDown;

            var spUsername = new Panel { Dock = DockStyle.Top, Height = 12, Name = "spUsername" };

            lblPassword.Dock = DockStyle.Top;
            lblPassword.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblPassword.Height = 24;
            lblPassword.Name = "lblPassword";
            lblPassword.RightToLeft = RightToLeft.Yes;
            lblPassword.Text = "كلمة المرور";
            lblPassword.TextAlign = ContentAlignment.MiddleLeft;

            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Dock = DockStyle.Top;
            txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.PlaceholderText = "أدخل كلمة المرور";
            txtPassword.RightToLeft = RightToLeft.Yes;
            txtPassword.TabIndex = 1;
            txtPassword.TextAlign = HorizontalAlignment.Right;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.KeyDown += txtPassword_KeyDown;

            var spPassword = new Panel { Dock = DockStyle.Top, Height = 12, Name = "spPassword" };

            lblError.Dock = DockStyle.Top;
            lblError.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblError.ForeColor = Color.FromArgb(185, 28, 28);
            lblError.Height = 36;
            lblError.Name = "lblError";
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            lblError.Visible = false;

            var spError = new Panel { Dock = DockStyle.Top, Height = 10, Name = "spError" };

            btnLogin.BackColor = Color.FromArgb(13, 148, 136);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Dock = DockStyle.Top;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnLogin.ForeColor = Color.White;
            btnLogin.Height = 46;
            btnLogin.Name = "btnLogin";
            btnLogin.TabIndex = 2;
            btnLogin.Text = "تسجيل الدخول";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;

            var spGuest = new Panel { Dock = DockStyle.Top, Height = 10, Name = "spGuest" };

            btnCreateGuest.BackColor = Color.FromArgb(248, 250, 252);
            btnCreateGuest.Cursor = Cursors.Hand;
            btnCreateGuest.Dock = DockStyle.Top;
            btnCreateGuest.FlatAppearance.BorderColor = Color.FromArgb(13, 148, 136);
            btnCreateGuest.FlatAppearance.BorderSize = 1;
            btnCreateGuest.FlatStyle = FlatStyle.Flat;
            btnCreateGuest.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnCreateGuest.ForeColor = Color.FromArgb(13, 148, 136);
            btnCreateGuest.Height = 42;
            btnCreateGuest.Name = "btnCreateGuest";
            btnCreateGuest.TabIndex = 3;
            btnCreateGuest.Text = "دخول كضيف";
            btnCreateGuest.UseVisualStyleBackColor = false;
            btnCreateGuest.Click += btnCreateGuest_Click;

            lblFooter.Dock = DockStyle.Bottom;
            lblFooter.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblFooter.ForeColor = Color.FromArgb(148, 163, 184);
            lblFooter.Height = 26;
            lblFooter.Name = "lblFooter";
            lblFooter.Text = "واجهة عربية (RTL) — UrPOS";
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;

            pnlBody.Controls.Add(btnCreateGuest);
            pnlBody.Controls.Add(spGuest);
            pnlBody.Controls.Add(btnLogin);
            pnlBody.Controls.Add(spError);
            pnlBody.Controls.Add(lblError);
            pnlBody.Controls.Add(spPassword);
            pnlBody.Controls.Add(txtPassword);
            pnlBody.Controls.Add(lblPassword);
            pnlBody.Controls.Add(spUsername);
            pnlBody.Controls.Add(txtUsername);
            pnlBody.Controls.Add(lblUsername);
            pnlBody.Controls.Add(lblSubtitle);
            pnlBody.Controls.Add(lblBrand);
            pnlBody.Controls.Add(lblFooter);

            pnlCard.Controls.Add(pnlBody);
            pnlCard.Controls.Add(pnlAccent);
            Controls.Add(pnlCard);

            pnlBody.ResumeLayout(false);
            pnlCard.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
