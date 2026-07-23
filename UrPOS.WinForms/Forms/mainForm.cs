using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;

namespace UrPOS.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthService _authService;

        public MainForm(IServiceProvider serviceProvider, IAuthService authService)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            InitializeComponent();
            ApplySessionInfo();
        }

        private void ApplySessionInfo()
        {
            var session = UserSession.Instance;
            lblWelcome.Text = session.IsLoggedIn
                ? $"مرحباً، {session.FullName}"
                : "مرحباً";
            lblRole.Text = session.IsLoggedIn
                ? $"الدور: {session.RoleName}"
                : string.Empty;
        }

        private void btnOpenPos_Click(object? sender, EventArgs e)
        {
            using var posForm = _serviceProvider.GetRequiredService<PosSalesForm>();
            posForm.ShowDialog(this);
        }

        private void btnNavProducts_Click(object? sender, EventArgs e)
        {
            ShowUiPlaceholder("إدارة المنتجات", "شاشة إدارة المنتجات قيد التجهيز (واجهة فقط).");
        }

        private void btnNavInvoices_Click(object? sender, EventArgs e)
        {
            ShowUiPlaceholder("سجل الفواتير", "شاشة سجل الفواتير قيد التجهيز (واجهة فقط).");
        }

        private void btnNavSettings_Click(object? sender, EventArgs e)
        {
            ShowUiPlaceholder("الإعدادات والأمان", "شاشة الإعدادات والأمان قيد التجهيز (واجهة فقط).");
        }

        private void ShowUiPlaceholder(string title, string message)
        {
            MessageBox.Show(
                this,
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                this,
                "هل تريد تسجيل الخروج؟",
                "تأكيد",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            _authService.Logout();
            DialogResult = DialogResult.Retry;
            Close();
        }
    }
}
