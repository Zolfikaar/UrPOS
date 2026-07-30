using System.Drawing;
using System.Windows.Forms;
using UrPOS.Core.Interfaces;

namespace UrPOS.WinForms.Controls
{
    /// <summary>
    /// Settings page UI with Arabic RTL tabs, including encrypted database backup/restore.
    /// </summary>
    public class SettingsControl : UserControl
    {
        private readonly TabControl _tabs;
        private readonly IBackupService _backupService;

        public SettingsControl(IBackupService backupService)
        {
            _backupService = backupService;

            AutoScaleMode = AutoScaleMode.None;
            RightToLeft = RightToLeft.Yes;
            BackColor = Color.FromArgb(241, 245, 249);
            Dock = DockStyle.Fill;
            Padding = new Padding(24, 16, 24, 24);

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(15, 23, 42),
                Height = 44,
                Text = "الإعدادات والأمان",
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblHint = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(100, 116, 139),
                Height = 28,
                Text = "إدارة بيانات المتجر، النسخ الاحتياطي المشفر، وتفضيلات النظام.",
                TextAlign = ContentAlignment.MiddleRight
            };

            _tabs = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true,
                Name = "tabsSettings"
            };

            _tabs.TabPages.Add(BuildStoreProfileTab());
            _tabs.TabPages.Add(BuildBackupSecurityTab());
            _tabs.TabPages.Add(BuildSystemPrefsTab());

            Controls.Add(_tabs);
            Controls.Add(lblHint);
            Controls.Add(lblTitle);
        }

        public void ShowBackupTab()
        {
            if (_tabs.TabPages.Count > 1)
            {
                _tabs.SelectedIndex = 1;
            }
        }

        private static TabPage BuildStoreProfileTab()
        {
            var page = CreateTabPage("tabStore", "بيانات المتجر");
            var flow = CreateFlowBody();

            flow.Controls.Add(CreatePlaceholderNote(
                "ملف المتجر وهوية الإيصال",
                "اسم المتجر، الهاتف، العنوان، ونص تذييل الفاتورة — placeholders للربط مع AppConfigurations."));
            flow.Controls.Add(CreateLabeledField("اسم المتجر", "متجر مدار"));
            flow.Controls.Add(CreateLabeledField("هاتف المتجر", ""));
            flow.Controls.Add(CreateLabeledField("العنوان", ""));
            flow.Controls.Add(CreateLabeledField("نص تذييل الإيصال", "شكراً لزيارتكم!"));
            flow.Controls.Add(CreateActionButton("حفظ بيانات المتجر (قريباً)", Color.FromArgb(13, 148, 136)));

            page.Controls.Add(flow);
            return page;
        }

        private TabPage BuildBackupSecurityTab()
        {
            var page = CreateTabPage("tabBackup", "النسخ الاحتياطي والأمان");
            var flow = CreateFlowBody();

            flow.Controls.Add(CreatePlaceholderNote(
                "نسخ احتياطي واستعادة قاعدة البيانات",
                "إنشاء نسخة احتياطية مشفرة بـ AES-256 أو استعادة ملف .upbak. تأكد من تثبيت أدوات PostgreSQL (pg_dump / psql)."));

            var row = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Margin = new Padding(0, 8, 0, 8),
                RightToLeft = RightToLeft.Yes,
                WrapContents = false,
                Width = 720
            };

            var btnBackup = CreateActionButton("إنشاء نسخة احتياطية مشفرة", Color.FromArgb(13, 148, 136));
            btnBackup.MinimumSize = new Size(220, 40);
            btnBackup.Click += async (_, _) => await CreateEncryptedBackupAsync();

            var btnRestore = CreateActionButton("استعادة نسخة احتياطية", Color.FromArgb(51, 65, 85));
            btnRestore.MinimumSize = new Size(200, 40);
            btnRestore.Click += async (_, _) => await RestoreEncryptedBackupAsync();

            row.Controls.Add(btnBackup);
            row.Controls.Add(btnRestore);

            flow.Controls.Add(row);
            flow.Controls.Add(CreateLabeledField("امتداد الملف", "*.upbak"));

            page.Controls.Add(flow);
            return page;
        }

        private static TabPage BuildSystemPrefsTab()
        {
            var page = CreateTabPage("tabPrefs", "تفضيلات النظام");
            var flow = CreateFlowBody();

            flow.Controls.Add(CreatePlaceholderNote(
                "الطابعة والتفضيلات العامة",
                "إعدادات الطابعة الافتراضية والطباعة التلقائية للفواتير."));
            flow.Controls.Add(CreateLabeledField("اسم الطابعة الافتراضية", ""));
            flow.Controls.Add(CreateCheckRow("طباعة الفاتورة تلقائياً بعد البيع", true));
            flow.Controls.Add(CreateLabeledField("نوع الموديول", "General"));

            page.Controls.Add(flow);
            return page;
        }

        private async Task CreateEncryptedBackupAsync()
        {
            using var dialog = new SaveFileDialog
            {
                Title = "حفظ النسخة الاحتياطية المشفرة",
                Filter = "نسخة احتياطية مشفرة (*.upbak)|*.upbak",
                DefaultExt = "upbak",
                AddExtension = true,
                FileName = $"UrPOS_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.upbak",
                OverwritePrompt = true
            };

            if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
            {
                return;
            }

            if (!TryPromptPassword("كلمة مرور التشفير", "أدخل كلمة مرور لتشفير النسخة الاحتياطية:", out var encryptionKey)
                || string.IsNullOrWhiteSpace(encryptionKey))
            {
                MessageBox.Show(
                    "يجب إدخال كلمة مرور للتشفير.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }

            UseWaitCursor = true;
            Enabled = false;
            try
            {
                var result = await _backupService.CreateEncryptedBackupAsync(dialog.FileName, encryptionKey);
                if (result.isSuccess)
                {
                    MessageBox.Show(
                        "تم إنشاء النسخة الاحتياطية المشفرة بنجاح.",
                        "نجاح",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                }
                else
                {
                    MessageBox.Show(
                        string.IsNullOrWhiteSpace(result.ErrorMessage)
                            ? "فشل إنشاء النسخة الاحتياطية."
                            : result.ErrorMessage,
                        "خطأ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                }
            }
            finally
            {
                Enabled = true;
                UseWaitCursor = false;
            }
        }

        private async Task RestoreEncryptedBackupAsync()
        {
            using var dialog = new OpenFileDialog
            {
                Title = "اختيار ملف النسخة الاحتياطية",
                Filter = "نسخة احتياطية مشفرة (*.upbak)|*.upbak",
                DefaultExt = "upbak",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
            {
                return;
            }

            if (!TryPromptPassword("كلمة مرور فك التشفير", "أدخل كلمة المرور المستخدمة عند إنشاء النسخة الاحتياطية:", out var encryptionKey)
                || string.IsNullOrWhiteSpace(encryptionKey))
            {
                MessageBox.Show(
                    "يجب إدخال كلمة مرور فك التشفير.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }

            var confirm = MessageBox.Show(
                "تحذير: سيتم استبدال البيانات الحالية بالكامل، هل تريد الاستمرار؟",
                "تأكيد الاستعادة",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            UseWaitCursor = true;
            Enabled = false;
            try
            {
                var result = await _backupService.RestoreEncryptedBackupAsync(dialog.FileName, encryptionKey);
                if (result.isSuccess)
                {
                    MessageBox.Show(
                        "تمت استعادة النسخة الاحتياطية بنجاح. يُفضل إعادة تشغيل التطبيق.",
                        "نجاح",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                }
                else
                {
                    MessageBox.Show(
                        string.IsNullOrWhiteSpace(result.ErrorMessage)
                            ? "فشلت الاستعادة."
                            : result.ErrorMessage,
                        "خطأ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                }
            }
            finally
            {
                Enabled = true;
                UseWaitCursor = false;
            }
        }

        private bool TryPromptPassword(string title, string prompt, out string password)
        {
            password = string.Empty;

            using var form = new Form
            {
                Text = title,
                RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
                ClientSize = new Size(420, 160),
                Font = new Font("Segoe UI", 10F)
            };

            var lbl = new Label
            {
                Text = prompt,
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(12, 12, 12, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            var txt = new TextBox
            {
                PasswordChar = '●',
                Dock = DockStyle.Top,
                Margin = new Padding(12),
                Height = 28
            };
            var txtHost = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(16, 8, 16, 8)
            };
            txtHost.Controls.Add(txt);
            txt.Dock = DockStyle.Fill;

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.LeftToRight,
                Height = 48,
                Padding = new Padding(12, 8, 12, 8),
                RightToLeft = RightToLeft.Yes
            };

            var btnOk = new Button
            {
                Text = "موافق",
                DialogResult = DialogResult.OK,
                Width = 90,
                Height = 32
            };
            var btnCancel = new Button
            {
                Text = "إلغاء",
                DialogResult = DialogResult.Cancel,
                Width = 90,
                Height = 32
            };

            buttons.Controls.Add(btnCancel);
            buttons.Controls.Add(btnOk);

            form.Controls.Add(buttons);
            form.Controls.Add(txtHost);
            form.Controls.Add(lbl);
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            if (form.ShowDialog(FindForm()) != DialogResult.OK)
            {
                return false;
            }

            password = txt.Text;
            return true;
        }

        private static TabPage CreateTabPage(string name, string text)
        {
            return new TabPage
            {
                Name = name,
                Padding = new Padding(12),
                RightToLeft = RightToLeft.Yes,
                Text = text,
                UseVisualStyleBackColor = true
            };
        }

        private static FlowLayoutPanel CreateFlowBody()
        {
            return new FlowLayoutPanel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(8),
                RightToLeft = RightToLeft.Yes,
                WrapContents = false
            };
        }

        private static Panel CreatePlaceholderNote(string title, string body)
        {
            var panel = new Panel
            {
                BackColor = Color.FromArgb(240, 253, 250),
                Height = 88,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(16),
                RightToLeft = RightToLeft.Yes,
                Width = 680
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Height = 28,
                Text = title,
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblBody = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = body,
                TextAlign = ContentAlignment.TopRight
            };

            panel.Controls.Add(lblBody);
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private static Panel CreateLabeledField(string label, string value)
        {
            var panel = new Panel
            {
                Height = 64,
                Margin = new Padding(0, 0, 0, 8),
                RightToLeft = RightToLeft.Yes,
                Width = 680
            };

            var lbl = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Height = 24,
                Text = label,
                TextAlign = ContentAlignment.MiddleRight
            };

            var txt = new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 11F),
                Height = 28,
                RightToLeft = RightToLeft.Yes,
                Text = value,
                TextAlign = HorizontalAlignment.Right
            };

            panel.Controls.Add(txt);
            panel.Controls.Add(lbl);
            return panel;
        }

        private static CheckBox CreateCheckRow(string text, bool isChecked)
        {
            return new CheckBox
            {
                Checked = isChecked,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(51, 65, 85),
                Height = 32,
                Margin = new Padding(0, 4, 0, 12),
                RightToLeft = RightToLeft.Yes,
                Text = text,
                TextAlign = ContentAlignment.MiddleRight,
                Width = 680
            };
        }

        private static Button CreateActionButton(string text, Color backColor)
        {
            var btn = new Button
            {
                AutoSize = true,
                BackColor = backColor,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Margin = new Padding(0, 8, 8, 8),
                MinimumSize = new Size(180, 40),
                Padding = new Padding(16, 0, 16, 0),
                Text = text,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }
    }
}
