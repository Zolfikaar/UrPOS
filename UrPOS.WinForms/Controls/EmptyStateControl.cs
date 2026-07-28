using System.Drawing;
using System.Windows.Forms;

namespace UrPOS.WinForms.Controls
{
    /// <summary>
    /// Arabic empty-state / placeholder panel for inactive menu sections.
    /// </summary>
    public class EmptyStateControl : UserControl
    {
        private readonly Label _lblIcon;
        private readonly Label _lblTitle;
        private readonly Label _lblMessage;

        public EmptyStateControl()
            : this("هذه الصفحة قيد التجهيز", "لا توجد بيانات للعرض حالياً.")
        {
        }

        public EmptyStateControl(string title, string message)
        {
            RightToLeft = RightToLeft.Yes;
            BackColor = Color.FromArgb(241, 245, 249);
            Dock = DockStyle.Fill;
            Padding = new Padding(40);

            var card = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(48, 64, 48, 48)
            };

            _lblIcon = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Symbol", 42F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(148, 163, 184),
                Height = 72,
                Text = "◇",
                TextAlign = ContentAlignment.MiddleCenter
            };

            _lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(15, 23, 42),
                Height = 48,
                Text = title,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblMessage = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(100, 116, 139),
                Height = 64,
                Text = message,
                TextAlign = ContentAlignment.TopLeft
            };

            // Top dock: add bottom-first
            card.Controls.Add(_lblMessage);
            card.Controls.Add(_lblTitle);
            card.Controls.Add(_lblIcon);
            Controls.Add(card);
        }

        public void SetContent(string title, string message)
        {
            _lblTitle.Text = title;
            _lblMessage.Text = message;
        }
    }
}
