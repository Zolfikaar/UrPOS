using System.Drawing;
using System.Windows.Forms;

namespace UrPOS.WinForms.Controls
{
    /// <summary>
    /// Settings page UI with Arabic RTL tabs (visual placeholders — no backend wiring).
    /// </summary>
    public class SettingsControl : UserControl
    {
        private readonly TabControl _tabs;

        public SettingsControl()
        {
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
                Text = "واجهة إعدادات مركزية — الحقول أدناه جاهزة بصرياً للتوصيل لاحقاً.",
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

        private static TabPage BuildBackupSecurityTab()
        {
            var page = CreateTabPage("tabBackup", "النسخ الاحتياطي والأمان");
            var flow = CreateFlowBody();

            flow.Controls.Add(CreatePlaceholderNote(
                "نسخ احتياطي واستعادة قاعدة البيانات",
                "أزرار واجهة فقط — منطق النسخ الاحتياطي سيُربط لاحقاً بـ PostgreSQL."));

            var row = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Margin = new Padding(0, 8, 0, 8),
                RightToLeft = RightToLeft.Yes,
                WrapContents = false,
                Width = 640
            };
            row.Controls.Add(CreateActionButton("إنشاء نسخة احتياطية", Color.FromArgb(13, 148, 136)));
            row.Controls.Add(CreateActionButton("استعادة من ملف", Color.FromArgb(51, 65, 85)));

            flow.Controls.Add(row);
            flow.Controls.Add(CreateLabeledField("مسار النسخ الاحتياطي", @"C:\UrPOS\Backups"));

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
