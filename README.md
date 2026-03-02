# EF6 QueryTaker v3

An ASP.NET MVC 5 support-ticket web application built on **.NET Framework 4.8**, **Entity Framework 6**, and **ASP.NET Identity**.

This project helps teams capture, triage, assign, and track service queries (tickets) with role-aware access for administrators, operators, engineers, and customers.

---

## Overview

EF6 QueryTaker v3 is structured around a classic MVC architecture:

- **Authentication & authorization** with ASP.NET Identity + OWIN cookies.
- **Data access** with Entity Framework 6 Code First and migrations.
- **Role-based ticket workflow** in `QueriesController`.
- **Administrative views** for users and roles.

The default route is:

```text
/{controller}/{action}/{id}
```

with startup landing at `Home/Index`.

---

## Core Features

### 1) Role-based query visibility
The query list behaves differently depending on the signed-in user role:

- **Administrator / Operator**: can view all queries.
- **Engineer**: sees assigned queries and unassigned queries.
- **Customer (User)**: sees only their own created queries.

### 2) Query lifecycle management
Users can perform standard ticket operations:

- Create
- View details
- Edit
- Delete

When a customer creates a query, status defaults to **To Be Processed**.

### 3) Role-specific edit behavior
During edits:

- Admin/Engineer can update assignee and status.
- Subject, description, category, and `DateUpdated` are maintained on save.

### 4) Built-in status and category options
Static collections are used for user-facing dropdowns:

**Statuses**
- To Be Processed
- In Progress
- Processed
- Cancelled

**Categories**
- Home Security
- Plumbing
- Electricity
- Interior Design
- Cyber Security
- Exterior Design
- Hardware

### 5) Identity-driven user and role management
- `RolesController` exposes role listing.
- `UsersController` lists users and highlights role membership flags (admin/engineer/user).

---

## Tech Stack

- **Framework:** ASP.NET MVC 5 (`System.Web.Mvc`)
- **Runtime:** .NET Framework 4.8
- **ORM:** Entity Framework 6.4.4
- **Auth:** ASP.NET Identity 2.x + OWIN Cookie Auth
- **Database (default):** SQL Server LocalDB (`(LocalDb)\\MSSQLLocalDB`)
- **Client UI:** Bootstrap + jQuery

---

## Project Structure (high level)

```text
EF6-QueryTaker_v3/
├── App_Start/               # MVC, auth, routing, bundles
├── Context/                 # EF DbContext
├── Controllers/             # MVC controllers (queries, users, roles, account)
├── Migrations/              # EF6 code-first migrations
├── Models/                  # Domain and Identity models
├── Views/                   # Razor views
├── Content/                 # CSS/Bootstrap
├── Scripts/                 # JS dependencies
├── Web.config               # App/runtime/db configuration
└── EF6-QueryTaker.csproj    # Project definition
```

---

## Getting Started

## Prerequisites

Recommended local setup:

- Windows with Visual Studio 2022 (or 2019) + ASP.NET workload
- .NET Framework 4.8 targeting pack
- SQL Server LocalDB (installed with Visual Studio)
- NuGet package restore enabled

> Note: This is a classic `.csproj` for ASP.NET MVC 5 on .NET Framework, not an ASP.NET Core SDK-style project.

## Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/ValsCodes/EF6-QueryTaker_v3.git
   cd EF6-QueryTaker_v3
   ```

2. Restore NuGet packages:

   ```bash
   nuget restore EF6-QueryTaker.sln
   ```

   or from Visual Studio: **Right click solution → Restore NuGet Packages**.

3. Apply Entity Framework migrations:

   ```powershell
   Update-Database
   ```

   Run this in **Package Manager Console** with the default project set to the web app.

4. Run the application:

   - Set `EF6-QueryTaker` as startup project.
   - Press **F5** (IIS Express).

---

## Default Data & Roles

This repository defines role semantics in code (`RolesEnum`) with the following names:

- `Administrator`
- `Engineer`
- `Operator`
- `User`

If your local database does not yet contain roles/users, create them using your preferred Identity seeding or admin workflow before testing role-dependent query behavior.

---

## Configuration Notes

- Connection string name: `DefaultConnection`
- Default DB target: LocalDB MDF in `|DataDirectory|`
- Cookie login path: `/Account/Login`

You can swap the database target by changing `DefaultConnection` in `Web.config`.

---

## Current Limitations / Future Work

A few scaffolding areas are present and can be expanded:

- `UsersController` create/edit/delete actions are placeholders.
- Migration `Seed(...)` is currently empty.
- Role initialization is not automated in migration seed.

Potential improvements:

- Add startup role/user seeding.
- Add filtering/search/sorting for queries.
- Add comment workflow using existing comment-related models.
- Add automated tests around controller role behavior.

---

## License

No license file is currently included in this repository. If you plan to open-source or distribute it, add a `LICENSE` file with your intended terms.
