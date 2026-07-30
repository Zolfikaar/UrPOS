using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.WinForms.Forms;
using static UrPOS.WinForms.Forms.ProductForm;

namespace UrPOS.WinForms.Controls
{
    /// <summary>
    /// Products management page (Arabic RTL). Product data via <see cref="IProductRepository"/>;
    /// stock adjustments are recorded through <see cref="IInventoryRepository"/> inside ProductForm.
    /// </summary>
    public class ProductsControl : UserControl
    {
        public const int DefaultPageSize = 15;

        /// <summary>UI row model mapped from the Core <see cref="Product"/> entity.</summary>
        public sealed class ProductListItem
        {
            public int Id { get; set; }
            public string Barcode { get; set; } = string.Empty;
            public string ProductName { get; set; } = string.Empty;
            public decimal CostPrice { get; set; }
            public decimal SalePrice { get; set; }
            public int CurrentStock { get; set; }
            public int MinStockLevel { get; set; }
            public bool IsActive { get; set; } = true;

            public string StatusText => IsActive ? "نشط" : "معطّل";
            public string ToggleActionText => IsActive ? "تعطيل" : "تفعيل";

            public static ProductListItem FromProduct(Product product) => new()
            {
                Id = product.Id,
                Barcode = product.Barcode,
                ProductName = product.ProductName,
                CostPrice = product.CostPrice,
                SalePrice = product.SalePrice,
                CurrentStock = product.CurrentStock,
                MinStockLevel = product.MinStockLevel,
                IsActive = true
            };

            public void Apply(Product product)
            {
                Id = product.Id;
                Barcode = product.Barcode;
                ProductName = product.ProductName;
                CostPrice = product.CostPrice;
                SalePrice = product.SalePrice;
                CurrentStock = product.CurrentStock;
                MinStockLevel = product.MinStockLevel;
            }

            public Product ToProduct() => new()
            {
                Id = Id,
                Barcode = Barcode,
                ProductName = ProductName,
                CostPrice = CostPrice,
                SalePrice = SalePrice,
                CurrentStock = CurrentStock,
                MinStockLevel = MinStockLevel
            };
        }

        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly List<ProductListItem> _allProducts = new();
        private readonly BindingList<ProductListItem> _pageItems = new();
        private readonly BindingSource _bindingSource = new();

        private TextBox _txtSearch = null!;
        private DataGridView _dgvProducts = null!;
        private Label _lblCount = null!;
        private Label _lblPageInfo = null!;
        private Button _btnPrevPage = null!;
        private Button _btnNextPage = null!;

        private string _currentQuery = string.Empty;
        private int _currentPage = 1;
        private int _pageSize = DefaultPageSize;
        private bool _isLoading;

        public ProductsControl(
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));

            // None: this control is swapped in/out of MainForm. Font/Dpi autoscaling
            // on re-parent used to compound and crush the whole shell layout.
            AutoScaleMode = AutoScaleMode.None;
            RightToLeft = RightToLeft.Yes;
            BackColor = Color.FromArgb(241, 245, 249);
            Dock = DockStyle.Fill;
            Padding = new Padding(0);

            BuildLayout();
            BindGrid();
            Load += ProductsControl_Load;
        }

        private async void ProductsControl_Load(object? sender, EventArgs e)
        {
            await ReloadProductsAsync();
        }

        /// <summary>Reload all products from the repository.</summary>
        public async Task ReloadProductsAsync()
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            try
            {
                var products = await _productRepository.GetAllAsync();
                _allProducts.Clear();
                _allProducts.AddRange(products.Select(ProductListItem.FromProduct));
                _currentPage = 1;
                RefreshPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FindForm(),
                    $"تعذر تحميل المنتجات:\n{ex.Message}",
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>Open the add-product modal (also used by dashboard quick action).</summary>
        public void OpenAddProductForm()
        {
            OpenProductForm(productToEdit: null);
        }

        private void OpenProductForm(Product? productToEdit)
        {
            using var form = productToEdit is null
                ? new ProductForm(_productRepository, _inventoryRepository)
                : new ProductForm(_productRepository, _inventoryRepository, productToEdit);

            form.ProductSaved += OnProductSaved;
            try
            {
                form.ShowDialog(FindForm());
            }
            finally
            {
                form.ProductSaved -= OnProductSaved;
            }
        }

        private void OnProductSaved(object? sender, ProductSavedEventArgs e)
        {
            if (e.IsEdit)
            {
                UpdateRowInGrid(e.Product);
            }
            else
            {
                AddRowToGrid(e.Product);
            }
        }

        private void UpdateRowInGrid(Product product)
        {
            var existing = _allProducts.FirstOrDefault(p => p.Id == product.Id);
            if (existing is null)
            {
                AddRowToGrid(product);
                return;
            }

            existing.Apply(product);
            _bindingSource.ResetBindings(false);
            RefreshPage();
        }

        private void AddRowToGrid(Product product)
        {
            _allProducts.Insert(0, ProductListItem.FromProduct(product));
            _currentQuery = string.Empty;
            _txtSearch.Clear();
            _currentPage = 1;
            RefreshPage();
        }

        private void BuildLayout()
        {
            var lblTitle = CreateFixedTopLabel(
                "lblProductsTitle",
                "إدارة المنتجات",
                new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point),
                Color.FromArgb(15, 23, 42),
                height: 44);

            var lblHint = CreateFixedTopLabel(
                "lblProductsHint",
                "ابحث بالاسم أو الباركود، أو أضف منتجاً جديداً.",
                new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                Color.FromArgb(100, 116, 139),
                height: 28);

            var pnlTopBar = new TableLayoutPanel
            {
                ColumnCount = 3,
                Dock = DockStyle.Top,
                Height = 48,
                MaximumSize = new Size(0, 48),
                MinimumSize = new Size(0, 48),
                Name = "pnlProductsTopBar",
                Padding = new Padding(0, 4, 0, 4),
                RightToLeft = RightToLeft.Yes,
                RowCount = 1
            };
            pnlTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 176F));
            pnlTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            pnlTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlTopBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var btnAdd = CreateAccentButton("btnAddProduct", "إضافة منتج جديد", Color.FromArgb(13, 148, 136), 168, 36);
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.Margin = new Padding(0, 2, 8, 2);
            btnAdd.Click += (_, _) => OpenAddProductForm();

            var btnSearch = CreateAccentButton("btnSearchProducts", "بحث", Color.FromArgb(51, 65, 85), 88, 36);
            btnSearch.Dock = DockStyle.Fill;
            btnSearch.Margin = new Padding(0, 2, 8, 2);
            btnSearch.Click += (_, _) => RaiseSearch();

            _txtSearch = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                Margin = new Padding(0, 2, 0, 2),
                Name = "txtSearchProducts",
                PlaceholderText = "بحث بالاسم أو الباركود...",
                RightToLeft = RightToLeft.Yes,
                TextAlign = HorizontalAlignment.Right
            };
            _txtSearch.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    RaiseSearch();
                }
            };

            pnlTopBar.Controls.Add(btnAdd, 0, 0);
            pnlTopBar.Controls.Add(btnSearch, 1, 0);
            pnlTopBar.Controls.Add(_txtSearch, 2, 0);

            var pnlPager = BuildPagerBar();

            _lblCount = new Label
            {
                Dock = DockStyle.Bottom,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(100, 116, 139),
                Height = 26,
                MaximumSize = new Size(0, 26),
                MinimumSize = new Size(0, 26),
                Name = "lblProductsCount",
                RightToLeft = RightToLeft.Yes,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var pnlGridCard = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Name = "pnlProductsGridCard",
                Padding = new Padding(1),
                RightToLeft = RightToLeft.Yes
            };

            _dgvProducts = BuildProductsGrid();
            _dgvProducts.CellContentClick += DgvProducts_CellContentClick;
            _dgvProducts.CellFormatting += DgvProducts_CellFormatting;
            pnlGridCard.Controls.Add(_dgvProducts);

            var spTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 10,
                MaximumSize = new Size(0, 10),
                MinimumSize = new Size(0, 10),
                Name = "spProductsTop"
            };

            Controls.Add(pnlGridCard);
            Controls.Add(_lblCount);
            Controls.Add(pnlPager);
            Controls.Add(spTop);
            Controls.Add(pnlTopBar);
            Controls.Add(lblHint);
            Controls.Add(lblTitle);
        }

        private Panel BuildPagerBar()
        {
            var pnlPager = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                MaximumSize = new Size(0, 44),
                MinimumSize = new Size(0, 44),
                Name = "pnlProductsPager",
                Padding = new Padding(0, 6, 0, 0),
                RightToLeft = RightToLeft.Yes
            };

            _btnNextPage = CreateAccentButton("btnNextPage", "التالي ←", Color.FromArgb(51, 65, 85), 110, 32);
            _btnNextPage.Dock = DockStyle.Left;
            _btnNextPage.Click += (_, _) =>
            {
                if (_currentPage < GetTotalPages(GetFilteredProducts().Count))
                {
                    _currentPage++;
                    RefreshPage();
                }
            };

            _btnPrevPage = CreateAccentButton("btnPrevPage", "→ السابق", Color.FromArgb(51, 65, 85), 110, 32);
            _btnPrevPage.Dock = DockStyle.Left;
            _btnPrevPage.Click += (_, _) =>
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    RefreshPage();
                }
            };

            _lblPageInfo = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(51, 65, 85),
                Name = "lblPageInfo",
                RightToLeft = RightToLeft.Yes,
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlPager.Controls.Add(_lblPageInfo);
            pnlPager.Controls.Add(_btnPrevPage);
            pnlPager.Controls.Add(_btnNextPage);
            return pnlPager;
        }

        private static Label CreateFixedTopLabel(string name, string text, Font font, Color foreColor, int height)
        {
            return new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Font = font,
                ForeColor = foreColor,
                Height = height,
                MaximumSize = new Size(0, height),
                MinimumSize = new Size(0, height),
                Name = name,
                RightToLeft = RightToLeft.Yes,
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private DataGridView BuildProductsGrid()
        {
            var headerStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(15, 23, 42),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(15, 23, 42),
                SelectionForeColor = Color.White
            };
            var cellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(30, 41, 59),
                SelectionBackColor = Color.FromArgb(204, 251, 241),
                SelectionForeColor = Color.FromArgb(15, 23, 42),
                WrapMode = DataGridViewTriState.False
            };

            var grid = new DataGridView
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
                ColumnHeadersDefaultCellStyle = headerStyle,
                ColumnHeadersHeight = 42,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                DefaultCellStyle = cellStyle,
                Dock = DockStyle.Fill,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(226, 232, 240),
                MultiSelect = false,
                Name = "dgvProducts",
                ReadOnly = true,
                RightToLeft = RightToLeft.Yes,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            grid.RowTemplate.Height = 40;

            grid.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(ProductListItem.Barcode),
                    FillWeight = 14,
                    HeaderText = "البار كود",
                    MinimumWidth = 90,
                    Name = "colBarcode"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(ProductListItem.ProductName),
                    FillWeight = 22,
                    HeaderText = "اسم المنتج",
                    MinimumWidth = 120,
                    Name = "colProductName"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(ProductListItem.CostPrice),
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N2" },
                    FillWeight = 12,
                    HeaderText = "سعر الشراء",
                    MinimumWidth = 80,
                    Name = "colCostPrice"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(ProductListItem.SalePrice),
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N2" },
                    FillWeight = 12,
                    HeaderText = "سعر البيع",
                    MinimumWidth = 80,
                    Name = "colSalePrice"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(ProductListItem.CurrentStock),
                    FillWeight = 12,
                    HeaderText = "الكمية بالمخزون",
                    MinimumWidth = 80,
                    Name = "colStock"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(ProductListItem.MinStockLevel),
                    FillWeight = 12,
                    HeaderText = "تنبيه أصل المخزون",
                    MinimumWidth = 90,
                    Name = "colMinStock"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(ProductListItem.StatusText),
                    FillWeight = 8,
                    HeaderText = "الحالة",
                    MinimumWidth = 70,
                    Name = "colStatus"
                },
                new DataGridViewButtonColumn
                {
                    FlatStyle = FlatStyle.Flat,
                    FillWeight = 8,
                    HeaderText = "تعديل",
                    MinimumWidth = 70,
                    Name = "colEdit",
                    Text = "تعديل",
                    UseColumnTextForButtonValue = true
                },
                new DataGridViewButtonColumn
                {
                    DataPropertyName = nameof(ProductListItem.ToggleActionText),
                    FlatStyle = FlatStyle.Flat,
                    FillWeight = 8,
                    HeaderText = "تعطيل/تفعيل",
                    MinimumWidth = 90,
                    Name = "colToggle",
                    UseColumnTextForButtonValue = false
                });

            return grid;
        }

        private void BindGrid()
        {
            _bindingSource.DataSource = _pageItems;
            _dgvProducts.DataSource = _bindingSource;
        }

        private void RaiseSearch()
        {
            _currentQuery = _txtSearch.Text.Trim();
            _currentPage = 1;
            RefreshPage();
        }

        private List<ProductListItem> GetFilteredProducts()
        {
            if (string.IsNullOrWhiteSpace(_currentQuery))
            {
                return _allProducts.ToList();
            }

            return _allProducts
                .Where(p =>
                    p.ProductName.Contains(_currentQuery, StringComparison.OrdinalIgnoreCase) ||
                    p.Barcode.Contains(_currentQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private static int GetTotalPages(int totalItems)
        {
            if (totalItems <= 0)
            {
                return 1;
            }

            return (int)Math.Ceiling(totalItems / (double)DefaultPageSize);
        }

        private void RefreshPage()
        {
            var filtered = GetFilteredProducts();
            var totalPages = GetTotalPages(filtered.Count);
            if (_currentPage > totalPages)
            {
                _currentPage = totalPages;
            }

            if (_currentPage < 1)
            {
                _currentPage = 1;
            }

            _pageSize = DefaultPageSize;
            var pageSlice = filtered
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            _pageItems.Clear();
            foreach (var item in pageSlice)
            {
                _pageItems.Add(item);
            }

            var active = filtered.Count(p => p.IsActive);
            _lblCount.Text =
                $"الإجمالي: {filtered.Count}  |  النشطة: {active}  |  حجم الصفحة: {_pageSize}";

            _lblPageInfo.Text = $"صفحة {_currentPage} من {totalPages}";
            _btnPrevPage.Enabled = _currentPage > 1;
            _btnNextPage.Enabled = _currentPage < totalPages;
        }

        private void DgvProducts_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (_dgvProducts.Rows[e.RowIndex].DataBoundItem is not ProductListItem item)
            {
                return;
            }

            if (item.CurrentStock > item.MinStockLevel)
            {
                return;
            }

            // Soft red for low-stock rows (stock at or below warning level).
            e.CellStyle.BackColor = Color.FromArgb(254, 226, 226);
            e.CellStyle.ForeColor = Color.FromArgb(153, 27, 27);
            e.CellStyle.SelectionBackColor = Color.FromArgb(252, 165, 165);
            e.CellStyle.SelectionForeColor = Color.FromArgb(127, 29, 29);
        }

        private void DgvProducts_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (_dgvProducts.Rows[e.RowIndex].DataBoundItem is not ProductListItem item)
            {
                return;
            }

            var colName = _dgvProducts.Columns[e.ColumnIndex].Name;
            if (colName == "colEdit")
            {
                OpenProductForm(item.ToProduct());
            }
            else if (colName == "colToggle")
            {
                // Local UI toggle only — Product entity has no IsActive persistence yet.
                item.IsActive = !item.IsActive;
                _dgvProducts.InvalidateRow(e.RowIndex);
                RefreshPage();
            }
        }

        private static Button CreateAccentButton(string name, string text, Color backColor, int width, int height)
        {
            var btn = new Button
            {
                AutoSize = false,
                BackColor = backColor,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                MaximumSize = new Size(width, height),
                MinimumSize = new Size(width, height),
                Name = name,
                Size = new Size(width, height),
                Text = text,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }
    }
}
