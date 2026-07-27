using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.Infrastructure.Services;

namespace UrPOS.WinForms
{
    public partial class SetupForm : Form
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthService _authService;

        public SetupForm(IUserRepository userRepository, IPasswordHasher passwordHasher, IAuthService authService)
        {
            InitializeComponent();
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authService = authService;
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


        private async void BtnCreateGuest_Click(object sender, EventArgs e)
        {

            await _authService.LoginAsGuestAsync();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
