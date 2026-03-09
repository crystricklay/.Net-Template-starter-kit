## .NET Template Starter Kit

### Overview

**.NET Template Starter Kit** is a modular starter template for building modern ASP.NET Core backend services using **Clean Architecture**, **CQRS with MediatR**, **Entity Framework Core**, and **PostgreSQL**.

The template is designed to:

- enforce clear separation of concerns,
- make the codebase easy to scale and maintain,
- provide a ready-to-use CRUD example (`User`) as a reference for new features,
- standardize patterns across projects (Repositories, Unit of Work, MediatR handlers, global error handling).

You can use this repository as a starting point for REST APIs, microservices, or enterprise backend systems.

---

## Architecture

The solution follows a layered architecture inspired by **Clean Architecture** and **DDD (Domain-Driven Design)**:

### Domain Layer (`Template.Domain`)

**Responsibilities:**

- Core business concepts and invariants.
- Entities and value objects.
- Domain contracts such as repository interfaces and `IUnitOfWork`.

**Contains:**

- `Entities/`
  - `Base/BaseEntity` – base class for entities with `Id`, `CreatedAt`, `UpdatedAt`.
  - `User` – sample aggregate root with an `Email` property.
- `Interfaces/`
  - `Repositories/Base/IRepository<T>` – generic CRUD repository abstraction.
  - `Repositories/IUserRepository` – user-specific repo abstraction (`GetByEmailAsync` etc.).
  - `UnitOfWork/IUnitOfWork` – entry point for repositories + `SaveChangesAsync`.

**Key rules:**

- No dependencies on other project layers.
- Only pure domain logic and abstractions.

---

### Application Layer (`Template.Application`)

**Responsibilities:**

- Application use cases (commands & queries).
- Input/Output models (DTOs).
- Application services (orchestrating domain + infrastructure).
- MediatR handlers & pipelines.

**Contains:**

- `DTOs/`
  - `User/UserDTO` – data shape returned/accepted by user-related use cases.
- `Services/`
  - `Interfaces/IUserService` – application contract for user operations (CRUD, `GetByEmailAsync`).
  - `UserService` – implementation that uses `IUnitOfWork` and domain entities.
- `Commands/Users/` – write operations (create, update, delete).
- `Queries/Users/` – read operations (get list, get by id, get by email).
- `Handlers/Users/` – MediatR handlers delegating to `IUserService`.
- `ApplicationModule/ApplicationDependencies` – DI registration point (MediatR + services).
- `Options/Database/PostgresOptions` – configuration model for database options.

**Patterns:**

- **CQRS with MediatR** – each use case is a `Command` or `Query` + `Handler`.
- **Thin controllers** – controllers only talk to `IMediator`, not to repositories/services directly.

---

### Infrastructure Layer (`Template.Infrastructure`)

**Responsibilities:**

- Implementations of domain interfaces (repositories, unit of work).
- Database access with EF Core + PostgreSQL.
- Migrations.
- External integrations (if any).

**Contains:**

- `Persistence/ApplicationDbContext` – EF Core `DbContext` with `DbSet<User>`.
- `Repositories/Base/Repository<T>` – generic CRUD implementation for `IRepository<T>`.
- `Repositories/UserRepository` – `User`-specific repo with `GetByEmailAsync`.
- `UnitOfWork/UnitOfWork` – aggregates repositories and persists changes.
- `InfrastructureModule/InfrastructureDependencies` – DI registration for:
  - `ApplicationDbContext`
  - `IUserRepository`
  - `IUnitOfWork`
  - PostgreSQL configuration.

**Key rules:**

- Depends on `Template.Domain` and `Template.Application`.
- No direct dependency on the API layer.

---

### API Layer (`Template.API`)

**Responsibilities:**

- HTTP endpoints.
- Composition root (application startup & DI wiring).
- Middleware pipeline.
- OpenAPI/Swagger configuration.

**Contains:**

- `Program.cs`
  - Registers `InfrastructureDependencies.AddInfrastructure(...)`
  - Registers `ApplicationDependencies.AddApplicationServices(...)`
  - Adds controllers, Swagger, HTTPS redirection, etc.
- `Controllers/UsersController`
  - Exposes CRUD endpoints for `User` via MediatR.
- `Middleware/ExceptionHandlingMiddleware`
  - Centralized exception handling and consistent error responses.
- `APIModules/APIDependencies`
  - Place for additional API-specific options bindings (e.g., configuration options).

---

## Technology Stack

- **.NET 10**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **MediatR (CQRS)**
- **Repository & Unit of Work patterns**
- **OpenAPI / Swagger**
- **Dependency Injection**

---

## Project Structure (High Level)

```text
Template.Domain/
  Entities/
  Interfaces/
  Template.Domain.csproj

Template.Application/
  DTOs/
  Services/
  Commands/
  Queries/
  Handlers/
  Options/
  ApplicationModule/
  Template.Application.csproj

Template.Infrastructure/
  Persistence/
  Repositories/
  UnitOfWork/
  InfrastructureModule/
  Template.Infrastructure.csproj

Template.API/
  Controllers/
  Middleware/
  APIModules/
  Program.cs
  Template.API.csproj
