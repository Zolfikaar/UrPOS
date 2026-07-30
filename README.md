# UrPOS — Offline-First Point of Sale

UrPOS is a modern, **offline-first** Point of Sale and inventory system for local desktop use. It is built with **C# / .NET 10**, **WinForms**, **Dapper**, and **PostgreSQL**, with a full Arabic **RTL** presentation layer.

**Status:** Core POS, products, invoice history, encrypted backup, and demo mode are implemented and buildable. Selected roadmap items are deferred (see below).

## Architecture (Clean Architecture)

```
UrPOS/
├── UrPOS.Core/              # Domain entities, interfaces, UserSession, ServiceResult
├── UrPOS.Infrastructure/    # Dapper + PostgreSQL, auth, services, DbInitializer
├── UrPOS.WinForms/          # UI (Setup, Login, Main shell, POS) + DI host
│   ├── Controls/            # Products, Invoice History, Settings, EmptyState
│   └── Forms/               # Setup, Login, Main, PosSales, Product
├── UrPOS.Tests/             # xUnit + Moq
└── README.md
```

| Layer | Responsibility |
|-------|----------------|
| **Core** | Entities, repository/service contracts, thread-safe `UserSession` |
| **Infrastructure** | Dapper repositories, BCrypt hashing, JSON config, schema seed |
| **WinForms** | Presentation + `Microsoft.Extensions.Hosting` Dependency Injection |

Composition root: `ServiceConfigurator` registers services and forms; `Program.Main` initializes the database, optionally shows the first-run wizard, then runs the login → main shell loop.

## Key Features Implemented

- **First-Run Setup Wizard (`SetupForm`)** — Create admin account or enter as guest.
- **Authentication & roles** — Login with hashed passwords; role shown in the main header.
- **Demo account** — Default seeded credentials when no admin exists: `admin` / `admin123`.
- **Guest / Demo Mode** — Temporary guest session; Demo Mode toggle in Settings with trial-day countdown in the header (`مرحباً، زائر تجريبي | الدور: Admin | الفترة التجريبية: N أيام`).
- **Main shell (`MainForm`)** — RTL sidebar, dashboard quick actions.
- **POS sales (`PosSalesForm`)** — Cart, barcode/name search, numpad, parked invoices, **manual product entry**, checkout.
- **Products** — CRUD with **barcode uniqueness** checks, **units of measure** (قطعة / صندوق / عبوة / …).
- **Invoice History (`InvoicesHistoryControl`)** — Completed sales from PostgreSQL via `IInvoiceRepository`, parked drafts from session, line-item details, resume parked into POS.
- **Settings** — Encrypted backup/restore, Demo Mode indicator, OCR add-on placeholder.

## Deferred / Future Roadmap

The following are **not** in the current release and are tracked for later:

| Item | Notes |
|------|--------|
| **Offline Password Recovery System** (`المهمة السادسة`) | **Officially deferred** to future releases. |
| **OCR Purchase Invoice Scanner** (`قراءة قوائم التسوق والصور`) | UI placeholder under Settings → add-ons; planned as a paid/integrated service. |
| **External Hardware / Scale Integration** | Future add-on for scales and related devices. |

## UI / RTL Notes

All primary forms use `RightToLeft = Yes` and `RightToLeftLayout = True`.

| Screen | Highlights |
|--------|------------|
| Setup / Login | Centered card, teal accent, RTL field alignment |
| MainForm | Right sidebar, demo/trial header when active |
| PosSalesForm | Cart grid, parked invoices, manual item dialog |
| Invoice History | Filters (invoice #, date range, Completed/Parked), details grid |

## Tech Stack

- .NET 10 / WinForms
- Dapper + Npgsql (PostgreSQL)
- BCrypt.Net-Next
- Microsoft.Extensions.Hosting (DI)
- System.Text.Json (`appsettings.json`)
- xUnit + Moq

## Getting Started

### Prerequisites

1. **.NET 10 SDK**
2. Local **PostgreSQL** instance (running and reachable)

### Database configuration

Edit `UrPOS.WinForms/appsettings.json`:

```json
{
  "DbHost": "localhost",
  "DbPort": 5432,
  "DbName": "urpos_db",
  "DbUsername": "postgres",
  "DbPassword": "postgres"
}
```

On startup, `DbInitializer` will:

1. Ensure the database exists
2. Apply the embedded schema script (`001_InitialSchema.sql`), including `unit_of_measure` on products
3. Seed roles and default admin (`admin` / `admin123`) if missing
4. Clean orphan guest rows if needed

*(There is no separate EF migration step — schema is applied by the initializer.)*

### Build & run

```bash
dotnet restore
dotnet build
dotnet run --project UrPOS.WinForms
```

Or rebuild the full solution from Visual Studio / `dotnet build UrPOS.slnx`.

### First launch

- If **no users** exist → `SetupForm` appears (create admin **or** enter as guest).
- Otherwise → `LoginForm`.

| Field | Value |
|-------|--------|
| Username | `admin` |
| Password | `admin123` |

Guest login uses a temporary account (display name **زائر تجريبي**); cart browsing works, checkout remains blocked for guests. Admin rights are unchanged when Demo Mode is only an indicator/toggle.
