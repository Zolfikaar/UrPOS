# UrPOS - Core System (The Architectural Backbone of Madar)

UrPOS is the robust, high-performance shared core codebase designed to power various retail and specialized business management sectors (such as Supermarkets, Pharmacies, and General Markets) under the commercial product line brand **Madar**.

## 🚀 Engineering Philosophy
- **Robust Backend:** Absolute focus on backend stability, database integrity, and atomic transactional operations.
- **High Performance:** Heavily optimized architecture built to run seamlessly on low-spec client machines with minimal CPU and RAM footprints.
- **Clean Architecture:** Strict separation of concerns ensuring that the core system remains decoupleable, highly testable, and completely ready for future Cloud/API synchronization without breaking foundational logic.
- **Database Stability:** Leverages localized PostgreSQL configurations for superior concurrency management and schema-less flexibility using `JSONB` fields for dynamic module extensions.

## 🛠️ Technology Stack
- **Language:** C# (.NET 10.0 LTS)
- **UI Framework:** Windows Forms (WinForms)
- **Data Access:** Dapper (Micro-ORM) & Npgsql
- **Database Engine:** PostgreSQL

## 📂 Solution Architecture
The solution follows a streamlined Clean Architecture structure:
- `1.Core (UrPOS.Core)`: Contains domain entities, repository contracts (interfaces), and fundamental business rules. Zero external dependencies.
- `2.Infrastructure (UrPOS.Infrastructure)`: Handles database connectivity, persistence operations, transaction management, and optimized raw SQL execution via Dapper.
- `3.Presentation (UrPOS.WinForms)`: Lightweight user interface layer, managing dependency injection container composition, and application startup lifecycles.

## 🗄️ Core Progress
- [x] Initial relational database schema design.
- [x] Repository Pattern infrastructure setup for Products.
- [x] High-performance, case-insensitive partial product searching (`SearchByNameAsync`).
- [x] Atomic transactional pipeline setup for Users, Roles, and Permissions tracking.