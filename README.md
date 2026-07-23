# UrPOS — Point of Sale System

A unified Point of Sale (POS) and Inventory Management System built with Clean Architecture for local, high-performance desktop use.

## Current UI Enhancements & RTL

The WinForms presentation layer is Arabic-first with full right-to-left layout:

| Form | RTL | Layout notes |
|------|-----|--------------|
| **LoginForm** | `RightToLeft = Yes`, `RightToLeftLayout = True` | Modern login card with teal accent; card stays centered on resize |
| **MainForm** | Same | Right-side sidebar navigation + welcome content panel |
| **PosSalesForm** | Same | Cart / search / numpad aligned for Arabic; multi-cart toolbar placeholders |

Labels, text boxes, and action buttons use RTL text alignment so Arabic UI reads naturally from right to left.

## Multi-Cart / Parked Orders (UI Overview)

On the POS screen (`PosSalesForm`), a toolbar above the cart provides **UI placeholders only** (no backend logic yet):

- **`+ فاتورة جديدة`** — reserved for starting an additional open cart / invoice
- **`الفواتير المعلقة (0)`** — reserved for listing parked (held) orders; the count badge is static for now

Existing cart controls (grid, remove item, clear cart, checkout, numpad) are unchanged.

## MainForm Sidebar Navigation

The right sidebar includes:

| Button | Status |
|--------|--------|
| شاشة الكاشير (POS) | Opens the live POS dialog |
| إدارة المنتجات | UI placeholder |
| سجل الفواتير | UI placeholder |
| الإعدادات والأمان | UI placeholder |

## Project Structure

```
UrPOS/
├── UrPOS.Core/              # Entities, interfaces, UserSession, ServiceResult
├── UrPOS.Infrastructure/    # Dapper + PostgreSQL, auth, services, DbInitializer
├── UrPOS.WinForms/          # Presentation (Login, Main, POS forms) + DI host
│   └── Forms/
│       ├── LoginForm.*
│       ├── mainForm.*
│       └── PosSalesForm.*
├── UrPOS.Tests/             # xUnit + Moq
└── README.md
```

## Core Features

- **Unified invoice engine** — sales/purchase invoices with atomic line + inventory persistence
- **Supplier & inventory ledger** — stock movements and supplier balances
- **Security** — BCrypt password hashing and in-memory `UserSession`
- **Zero-setup DB** — auto-creates PostgreSQL DB, schema, and seed user on first boot
- **Arabic RTL UI** — login, shell, and cashier screens

## Tech Stack

- .NET 10.0 (C#) / WinForms
- Dapper + Npgsql (PostgreSQL)
- BCrypt.Net-Next
- Microsoft.Extensions.Hosting (DI)
- xUnit + Moq

## How to Run

1. Install **.NET 10 SDK** and a local **PostgreSQL** instance.
2. Configure DB settings in `UrPOS.WinForms/appsettings.json` (`DbHost`, `DbPort`, `DbName`, `DbUsername`, `DbPassword`).
3. From the repo root:

```bash
dotnet restore
dotnet build
dotnet run --project UrPOS.WinForms
```

4. Sign in with the seeded account (created on first boot if missing):

- **Username:** `admin`
- **Password:** `admin123`

5. From **MainForm**, open **شاشة الكاشير (POS)** via the sidebar to sell; other sidebar items and multi-cart buttons are UI placeholders for upcoming features.
