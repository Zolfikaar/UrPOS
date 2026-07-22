using System.Drawing;
using System.Windows.Forms;

namespace UrPOS.WinForms.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlCard;
        private Label lblBrand;
        private Label lblSubtitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblError;

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
            lblBrand = new Label();
            lblSubtitle = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            lblError = new Label();

            SuspendLayout();
            pnlCard.SuspendLayout();

            // LoginForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 23, 42);
            ClientSize = new Size(980, 640);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UrPOS — تسجيل الدخول";

            // pnlCard
            pnlCard.Anchor = AnchorStyles.None;
            pnlCard.BackColor = Color.FromArgb(248, 250, 252);
            pnlCard.Location = new Point(265, 95);
            pnlCard.Name = "pnlCard";
            pnlCard.Padding = new Padding(36, 32, 36, 32);
            pnlCard.Size = new Size(450, 450);

            // lblBrand
            lblBrand.Dock = DockStyle.Top;
            lblBrand.Font = new Font("Segoe UI Semibold", 28F, FontStyle.Bold, GraphicsUnit.Point);
            lblBrand.ForeColor = Color.FromArgb(15, 23, 42);
            lblBrand.Name = "lblBrand";
            lblBrand.Padding = new Padding(0, 8, 0, 0);
            lblBrand.Size = new Size(378, 56);
            lblBrand.Text = "UrPOS";
            lblBrand.TextAlign = ContentAlignment.MiddleCenter;

            // lblSubtitle
            lblSubtitle.Dock = DockStyle.Top;
            lblSubtitle.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Padding = new Padding(0, 0, 0, 20);
            lblSubtitle.Size = new Size(378, 44);
            lblSubtitle.Text = "نظام نقاط البيع";
            lblSubtitle.TextAlign = ContentAlignment.TopCenter;

            // lblUsername
            lblUsername.Dock = DockStyle.Top;
            lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblUsername.ForeColor = Color.FromArgb(51, 65, 85);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(378, 28);
            lblUsername.Text = "اسم المستخدم";
            lblUsername.TextAlign = ContentAlignment.MiddleRight;

            // txtUsername
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Dock = DockStyle.Top;
            txtUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtUsername.Margin = new Padding(0, 0, 0, 12);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "أدخل اسم المستخدم";
            txtUsername.Size = new Size(378, 29);
            txtUsername.TabIndex = 0;
            txtUsername.KeyDown += txtUsername_KeyDown;

            // spacer after username
            var spUsername = new Panel
            {
                Dock = DockStyle.Top,
                Height = 14,
                Name = "spUsername"
            };

            // lblPassword
            lblPassword.Dock = DockStyle.Top;
            lblPassword.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(378, 28);
            lblPassword.Text = "كلمة المرور";
            lblPassword.TextAlign = ContentAlignment.MiddleRight;

            // txtPassword
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Dock = DockStyle.Top;
            txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.PlaceholderText = "أدخل كلمة المرور";
            txtPassword.Size = new Size(378, 29);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.KeyDown += txtPassword_KeyDown;

            var spPassword = new Panel
            {
                Dock = DockStyle.Top,
                Height = 18,
                Name = "spPassword"
            };

            // lblError
            lblError.Dock = DockStyle.Top;
            lblError.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblError.ForeColor = Color.FromArgb(185, 28, 28);
            lblError.Name = "lblError";
            lblError.Padding = new Padding(4, 6, 4, 6);
            lblError.Size = new Size(378, 42);
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
            btnLogin.Dock = DockStyle.Top;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnLogin.ForeColor = Color.White;
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(378, 48);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "تسجيل الدخول";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;

            // Dock order (bottom-first for Top dock)
            pnlCard.Controls.Add(btnLogin);
            pnlCard.Controls.Add(spError);
            pnlCard.Controls.Add(lblError);
            pnlCard.Controls.Add(spPassword);
            pnlCard.Controls.Add(txtPassword);
            pnlCard.Controls.Add(lblPassword);
            pnlCard.Controls.Add(spUsername);
            pnlCard.Controls.Add(txtUsername);
            pnlCard.Controls.Add(lblUsername);
            pnlCard.Controls.Add(lblSubtitle);
            pnlCard.Controls.Add(lblBrand);

            Controls.Add(pnlCard);

            Resize += (_, _) =>
            {
                pnlCard.Left = (ClientSize.Width - pnlCard.Width) / 2;
                pnlCard.Top = (ClientSize.Height - pnlCard.Height) / 2;
            };

            pnlCard.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
