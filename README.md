# .NET Template Starter Kit

> **Quick links:** [Implementation Guide](#implementation-guide) · [Getting Started](#getting-started) · [User CRUD Endpoints](#user-crud-endpoints) · [Adding a New Entity](#adding-a-new-entity)

---

## Overview

**.NET Template Starter Kit** is a modular starter template for building modern ASP.NET Core backend services using **Clean Architecture**, **CQRS with MediatR**, **Entity Framework Core**, and **PostgreSQL**.

The template provides:

- Clear separation of concerns across layers
- Scalable, maintainable, and testable structure
- Ready-to-use User CRUD as a reference implementation
- Standard patterns: Repository, Unit of Work, MediatR handlers, global error handling

Use this repository as a starting point for REST APIs, microservices, or enterprise backend systems.

---

## Architecture

The solution follows **Clean Architecture** and **DDD** principles:

| Layer | Project | Responsibilities |
|-------|---------|------------------|
| **Domain** | `Template.Domain` | Entities, interfaces, domain contracts. No external dependencies. |
| **Application** | `Template.Application` | Use cases, DTOs, services, MediatR commands/queries and handlers. |
| **Infrastructure** | `Template.Infrastructure` | EF Core, PostgreSQL, repository implementations, Unit of Work. |
| **API** | `Template.API` | HTTP endpoints, middleware, Swagger, DI composition. |

---

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- MediatR (CQRS)
- Repository & Unit of Work patterns
- OpenAPI / Swagger
- Dependency Injection

---

## Implementation Guide

### 1. Required NuGet Packages

#### Domain (`Template.Domain`)

No external packages — pure domain logic only.

#### Application (`Template.Application`)

```bash
dotnet add Template.Application package MediatR
dotnet add Template.Application package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add Template.Application package Microsoft.Extensions.Configuration.Abstractions
```

#### Infrastructure (`Template.Infrastructure`)

```bash
dotnet add Template.Infrastructure package Microsoft.EntityFrameworkCore
dotnet add Template.Infrastructure package Microsoft.EntityFrameworkCore.Design
dotnet add Template.Infrastructure package Npgsql
dotnet add Template.Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add Template.Infrastructure package Microsoft.Extensions.Configuration
```

#### API (`Template.API`)

```bash
dotnet add Template.API package Swashbuckle.AspNetCore
dotnet add Template.API package Microsoft.AspNetCore.OpenApi
```

### 2. Project References

- **Application** → Domain
- **Infrastructure** → Application, Domain
- **API** → Application, Infrastructure

### 3. DI Registration Flow

**`Program.cs`** wires everything:

```csharp
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

**`ApplicationDependencies`** registers MediatR and application services:

```csharp
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationDependencies).Assembly));
services.AddScoped<IUserService, UserService>();
```

**`InfrastructureDependencies`** registers DbContext, repositories, and Unit of Work.

---

## Getting Started

### Prerequisites

- .NET SDK 10
- PostgreSQL
- A database created (e.g. `template_db`)

### 1. Database Configuration

Add connection string to `Template.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=template_db;Username=postgres;Password=your_password"
  }
}
```

### 2. Install EF Core Tools (one-time)

```bash
dotnet tool install --global dotnet-ef
```

### 3. Create and Apply Migrations

From the solution root:

```bash
# Create initial migration
dotnet ef migrations add InitialCreate \
  --project Template.Infrastructure \
  --startup-project Template.API \
  --output-dir Migrations

# Apply to database
dotnet ef database update \
  --project Template.Infrastructure \
  --startup-project Template.API
```

### 4. Run the API

```bash
dotnet run --project Template.API
```

Swagger UI: `http://localhost:5277/`

---

## User CRUD Endpoints

Base route: `api/v1/users`

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/v1/users` | Get all users |
| `GET` | `/api/v1/users/{id}` | Get user by ID |
| `GET` | `/api/v1/users/by-email?email=...` | Get user by email |
| `POST` | `/api/v1/users` | Create user |
| `PUT` | `/api/v1/users/{id}` | Update user |
| `DELETE` | `/api/v1/users/{id}` | Delete user |

---

## Request Flow (CQRS)

Example: **Get user by ID**

1. **API** — `UsersController.GetById(id)` sends `GetUserByIdQuery(id)` to `IMediator`
2. **Application** — `GetUserByIdHandler` receives query, calls `IUserService.GetByIdAsync(id)`
3. **Application** — `UserService` uses `IUnitOfWork.Users.GetByIdAsync(id)`, maps to DTO
4. **Infrastructure** — `UserRepository` queries `DbSet<User>` via EF Core

Same pattern for all commands and queries.

---

## Adding a New Entity

Example: adding `Product` CRUD.

### 1. Domain

- Create `Product` entity
- Add `IProductRepository : IRepository<Product>`
- Add `IProductRepository Products { get; }` to `IUnitOfWork`

### 2. Infrastructure

- Implement `ProductRepository : Repository<Product>, IProductRepository`
- Update `UnitOfWork` constructor and `Products` property
- Register `IProductRepository` in `InfrastructureDependencies`
- Add `DbSet<Product>` to `ApplicationDbContext`

### 3. Application

- Create `ProductDTO` (and optional `CreateProductRequest`, `UpdateProductRequest`)
- Add `IProductService` and `ProductService`
- Create queries: `GetProductsQuery`, `GetProductByIdQuery`
- Create commands: `CreateProductCommand`, `UpdateProductCommand`, `DeleteProductCommand`
- Implement handlers in `Handlers/Products`
- Register `IProductService` in `ApplicationDependencies`

### 4. API

- Create `ProductsController` with `IMediator`, mirror `UsersController` endpoints

### 5. Migrations

```bash
dotnet ef migrations add AddProduct \
  --project Template.Infrastructure \
  --startup-project Template.API \
  --output-dir Migrations

dotnet ef database update \
  --project Template.Infrastructure \
  --startup-project Template.API
```

---

## Global Error Handling

`ExceptionHandlingMiddleware` maps exceptions to HTTP status codes:

| Exception | Status Code |
|-----------|-------------|
| `UnauthorizedAccessException` | 401 |
| `ValidationException` | 400 |
| `KeyNotFoundException` | 404 |
| `InvalidOperationException` | 400 |
| Other | 500 |

Ensure it is registered in `Program.cs`:

```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

---

## Project Structure

```
Template.Domain/
  Entities/
  Interfaces/

Template.Application/
  DTOs/
  Services/
  Commands/
  Queries/
  Handlers/
  ApplicationModule/

Template.Infrastructure/
  Persistence/
  Repositories/
  UnitOfWork/
  InfrastructureModule/

Template.API/
  Controllers/
  Middleware/
  Program.cs
```

---

## Next Steps

- Add FluentValidation for request validation
- Use ProblemDetails (RFC 7807) for error responses
- Add JWT authentication and authorization
- Add domain events for complex workflows
- Add logging, metrics, and health checks
