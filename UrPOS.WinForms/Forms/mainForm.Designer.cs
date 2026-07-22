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
        private Panel pnlContent;
        private Label lblTitle;
        private Label lblHint;
        private Button btnOpenPos;

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
            pnlContent = new Panel();
            lblTitle = new Label();
            lblHint = new Label();
            btnOpenPos = new Button();

            SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlContent.SuspendLayout();

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

            // pnlHeader
            pnlHeader.BackColor = Color.FromArgb(15, 23, 42);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 72;
            pnlHeader.Padding = new Padding(24, 12, 24, 12);

            // lblBrand
            lblBrand.AutoSize = true;
            lblBrand.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(24, 18);
            lblBrand.Name = "lblBrand";
            lblBrand.Text = "UrPOS";

            // lblWelcome
            lblWelcome.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblWelcome.ForeColor = Color.FromArgb(226, 232, 240);
            lblWelcome.Location = new Point(820, 12);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Text = "مرحباً";

            // lblRole
            lblRole.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblRole.ForeColor = Color.FromArgb(148, 163, 184);
            lblRole.Location = new Point(820, 38);
            lblRole.Name = "lblRole";
            lblRole.Text = string.Empty;

            // btnLogout
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

            // pnlContent
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Padding = new Padding(48, 56, 48, 48);

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Location = new Point(48, 72);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "لوحة التحكم";

            // lblHint
            lblHint.AutoSize = true;
            lblHint.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblHint.ForeColor = Color.FromArgb(71, 85, 105);
            lblHint.Location = new Point(48, 120);
            lblHint.Name = "lblHint";
            lblHint.Text = "اختر شاشة الكاشير لبدء عملية البيع.";

            // btnOpenPos
            btnOpenPos.BackColor = Color.FromArgb(13, 148, 136);
            btnOpenPos.Cursor = Cursors.Hand;
            btnOpenPos.FlatAppearance.BorderSize = 0;
            btnOpenPos.FlatStyle = FlatStyle.Flat;
            btnOpenPos.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
            btnOpenPos.ForeColor = Color.White;
            btnOpenPos.Location = new Point(48, 180);
            btnOpenPos.Name = "btnOpenPos";
            btnOpenPos.Size = new Size(280, 64);
            btnOpenPos.Text = "فتح شاشة الكاشير";
            btnOpenPos.UseVisualStyleBackColor = false;
            btnOpenPos.Click += btnOpenPos_Click;

            pnlContent.Controls.Add(btnOpenPos);
            pnlContent.Controls.Add(lblHint);
            pnlContent.Controls.Add(lblTitle);

            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);

            Load += (_, _) =>
            {
                // Keep header actions readable in RTL maximized layout
                btnLogout.Left = 24;
                lblWelcome.Left = ClientSize.Width - 280;
                lblRole.Left = ClientSize.Width - 280;
            };

            Resize += (_, _) =>
            {
                btnLogout.Left = 24;
                lblWelcome.Left = Math.Max(160, ClientSize.Width - 280);
                lblRole.Left = Math.Max(160, ClientSize.Width - 280);
            };

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            ResumeLayout(false);
        }
    }
}
