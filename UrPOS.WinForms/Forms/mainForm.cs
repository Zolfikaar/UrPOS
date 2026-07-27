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
        private bool _guestCleanupCompleted;

        public MainForm(IServiceProvider serviceProvider, IAuthService authService)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            InitializeComponent();
            ApplySessionInfo();
            FormClosing += MainForm_FormClosing;
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

        private async void btnLogout_Click(object sender, EventArgs e)
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

            if (UserSession.Instance.IsGuest)
            {
                await _authService.CleanupGuestSessionAsync();
            }
            else
            {
                _authService.Logout();
            }

            _guestCleanupCompleted = true;
            DialogResult = DialogResult.Retry;
            Close();
        }

        private async void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_guestCleanupCompleted || !UserSession.Instance.IsGuest)
            {
                return;
            }

            // إلغاء الإغلاق مؤقتاً حتى يكتمل حذف سجل الضيف من PostgreSQL
            e.Cancel = true;
            _guestCleanupCompleted = true;

            try
            {
                await _authService.CleanupGuestSessionAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    $"تعذر حذف سجل الضيف من قاعدة البيانات: {ex.Message}",
                    "تحذير",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }

            // أعد الإغلاق بعد اكتمال التنظيف (DialogResult يبقى كما هو — Retry من Logout أو None عند X)
            BeginInvoke(new Action(Close));
        }
    }
}
