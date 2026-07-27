using System.Drawing;
using System.Windows.Forms;

namespace UrPOS.WinForms.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlBackground;
        private Panel pnlCard;
        private Panel pnlAccent;
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

            pnlBackground = new Panel();
            pnlCard = new Panel();
            pnlAccent = new Panel();
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
            pnlBackground.SuspendLayout();
            pnlCard.SuspendLayout();

            // LoginForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 23, 42);
            ClientSize = new Size(980, 640);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(720, 560);
            MaximizeBox = true;
            MinimizeBox = true;
            Name = "LoginForm";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UrPOS — تسجيل الدخول";

            // pnlBackground — fills form; card is centered inside
            pnlBackground.BackColor = Color.FromArgb(15, 23, 42);
            pnlBackground.Dock = DockStyle.Fill;
            pnlBackground.Name = "pnlBackground";

            // pnlCard
            pnlCard.Anchor = AnchorStyles.None;
            pnlCard.BackColor = Color.FromArgb(248, 250, 252);
            pnlCard.Location = new Point(265, 80);
            pnlCard.Name = "pnlCard";
            pnlCard.Padding = new Padding(0);
            pnlCard.Size = new Size(450, 540);

            // pnlAccent — teal strip at top of card
            pnlAccent.BackColor = Color.FromArgb(13, 148, 136);
            pnlAccent.Dock = DockStyle.Top;
            pnlAccent.Height = 6;
            pnlAccent.Name = "pnlAccent";

            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Name = "pnlBody",
                Padding = new Padding(40, 28, 40, 28)
            };

            // lblBrand
            lblBrand.Dock = DockStyle.Top;
            lblBrand.Font = new Font("Segoe UI Semibold", 30F, FontStyle.Bold, GraphicsUnit.Point);
            lblBrand.ForeColor = Color.FromArgb(15, 23, 42);
            lblBrand.Name = "lblBrand";
            lblBrand.Padding = new Padding(0, 4, 0, 0);
            lblBrand.Size = new Size(370, 52);
            lblBrand.Text = "UrPOS";
            lblBrand.TextAlign = ContentAlignment.MiddleCenter;

            // lblSubtitle
            lblSubtitle.Dock = DockStyle.Top;
            lblSubtitle.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Padding = new Padding(0, 0, 0, 18);
            lblSubtitle.Size = new Size(370, 40);
            lblSubtitle.Text = "نظام نقاط البيع — تسجيل الدخول";
            lblSubtitle.TextAlign = ContentAlignment.TopCenter;

            // lblUsername
            lblUsername.Dock = DockStyle.Top;
            lblUsername.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblUsername.ForeColor = Color.FromArgb(51, 65, 85);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(370, 26);
            lblUsername.Text = "اسم المستخدم";
            lblUsername.TextAlign = ContentAlignment.MiddleRight;

            // txtUsername
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Dock = DockStyle.Top;
            txtUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "أدخل اسم المستخدم";
            txtUsername.RightToLeft = RightToLeft.Yes;
            txtUsername.Size = new Size(370, 29);
            txtUsername.TabIndex = 0;
            txtUsername.TextAlign = HorizontalAlignment.Right;
            txtUsername.KeyDown += txtUsername_KeyDown;

            var spUsername = new Panel
            {
                Dock = DockStyle.Top,
                Height = 16,
                Name = "spUsername"
            };

            // lblPassword
            lblPassword.Dock = DockStyle.Top;
            lblPassword.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(370, 26);
            lblPassword.Text = "كلمة المرور";
            lblPassword.TextAlign = ContentAlignment.MiddleRight;

            // txtPassword
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Dock = DockStyle.Top;
            txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.PlaceholderText = "أدخل كلمة المرور";
            txtPassword.RightToLeft = RightToLeft.Yes;
            txtPassword.Size = new Size(370, 29);
            txtPassword.TabIndex = 1;
            txtPassword.TextAlign = HorizontalAlignment.Right;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.KeyDown += txtPassword_KeyDown;

            var spPassword = new Panel
            {
                Dock = DockStyle.Top,
                Height = 16,
                Name = "spPassword"
            };

            // lblError
            lblError.Dock = DockStyle.Top;
            lblError.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblError.ForeColor = Color.FromArgb(185, 28, 28);
            lblError.Name = "lblError";
            lblError.Padding = new Padding(4, 6, 4, 6);
            lblError.Size = new Size(370, 40);
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            lblError.Visible = false;

            var spError = new Panel
            {
                Dock = DockStyle.Top,
                Height = 12,
                Name = "spError"
            };

            // btnLogin
            btnLogin.BackColor = Color.FromArgb(13, 148, 136);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.DialogResult = DialogResult.None;
            btnLogin.Dock = DockStyle.Top;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnLogin.ForeColor = Color.White;
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(370, 48);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "تسجيل الدخول";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;

            var spGuest = new Panel
            {
                Dock = DockStyle.Top,
                Height = 12,
                Name = "spGuest"
            };

            // btnCreateGuest
            btnCreateGuest.BackColor = Color.FromArgb(248, 250, 252);
            btnCreateGuest.Cursor = Cursors.Hand;
            btnCreateGuest.DialogResult = DialogResult.None;
            btnCreateGuest.Dock = DockStyle.Top;
            btnCreateGuest.FlatAppearance.BorderColor = Color.FromArgb(13, 148, 136);
            btnCreateGuest.FlatAppearance.BorderSize = 1;
            btnCreateGuest.FlatStyle = FlatStyle.Flat;
            btnCreateGuest.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnCreateGuest.ForeColor = Color.FromArgb(13, 148, 136);
            btnCreateGuest.Name = "btnCreateGuest";
            btnCreateGuest.Size = new Size(370, 44);
            btnCreateGuest.TabIndex = 3;
            btnCreateGuest.Text = "دخول كضيف";
            btnCreateGuest.UseVisualStyleBackColor = false;
            btnCreateGuest.Click += btnCreateGuest_Click;

            // lblFooter
            lblFooter.Dock = DockStyle.Bottom;
            lblFooter.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblFooter.ForeColor = Color.FromArgb(148, 163, 184);
            lblFooter.Height = 28;
            lblFooter.Name = "lblFooter";
            lblFooter.Text = "واجهة عربية (RTL) — UrPOS";
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;

            // Dock order (bottom-first for Top dock)
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

            pnlBackground.Controls.Add(pnlCard);
            Controls.Add(pnlBackground);

            Load += (_, _) => CenterLoginCard();
            Resize += (_, _) => CenterLoginCard();

            pnlCard.ResumeLayout(false);
            pnlBackground.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void CenterLoginCard()
        {
            if (pnlCard == null || pnlBackground == null)
            {
                return;
            }

            pnlCard.Left = Math.Max(0, (pnlBackground.ClientSize.Width - pnlCard.Width) / 2);
            pnlCard.Top = Math.Max(0, (pnlBackground.ClientSize.Height - pnlCard.Height) / 2);
        }
    }
}
