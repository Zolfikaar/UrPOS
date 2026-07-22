# 🚀 UrPOS - Core Engine & Infrastructure

A unified Point of Sale (POS) and Inventory Management System core engine, engineered with Clean Architecture principles for high performance on local, lightweight machines.

## 🏗️ Architectural Overview (Clean Architecture)

The project is strictly decoupled into independent layers to ensure high maintainability, testability, and extensibility:

* **UrPOS.Core:** Contains Domain Entities, Interfaces, Stateful Session tracking (`UserSession`), and Service response abstractions (`ServiceResult`).
* **UrPOS.Infrastructure:** 
  * Micro-ORM Data Access powered by **Dapper** & **PostgreSQL**.
  * Security & Cryptography using **BCrypt.Net-Next**.
  * Local configuration storage with JSON (`JsonConfigurationService`).
  * Embedded Schema Auto-Initialization & Migrations (`DbInitializer`).
* **UrPOS.WinForms (Presentation):** UI layer powered by **Microsoft.Extensions.Hosting** for Dependency Injection.
* **UrPOS.Tests:** Automated unit test suite leveraging **xUnit** and **Moq** for domain validation rules.

## ⚡ Core Features & Highlights

- **Unified Invoice Engine:** Polymorphic inheritance for `SalesInvoice` and `PurchaseInvoice`, persisting lines and inventory movements atomically in single Transactions.
- **Supplier & Inventory Ledger:** Automated stock movement logging and real-time supplier balance updates.
- **Stateful Thread-Safe Security:** Password hashing via BCrypt and active cashier tracking using in-memory `UserSession` (Thread-Safe Singleton).
- **Zero-Setup Database Deployment:** Auto-creates PostgreSQL database, initial schema, indexes, and seeded credentials (`admin` / `admin123`) on first boot.
- **Robust Application Services:** Business-level validation layer protecting against inventory depletion and negative-margin sales.

## 🛠️ Tech Stack & Dependencies

- **Framework:** .NET 10.0 (C#)
- **Data Access:** Dapper (Micro-ORM), Npgsql
- **Security:** BCrypt.Net-Next
- **Dependency Injection:** Microsoft.Extensions.Hosting
- **Testing Suite:** xUnit, Moq
- **Configuration:** System.Text.Json