# UrPOS — Offline-First Point of Sale

UrPOS is a modern, **offline-first** Point of Sale and inventory system for local desktop use. It is built with **C# / .NET**, **WinForms**, **Dapper**, and **PostgreSQL**, with a full Arabic **RTL** presentation layer.

## Architecture (Clean Architecture)

```
UrPOS/
├── UrPOS.Core/              # Domain entities, interfaces, UserSession, ServiceResult
├── UrPOS.Infrastructure/    # Dapper + PostgreSQL, auth, services, DbInitializer
├── UrPOS.WinForms/          # UI (Setup, Login, Main shell, POS) + DI host
│   ├── Controls/            # EmptyStateControl, SettingsControl
│   └── Forms/               # SetupForm, LoginForm, MainForm, PosSalesForm
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

- **First-Run Setup Wizard (`SetupForm`)** — Dark-themed RTL card matching `LoginForm`; create admin account or enter as guest.
- **Authentication & roles** — Login with hashed passwords; role name shown in the main header.
- **Guest Demo Mode** — Temporary `guest` account is created on guest login and **automatically cleaned up** on logout or app exit.
- **Main shell (`MainForm`)** — RTL sidebar navigation, right-aligned user header, dashboard with quick actions.
- **POS sales (`PosSalesForm`)** — Cart grid with Arabic headers, barcode search, numpad, totals panel, multi-cart UI placeholders.
- **Centralized UI placeholders** — Products & Invoices show Arabic empty states; **Settings** has tabs for store profile, backup/security, and system preferences (visual layout ready for wiring).

## UI / RTL Notes

All primary forms use `RightToLeft = Yes` and `RightToLeftLayout = True`:

| Screen | Highlights |
|--------|------------|
| Setup / Login | Centered card, teal accent, RTL field alignment |
| MainForm | Right sidebar, right-aligned welcome/role, dashboard shortcuts |
| PosSalesForm | Arabic grid headers (`اسم المنتج`, `الكمية`, `السعر المفرد`, `الإجمالي`), bottom payment summary |

### Dashboard quick actions

- **شاشة البيع السريعة** → opens POS
- **إضافة منتج جديد** → Products empty-state view
- **النسخ الاحتياطي** → Settings → backup tab

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
2. Apply the embedded schema script (`001_InitialSchema.sql`)
3. Clean orphan guest rows if needed

*(There is no separate EF migration step — schema is applied by the initializer.)*

### Build & run

```bash
dotnet restore
dotnet build
dotnet run --project UrPOS.WinForms
```

### First launch

- If **no users** exist → `SetupForm` appears (create admin **or** enter as guest).
- Otherwise → `LoginForm`.

Default seeded admin (when created by older seed paths / manual setup):

| Field | Value |
|-------|--------|
| Username | `admin` |
| Password | `admin123` |

After login, use the sidebar or dashboard shortcuts to open POS, browse placeholder screens, or open Settings.
