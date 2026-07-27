namespace UrPOS.WinForms
{
    partial class SetupForm
    {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnCreateAdmin = new Button();
            lblUsername = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtEmail = new TextBox();
            lblEmail = new Label();
            txtPhone = new TextBox();
            lblPhone = new Label();
            btnCreateGuest = new Button();
            SuspendLayout();
            // 
            // btnCreateAdmin
            // 
            btnCreateAdmin.Font = new Font("Segoe UI", 10F);
            btnCreateAdmin.Location = new Point(35, 329);
            btnCreateAdmin.Name = "btnCreateAdmin";
            btnCreateAdmin.Size = new Size(457, 50);
            btnCreateAdmin.TabIndex = 0;
            btnCreateAdmin.Text = "حفظ و دخول";
            btnCreateAdmin.UseVisualStyleBackColor = true;
            btnCreateAdmin.Click += btnCreateAdmin_Click;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.FlatStyle = FlatStyle.System;
            lblUsername.Font = new Font("Segoe UI", 12F);
            lblUsername.Location = new Point(355, 82);
            lblUsername.Name = "lblUsername";
            lblUsername.RightToLeft = RightToLeft.Yes;
            lblUsername.Size = new Size(137, 28);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "اسم المستخدم:";
            lblUsername.TextAlign = ContentAlignment.TopRight;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(35, 86);
            txtUsername.Name = "txtUsername";
            txtUsername.RightToLeft = RightToLeft.Yes;
            txtUsername.Size = new Size(275, 27);
            txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(35, 141);
            txtPassword.Name = "txtPassword";
            txtPassword.RightToLeft = RightToLeft.Yes;
            txtPassword.Size = new Size(275, 27);
            txtPassword.TabIndex = 4;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.FlatStyle = FlatStyle.System;
            lblPassword.Font = new Font("Segoe UI", 12F);
            lblPassword.Location = new Point(355, 137);
            lblPassword.Name = "lblPassword";
            lblPassword.RightToLeft = RightToLeft.Yes;
            lblPassword.Size = new Size(95, 28);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "كلمة السر:";
            lblPassword.TextAlign = ContentAlignment.TopRight;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(35, 201);
            txtEmail.Name = "txtEmail";
            txtEmail.RightToLeft = RightToLeft.Yes;
            txtEmail.Size = new Size(275, 27);
            txtEmail.TabIndex = 6;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.FlatStyle = FlatStyle.System;
            lblEmail.Font = new Font("Segoe UI", 12F);
            lblEmail.Location = new Point(355, 197);
            lblEmail.Name = "lblEmail";
            lblEmail.RightToLeft = RightToLeft.Yes;
            lblEmail.Size = new Size(148, 28);
            lblEmail.TabIndex = 5;
            lblEmail.Text = "البريد الالكتروني:";
            lblEmail.TextAlign = ContentAlignment.TopRight;
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(35, 258);
            txtPhone.Name = "txtPhone";
            txtPhone.RightToLeft = RightToLeft.Yes;
            txtPhone.Size = new Size(275, 27);
            txtPhone.TabIndex = 8;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.FlatStyle = FlatStyle.System;
            lblPhone.Font = new Font("Segoe UI", 12F);
            lblPhone.Location = new Point(355, 254);
            lblPhone.Name = "lblPhone";
            lblPhone.RightToLeft = RightToLeft.Yes;
            lblPhone.Size = new Size(137, 28);
            lblPhone.TabIndex = 7;
            lblPhone.Text = "اسم المستخدم:";
            lblPhone.TextAlign = ContentAlignment.TopRight;
            // 
            // btnCreateGuest
            // 
            btnCreateGuest.Font = new Font("Segoe UI", 10F);
            btnCreateGuest.Location = new Point(35, 482);
            btnCreateGuest.Name = "btnCreateGuest";
            btnCreateGuest.Size = new Size(457, 50);
            btnCreateGuest.TabIndex = 9;
            btnCreateGuest.Text = "الدخول كضيف";
            btnCreateGuest.UseVisualStyleBackColor = true;
            // 
            // SetupForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(528, 662);
            Controls.Add(btnCreateGuest);
            Controls.Add(txtPhone);
            Controls.Add(lblPhone);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblUsername);
            Controls.Add(btnCreateAdmin);
            Name = "SetupForm";
            Text = "First Setup Form";
            Load += SetupWizardForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCreateAdmin;
        private Label lblUsername;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label lblPassword;
        private TextBox txtEmail;
        private Label lblEmail;
        private TextBox txtPhone;
        private Label lblPhone;
        private Button btnCreateGuest;


        
    }
}
