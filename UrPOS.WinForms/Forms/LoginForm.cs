using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UrPOS.Core.Interfaces;

namespace UrPOS.WinForms.Forms
{
    public partial class LoginForm : Form
    {
        private readonly IAuthService _authService;
        private bool _isBusy;

        public LoginForm(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterLoginCard();
            txtUsername.Focus();
        }

        private async void btnLogin_Click(object? sender, EventArgs e)
        {
            await AttemptLoginAsync();
        }

        private async void txtPassword_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await AttemptLoginAsync();
            }
        }

        private async void txtUsername_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtPassword.Focus();
            }
        }

        private async Task AttemptLoginAsync()
        {
            if (_isBusy)
            {
                return;
            }

            HideError();

            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("يرجى إدخال اسم المستخدم.");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("يرجى إدخال كلمة المرور.");
                txtPassword.Focus();
                return;
            }

            try
            {
                SetBusy(true);

                var success = await _authService.LoginAsync(username, password);
                if (!success)
                {
                    ShowError("فشل تسجيل الدخول. تحقق من اسم المستخدم أو كلمة المرور.");
                    txtPassword.SelectAll();
                    txtPassword.Focus();
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                ShowError("حدث خطأ غير متوقع أثناء تسجيل الدخول. حاول مرة أخرى.");
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void SetBusy(bool isBusy)
        {
            _isBusy = isBusy;
            btnLogin.Enabled = !isBusy;
            txtUsername.Enabled = !isBusy;
            txtPassword.Enabled = !isBusy;
            btnLogin.Text = isBusy ? "جاري التحقق..." : "تسجيل الدخول";
            UseWaitCursor = isBusy;
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private void HideError()
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
        }
    }
}
