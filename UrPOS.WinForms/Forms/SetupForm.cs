using UrPOS.Core.Interfaces;
using UrPOS.Core.Entities;

namespace UrPOS.WinForms
{
    public partial class SetupForm : Form
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public SetupForm(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            InitializeComponent();
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        private async void btnCreateAdmin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("يرجى إدخال اسم المستخدم وكلمة المرور.");
                return;
            }

            var newUser = new User
            {
                Username = txtUsername.Text.Trim(),
                PasswordHash = _passwordHasher.HashPassword(txtPassword.Text.Trim()),
                FullName = "مستخدم جديد",
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                IsActive = true,
            };

            await _userRepository.CreateAdminUserAsync(newUser);

            MessageBox.Show("تم إنشاء حساب المدير بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }



        private void SetupWizardForm_Load(object sender, EventArgs e)
        {
        }



    }
}
