-- 1. جدول الأدوار
CREATE TABLE IF NOT EXISTS roles (
    id SERIAL PRIMARY KEY,
    role_name VARCHAR(50) UNIQUE NOT NULL
);

-- زراعة الأدوار الافتراضية
INSERT INTO roles (id, role_name) VALUES (1, 'Admin'), (2, 'Cashier') ON CONFLICT DO NOTHING;

-- 2. جدول المستخدمين
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    full_name VARCHAR(100) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 3. جدول ربط أدوار المستخدمين
CREATE TABLE IF NOT EXISTS user_roles (
    user_id INT REFERENCES users(id) ON DELETE CASCADE,
    role_id INT REFERENCES roles(id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, role_id)
);

-- 4. جدول الموردين
CREATE TABLE IF NOT EXISTS suppliers (
    id SERIAL PRIMARY KEY,
    supplier_name VARCHAR(150) NOT NULL,
    phone VARCHAR(20),
    balance NUMERIC(12, 3) DEFAULT 0.000
);

-- 5. جدول المواد الأساسية
CREATE TABLE IF NOT EXISTS products (
    id SERIAL PRIMARY KEY,
    barcode VARCHAR(50) UNIQUE NOT NULL,
    product_name VARCHAR(150) NOT NULL,
    cost_price NUMERIC(12, 3) NOT NULL DEFAULT 0.000,
    sale_price NUMERIC(12, 3) NOT NULL DEFAULT 0.000,
    min_stock_level INT DEFAULT 0,
    current_stock INT DEFAULT 0,
    custom_attributes JSONB,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- إنشار Index على الباركود واسم المنتج لسرعة الاستعلام الخارقة في شاشات الكاشير
CREATE INDEX IF NOT EXISTS idx_products_barcode ON products(barcode);
CREATE INDEX IF NOT EXISTS idx_products_name ON products(product_name);

-- 6. جدول حركات المخزن
CREATE TABLE IF NOT EXISTS inventory_movements (
    id BIGSERIAL PRIMARY KEY,
    product_id INT REFERENCES products(id),
    movement_type VARCHAR(20) NOT NULL,
    quantity INT NOT NULL,
    reference_id BIGINT,
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 7. جدول فواتير المبيعات ورؤوسها
CREATE TABLE IF NOT EXISTS sales_invoices (
    id BIGSERIAL PRIMARY KEY,
    invoice_number VARCHAR(50) UNIQUE NOT NULL,
    user_id INT REFERENCES users(id),
    total_amount NUMERIC(12, 3) NOT NULL DEFAULT 0.000,
    discount NUMERIC(12, 3) DEFAULT 0.000,
    net_amount NUMERIC(12, 3) NOT NULL DEFAULT 0.000,
    payment_type VARCHAR(20) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS sales_invoice_items (
    id BIGSERIAL PRIMARY KEY,
    invoice_id BIGINT REFERENCES sales_invoices(id) ON DELETE CASCADE,
    product_id INT REFERENCES products(id),
    quantity INT NOT NULL,
    unit_price NUMERIC(12, 3) NOT NULL,
    total_price NUMERIC(12, 3) NOT NULL
);

-- 8. جدول فواتير المشتريات (المندوبين)
CREATE TABLE IF NOT EXISTS purchase_invoices (
    id BIGSERIAL PRIMARY KEY,
    invoice_number VARCHAR(50) NOT NULL,
    supplier_id INT REFERENCES suppliers(id),
    user_id INT REFERENCES users(id),
    total_amount NUMERIC(12, 3) NOT NULL DEFAULT 0.000,
    tax NUMERIC(12, 3) DEFAULT 0.000,
    net_amount NUMERIC(12, 3) NOT NULL DEFAULT 0.000,
    payment_type VARCHAR(20) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS purchase_invoice_items (
    id BIGSERIAL PRIMARY KEY,
    purchase_invoice_id BIGINT REFERENCES purchase_invoices(id) ON DELETE CASCADE,
    product_id INT REFERENCES products(id),
    quantity INT NOT NULL,
    cost_price NUMERIC(12, 3) NOT NULL,
    total_price NUMERIC(12, 3) NOT NULL
);

ALTER TABLE users ADD COLUMN IF NOT EXISTS email VARCHAR(150);
ALTER TABLE users ADD COLUMN IF NOT EXISTS phone VARCHAR(20);