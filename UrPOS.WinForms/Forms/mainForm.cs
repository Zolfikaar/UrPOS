using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.WinForms.Controls;

namespace UrPOS.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthService _authService;
        private readonly AppConfigurations _configs;
        private bool _guestCleanupCompleted;

        private ProductsControl? _productsControl;
        private InvoicesHistoryControl? _invoicesHistory;
        private SettingsControl? _settingsControl;

        public MainForm(IServiceProvider serviceProvider, IAuthService authService, AppConfigurations configs)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _configs = configs ?? throw new ArgumentNullException(nameof(configs));
            InitializeComponent();
            ApplySessionInfo();
            HighlightNav(btnNavDashboard);
            FormClosing += MainForm_FormClosing;
        }

        public void RefreshSessionHeader() => ApplySessionInfo();

        private void ApplySessionInfo()
        {
            var session = UserSession.Instance;
            var demoActive = session.IsGuest || session.IsDemoMode || _configs.IsDemoModeEnabled;

            if (session.IsLoggedIn && demoActive)
            {
                if (_configs.IsDemoModeEnabled && !_configs.DemoTrialStartedAtUtc.HasValue)
                {
                    _configs.DemoTrialStartedAtUtc = DateTime.UtcNow;
                }

                var displayName = session.IsGuest ? "زائر تجريبي" : session.FullName;
                var role = string.IsNullOrWhiteSpace(session.RoleName) ? "Admin" : session.RoleName;
                var days = _configs.GetDemoDaysRemaining();
                lblWelcome.Text = $"مرحباً، {displayName} | الدور: {role} | الفترة التجريبية: {days} أيام";
                lblRole.Text = "وضع التجربة مفعّل";
                pnlUserInfo.Width = 520;
            }
            else if (session.IsLoggedIn)
            {
                lblWelcome.Text = $"مرحباً، {session.FullName}";
                lblRole.Text = $"الدور: {session.RoleName}";
                pnlUserInfo.Width = 280;
            }
            else
            {
                lblWelcome.Text = "مرحباً";
                lblRole.Text = string.Empty;
                pnlUserInfo.Width = 280;
            }

            session.IsDemoMode = demoActive;
        }

        private void ShowInContentHost(Control view)
        {
            // Never Clear()/re-parent views: that re-runs AutoScale and collapses
            // Dock.Top sidebar buttons and content until the shell looks empty.
            pnlContentHost.SuspendLayout();

            if (!pnlContentHost.Controls.Contains(view))
            {
                view.Dock = DockStyle.Fill;
                pnlContentHost.Controls.Add(view);
            }

            foreach (Control child in pnlContentHost.Controls)
            {
                child.Visible = ReferenceEquals(child, view);
            }

            view.BringToFront();
            pnlContentHost.ResumeLayout(true);
        }

        private void ShowDashboard()
        {
            ShowInContentHost(pnlDashboard);
            HighlightNav(btnNavDashboard);
        }

        private void btnNavDashboard_Click(object? sender, EventArgs e)
        {
            ShowDashboard();
        }

        private async void btnOpenPos_Click(object? sender, EventArgs e)
        {
            using var posForm = _serviceProvider.GetRequiredService<PosSalesForm>();
            posForm.ShowDialog(this);

            // POS sales change stock in the DB — refresh the cached products grid.
            if (_productsControl is not null)
            {
                await _productsControl.ReloadProductsAsync();
            }
        }

        private async void btnNavProducts_Click(object? sender, EventArgs e)
        {
            EnsureProductsControl();
            ShowInContentHost(_productsControl!);
            HighlightNav(btnNavProducts);
            await _productsControl!.ReloadProductsAsync();
        }

        private async void btnQuickAddProduct_Click(object? sender, EventArgs e)
        {
            EnsureProductsControl();
            ShowInContentHost(_productsControl!);
            HighlightNav(btnNavProducts);
            await _productsControl!.ReloadProductsAsync();
            _productsControl.OpenAddProductForm();
        }

        private void EnsureProductsControl()
        {
            _productsControl ??= _serviceProvider.GetRequiredService<ProductsControl>();
        }

        private async void btnNavInvoices_Click(object? sender, EventArgs e)
        {
            EnsureInvoicesHistoryControl();
            ShowInContentHost(_invoicesHistory!);
            HighlightNav(btnNavInvoices);
            await _invoicesHistory!.ReloadAsync();
        }

        private void EnsureInvoicesHistoryControl()
        {
            if (_invoicesHistory is not null)
            {
                return;
            }

            _invoicesHistory = _serviceProvider.GetRequiredService<InvoicesHistoryControl>();
            _invoicesHistory.ProductsMayHaveChanged += async (_, _) =>
            {
                if (_productsControl is not null)
                {
                    await _productsControl.ReloadProductsAsync();
                }
            };
        }

        private void btnNavSettings_Click(object? sender, EventArgs e)
        {
            _settingsControl ??= _serviceProvider.GetRequiredService<SettingsControl>();
            ShowInContentHost(_settingsControl);
            HighlightNav(btnNavSettings);
            ApplySessionInfo();
        }

        private void btnQuickBackup_Click(object? sender, EventArgs e)
        {
            _settingsControl ??= _serviceProvider.GetRequiredService<SettingsControl>();
            _settingsControl.ShowBackupTab();
            ShowInContentHost(_settingsControl);
            HighlightNav(btnNavSettings);
        }

        private void HighlightNav(Button active)
        {
            var idle = Color.FromArgb(51, 65, 85);
            var accent = Color.FromArgb(13, 148, 136);

            foreach (var btn in new[] { btnNavDashboard, btnNavPos, btnNavProducts, btnNavInvoices, btnNavSettings })
            {
                btn.BackColor = ReferenceEquals(btn, active) ? accent : idle;
            }
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

            ParkedInvoiceSession.Instance.Clear();
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

            BeginInvoke(new Action(Close));
        }
    }
}
