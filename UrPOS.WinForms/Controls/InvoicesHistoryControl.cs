using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.WinForms.Forms;

namespace UrPOS.WinForms.Controls
{
    /// <summary>
    /// Invoice history: completed sales from PostgreSQL + parked drafts from session.
    /// </summary>
    public class InvoicesHistoryControl : UserControl
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly BindingList<HistoryRow> _rows = new();
        private readonly BindingList<SalesInvoiceItem> _detailItems = new();

        private TextBox _txtInvoiceNumber = null!;
        private DateTimePicker _dtFrom = null!;
        private DateTimePicker _dtTo = null!;
        private ComboBox _cboStatus = null!;
        private DataGridView _dgvInvoices = null!;
        private DataGridView _dgvItems = null!;
        private Button _btnSearch = null!;
        private Button _btnResume = null!;
        private Label _lblDetailsTitle = null!;
        private bool _isBusy;

        public event EventHandler? ProductsMayHaveChanged;

        private sealed class HistoryRow
        {
            public long Id { get; set; }
            public string InvoiceNumber { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
            public string CashierName { get; set; } = string.Empty;
            public decimal Subtotal { get; set; }
            public decimal Tax { get; set; }
            public decimal Discount { get; set; }
            public decimal FinalAmount { get; set; }
            public string Status { get; set; } = InvoiceStatus.Completed;
            public string StatusArabic => InvoiceStatus.ToArabic(Status);
            public int ParkedDraftIndex { get; set; } = -1;
            public bool IsParked => Status == InvoiceStatus.Parked;
        }

        public InvoicesHistoryControl(IInvoiceRepository invoiceRepository, IServiceProvider serviceProvider)
        {
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            AutoScaleMode = AutoScaleMode.None;
            RightToLeft = RightToLeft.Yes;
            BackColor = Color.FromArgb(241, 245, 249);
            Dock = DockStyle.Fill;
            Padding = new Padding(0);

            BuildUi();
            HandleCreated += async (_, _) => await ReloadAsync();
        }

        public Task ReloadAsync() => LoadHistoryAsync();

        private void BuildUi()
        {
            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Height = 44,
                Padding = new Padding(4, 0, 4, 0),
                Text = "سجل الفواتير",
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblHint = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Height = 28,
                Text = "عرض فواتير المبيعات المكتملة والمسودات المعلقة، مع استئناف الفواتير المعلقة إلى شاشة الكاشير.",
                TextAlign = ContentAlignment.MiddleLeft
            };

            var filters = BuildFiltersPanel();
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterWidth = 8,
                Panel1MinSize = 180,
                Panel2MinSize = 140
            };

            _dgvInvoices = CreateGrid();
            BindInvoiceColumns();
            _dgvInvoices.DataSource = _rows;
            _dgvInvoices.SelectionChanged += async (_, _) => await OnInvoiceSelectionChangedAsync();

            var topHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0) };
            topHost.Controls.Add(_dgvInvoices);

            _lblDetailsTitle = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Height = 36,
                Text = "تفاصيل أصناف الفاتورة",
                TextAlign = ContentAlignment.MiddleLeft
            };

            var actions = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                Padding = new Padding(0, 8, 0, 0)
            };

            _btnResume = new Button
            {
                BackColor = Color.FromArgb(13, 148, 136),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right,
                Enabled = false,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(200, 44),
                Text = "استئناف الفاتورة المعلقة",
                UseVisualStyleBackColor = false
            };
            _btnResume.FlatAppearance.BorderSize = 0;
            _btnResume.Click += (_, _) => ResumeSelectedParkedInvoice();
            actions.Controls.Add(_btnResume);

            _dgvItems = CreateGrid();
            BindItemColumns();
            _dgvItems.DataSource = _detailItems;

            var bottomHost = new Panel { Dock = DockStyle.Fill };
            bottomHost.Controls.Add(_dgvItems);
            bottomHost.Controls.Add(actions);
            bottomHost.Controls.Add(_lblDetailsTitle);

            split.Panel1.Controls.Add(topHost);
            split.Panel2.Controls.Add(bottomHost);

            Controls.Add(split);
            Controls.Add(filters);
            Controls.Add(lblHint);
            Controls.Add(lblTitle);

            // Defer splitter distance until laid out
            Load += (_, _) =>
            {
                try
                {
                    if (split.Height > 400)
                    {
                        split.SplitterDistance = Math.Max(220, (int)(split.Height * 0.55));
                    }
                }
                catch
                {
                    // ignore layout edge cases
                }
            };
        }

        private Panel BuildFiltersPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 72,
                Padding = new Padding(0, 8, 0, 8)
            };

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                RightToLeft = RightToLeft.Yes
            };

            _txtInvoiceNumber = new TextBox
            {
                Font = new Font("Segoe UI", 11F),
                PlaceholderText = "رقم الفاتورة...",
                RightToLeft = RightToLeft.Yes,
                Size = new Size(180, 32),
                Margin = new Padding(8, 4, 0, 4)
            };

            _dtFrom = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short,
                Size = new Size(120, 32),
                Margin = new Padding(8, 4, 0, 4),
                Value = DateTime.Today.AddDays(-30)
            };

            _dtTo = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short,
                Size = new Size(120, 32),
                Margin = new Padding(8, 4, 0, 4),
                Value = DateTime.Today
            };

            _cboStatus = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(140, 32),
                Margin = new Padding(8, 4, 0, 4),
                RightToLeft = RightToLeft.Yes
            };
            _cboStatus.Items.AddRange(["الكل", "مكتملة", "معلقة"]);
            _cboStatus.SelectedIndex = 0;

            _btnSearch = new Button
            {
                BackColor = Color.FromArgb(13, 148, 136),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(110, 36),
                Margin = new Padding(8, 2, 0, 2),
                Text = "بحث",
                UseVisualStyleBackColor = false
            };
            _btnSearch.FlatAppearance.BorderSize = 0;
            _btnSearch.Click += async (_, _) => await LoadHistoryAsync();

            flow.Controls.Add(Labeled("رقم الفاتورة", _txtInvoiceNumber));
            flow.Controls.Add(Labeled("من تاريخ", _dtFrom));
            flow.Controls.Add(Labeled("إلى تاريخ", _dtTo));
            flow.Controls.Add(Labeled("الحالة", _cboStatus));
            flow.Controls.Add(_btnSearch);

            panel.Controls.Add(flow);
            return panel;
        }

        private static Panel Labeled(string caption, Control control)
        {
            var host = new Panel
            {
                AutoSize = true,
                Margin = new Padding(4),
                Height = 56
            };
            var lbl = new Label
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = caption,
                TextAlign = ContentAlignment.MiddleLeft
            };
            control.Dock = DockStyle.Bottom;
            host.Controls.Add(control);
            host.Controls.Add(lbl);
            host.Width = Math.Max(control.Width + 8, 100);
            return host;
        }

        private static DataGridView CreateGrid()
        {
            var styleHeader = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(15, 23, 42),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(15, 23, 42),
                SelectionForeColor = Color.White
            };
            var styleCell = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(30, 41, 59),
                SelectionBackColor = Color.FromArgb(204, 251, 241),
                SelectionForeColor = Color.FromArgb(15, 23, 42)
            };

            return new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                ColumnHeadersDefaultCellStyle = styleHeader,
                ColumnHeadersHeight = 40,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                DefaultCellStyle = styleCell,
                Dock = DockStyle.Fill,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(226, 232, 240),
                MultiSelect = false,
                ReadOnly = true,
                RightToLeft = RightToLeft.Yes,
                RowHeadersVisible = false,
                RowTemplate = { Height = 36 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
        }

        private void BindInvoiceColumns()
        {
            var money = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N2" };
            _dgvInvoices.Columns.AddRange(
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(HistoryRow.InvoiceNumber), HeaderText = "رقم الفاتورة", FillWeight = 18, Name = "colInv" },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(HistoryRow.CreatedAt), HeaderText = "التاريخ", FillWeight = 14, Name = "colDate", DefaultCellStyle = new DataGridViewCellStyle { Format = "g" } },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(HistoryRow.CashierName), HeaderText = "الكاشير / المستخدم", FillWeight = 16, Name = "colCashier" },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(HistoryRow.Subtotal), HeaderText = "المجموع الفرعي", FillWeight = 12, Name = "colSub", DefaultCellStyle = money },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(HistoryRow.Tax), HeaderText = "الضريبة", FillWeight = 10, Name = "colTax", DefaultCellStyle = money },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(HistoryRow.Discount), HeaderText = "الخصم", FillWeight = 10, Name = "colDisc", DefaultCellStyle = money },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(HistoryRow.FinalAmount), HeaderText = "المبلغ النهائي", FillWeight = 12, Name = "colFinal", DefaultCellStyle = money },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(HistoryRow.StatusArabic), HeaderText = "الحالة", FillWeight = 10, Name = "colStatus" });
        }

        private void BindItemColumns()
        {
            var money = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N2" };
            _dgvItems.Columns.AddRange(
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(SalesInvoiceItem.ProductName), HeaderText = "اسم المنتج", FillWeight = 40, Name = "colItemName" },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(SalesInvoiceItem.Barcode), HeaderText = "الباركود", FillWeight = 18, Name = "colItemBarcode" },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(SalesInvoiceItem.Quantity), HeaderText = "الكمية", FillWeight = 12, Name = "colItemQty" },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(SalesInvoiceItem.SalePrice), HeaderText = "سعر الوحدة", FillWeight = 15, Name = "colItemPrice", DefaultCellStyle = money },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(SalesInvoiceItem.TotalPrice), HeaderText = "الإجمالي", FillWeight = 15, Name = "colItemTotal", DefaultCellStyle = money });
        }

        private async Task LoadHistoryAsync()
        {
            if (_isBusy)
            {
                return;
            }

            try
            {
                _isBusy = true;
                UseWaitCursor = true;

                var invoiceFilter = _txtInvoiceNumber.Text.Trim();
                var statusFilter = _cboStatus.SelectedIndex; // 0 all, 1 completed, 2 parked
                var from = _dtFrom.Value.Date;
                var to = _dtTo.Value.Date;

                var completed = statusFilter == 2
                    ? Enumerable.Empty<SalesInvoiceListItem>()
                    : await _invoiceRepository.SearchSalesInvoicesAsync(
                        string.IsNullOrWhiteSpace(invoiceFilter) ? null : invoiceFilter,
                        from,
                        to);

                var rows = new List<HistoryRow>();

                if (statusFilter != 2)
                {
                    foreach (var item in completed)
                    {
                        rows.Add(ToRow(item));
                    }
                }

                if (statusFilter != 1)
                {
                    var drafts = ParkedInvoiceSession.Instance.Drafts;
                    for (var i = 0; i < drafts.Count; i++)
                    {
                        var draft = drafts[i];
                        var subtotal = draft.Items.Sum(x => x.TotalPrice);
                        var number = $"PARK-{i + 1}";

                        if (!string.IsNullOrWhiteSpace(invoiceFilter) &&
                            !number.Contains(invoiceFilter, StringComparison.OrdinalIgnoreCase) &&
                            !draft.Title.Contains(invoiceFilter, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        if (draft.ParkedAt.Date < from || draft.ParkedAt.Date > to)
                        {
                            continue;
                        }

                        rows.Add(new HistoryRow
                        {
                            Id = 0,
                            InvoiceNumber = number,
                            CreatedAt = draft.ParkedAt,
                            CashierName = UserSession.Instance.FullName,
                            Subtotal = subtotal,
                            Tax = 0,
                            Discount = 0,
                            FinalAmount = subtotal,
                            Status = InvoiceStatus.Parked,
                            ParkedDraftIndex = i
                        });
                    }
                }

                _rows.Clear();
                foreach (var row in rows.OrderByDescending(r => r.CreatedAt))
                {
                    _rows.Add(row);
                }

                _detailItems.Clear();
                _btnResume.Enabled = false;
                _lblDetailsTitle.Text = "تفاصيل أصناف الفاتورة";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FindForm(),
                    $"تعذر تحميل سجل الفواتير:\n{ex.Message}",
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            finally
            {
                _isBusy = false;
                UseWaitCursor = false;
            }
        }

        private static HistoryRow ToRow(SalesInvoiceListItem item) => new()
        {
            Id = item.Id,
            InvoiceNumber = item.InvoiceNumber,
            CreatedAt = item.CreatedAt,
            CashierName = item.CashierName,
            Subtotal = item.Subtotal,
            Tax = item.Tax,
            Discount = item.Discount,
            FinalAmount = item.FinalAmount,
            Status = item.Status,
            ParkedDraftIndex = item.ParkedDraftIndex
        };

        private async Task OnInvoiceSelectionChangedAsync()
        {
            _detailItems.Clear();
            _btnResume.Enabled = false;

            if (_dgvInvoices.CurrentRow?.DataBoundItem is not HistoryRow row)
            {
                return;
            }

            _lblDetailsTitle.Text = row.IsParked
                ? $"تفاصيل المسودة: {row.InvoiceNumber}"
                : $"تفاصيل الفاتورة: {row.InvoiceNumber}";

            if (row.IsParked)
            {
                var drafts = ParkedInvoiceSession.Instance.Drafts;
                if (row.ParkedDraftIndex >= 0 && row.ParkedDraftIndex < drafts.Count)
                {
                    foreach (var item in drafts[row.ParkedDraftIndex].Items)
                    {
                        _detailItems.Add(item);
                    }
                }

                _btnResume.Enabled = true;
                return;
            }

            try
            {
                var invoice = await _invoiceRepository.GetSalesInvoiceByIdAsync(row.Id);
                if (invoice?.Items == null)
                {
                    return;
                }

                foreach (var item in invoice.Items)
                {
                    _detailItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FindForm(),
                    $"تعذر تحميل أصناف الفاتورة:\n{ex.Message}",
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
        }

        private void ResumeSelectedParkedInvoice()
        {
            if (_dgvInvoices.CurrentRow?.DataBoundItem is not HistoryRow row || !row.IsParked)
            {
                MessageBox.Show(
                    FindForm(),
                    "اختر فاتورة معلقة لاستئنافها.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }

            var owner = FindForm();
            using var posForm = _serviceProvider.GetRequiredService<PosSalesForm>();
            posForm.ResumeParkedDraft(row.ParkedDraftIndex);
            posForm.ShowDialog(owner);

            ProductsMayHaveChanged?.Invoke(this, EventArgs.Empty);
            _ = LoadHistoryAsync();
        }
    }
}
