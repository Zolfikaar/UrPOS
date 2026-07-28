using System.Drawing;
using System.Windows.Forms;

namespace UrPOS.WinForms
{
    partial class SetupForm
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
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPhone;
        private TextBox txtPhone;
        private Button btnCreateAdmin;
        private Button btnCreateGuest;
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
            btnCreateAdmin = new Button();
            btnCreateGuest = new Button();
            lblFooter = new Label();

            SuspendLayout();
            pnlCard.SuspendLayout();
            pnlBody.SuspendLayout();

            // Compact dialog — card fills the form (avoids DPI/RTL centering collapse)
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(15, 23, 42);
            ClientSize = new Size(480, 640);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SetupForm";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UrPOS — إعداد أول تشغيل";
            Load += SetupWizardForm_Load;

            pnlCard.BackColor = Color.FromArgb(248, 250, 252);
            pnlCard.Dock = DockStyle.Fill;
            pnlCard.Name = "pnlCard";
            pnlCard.Padding = new Padding(0);
            pnlCard.RightToLeft = RightToLeft.Yes;

            pnlAccent.BackColor = Color.FromArgb(13, 148, 136);
            pnlAccent.Dock = DockStyle.Top;
            pnlAccent.Height = 6;
            pnlAccent.Name = "pnlAccent";

            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(36, 20, 36, 16);
            pnlBody.RightToLeft = RightToLeft.Yes;

            lblBrand.Dock = DockStyle.Top;
            lblBrand.Font = new Font("Segoe UI Semibold", 26F, FontStyle.Bold, GraphicsUnit.Point);
            lblBrand.ForeColor = Color.FromArgb(15, 23, 42);
            lblBrand.Height = 44;
            lblBrand.Name = "lblBrand";
            lblBrand.Text = "UrPOS";
            lblBrand.TextAlign = ContentAlignment.MiddleCenter;

            lblSubtitle.Dock = DockStyle.Top;
            lblSubtitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Height = 36;
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Text = "معالج الإعداد الأول — أنشئ حساب المدير";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            lblUsername = CreateFieldLabel("lblUsername", "اسم المستخدم");
            txtUsername = CreateFieldTextBox("txtUsername", "أدخل اسم المستخدم", 0);
            var sp1 = CreateSpacer("sp1", 10);

            lblPassword = CreateFieldLabel("lblPassword", "كلمة المرور");
            txtPassword = CreateFieldTextBox("txtPassword", "أدخل كلمة المرور", 1);
            txtPassword.PasswordChar = '●';
            txtPassword.UseSystemPasswordChar = true;
            var sp2 = CreateSpacer("sp2", 10);

            lblEmail = CreateFieldLabel("lblEmail", "البريد الإلكتروني");
            txtEmail = CreateFieldTextBox("txtEmail", "example@store.com", 2);
            var sp3 = CreateSpacer("sp3", 10);

            lblPhone = CreateFieldLabel("lblPhone", "رقم الهاتف");
            txtPhone = CreateFieldTextBox("txtPhone", "07xxxxxxxx", 3);
            var sp4 = CreateSpacer("sp4", 16);

            btnCreateAdmin.BackColor = Color.FromArgb(13, 148, 136);
            btnCreateAdmin.Cursor = Cursors.Hand;
            btnCreateAdmin.Dock = DockStyle.Top;
            btnCreateAdmin.FlatAppearance.BorderSize = 0;
            btnCreateAdmin.FlatStyle = FlatStyle.Flat;
            btnCreateAdmin.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnCreateAdmin.ForeColor = Color.White;
            btnCreateAdmin.Height = 46;
            btnCreateAdmin.Name = "btnCreateAdmin";
            btnCreateAdmin.TabIndex = 4;
            btnCreateAdmin.Text = "حفظ ودخول";
            btnCreateAdmin.UseVisualStyleBackColor = false;
            btnCreateAdmin.Click += btnCreateAdmin_Click;

            var sp5 = CreateSpacer("sp5", 10);

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
            btnCreateGuest.TabIndex = 5;
            btnCreateGuest.Text = "الدخول كضيف";
            btnCreateGuest.UseVisualStyleBackColor = false;
            btnCreateGuest.Click += BtnCreateGuest_Click;

            lblFooter.Dock = DockStyle.Bottom;
            lblFooter.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblFooter.ForeColor = Color.FromArgb(148, 163, 184);
            lblFooter.Height = 26;
            lblFooter.Name = "lblFooter";
            lblFooter.Text = "واجهة عربية (RTL) — إعداد أول تشغيل";
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;

            // Top dock: add bottom-first so brand ends up at the top
            pnlBody.Controls.Add(btnCreateGuest);
            pnlBody.Controls.Add(sp5);
            pnlBody.Controls.Add(btnCreateAdmin);
            pnlBody.Controls.Add(sp4);
            pnlBody.Controls.Add(txtPhone);
            pnlBody.Controls.Add(lblPhone);
            pnlBody.Controls.Add(sp3);
            pnlBody.Controls.Add(txtEmail);
            pnlBody.Controls.Add(lblEmail);
            pnlBody.Controls.Add(sp2);
            pnlBody.Controls.Add(txtPassword);
            pnlBody.Controls.Add(lblPassword);
            pnlBody.Controls.Add(sp1);
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

        private static Label CreateFieldLabel(string name, string text)
        {
            return new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(51, 65, 85),
                Height = 24,
                Name = name,
                RightToLeft = RightToLeft.Yes,
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private static TextBox CreateFieldTextBox(string name, string placeholder, int tabIndex)
        {
            return new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                Height = 30,
                Name = name,
                PlaceholderText = placeholder,
                RightToLeft = RightToLeft.Yes,
                TabIndex = tabIndex,
                TextAlign = HorizontalAlignment.Right
            };
        }

        private static Panel CreateSpacer(string name, int height)
        {
            return new Panel
            {
                Dock = DockStyle.Top,
                Height = height,
                Name = name
            };
        }
    }
}
