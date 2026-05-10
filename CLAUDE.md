# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet restore
dotnet build
dotnet build --configuration Release

# Run API locally (requires MySQL running)
dotnet run --project src/FitHub.Platform.Workout.API

# Run all tests
dotnet test

# Run a single test project
dotnet test ./test/FitHub.Platform.Workout.Service.Tests/FitHub.Platform.Workout.Service.Tests.csproj
dotnet test ./test/FitHub.Platform.Workout.API.Tests/FitHub.Platform.Workout.API.Tests.csproj

# Run a specific test class or method
dotnet test --filter "FullyQualifiedName~ExerciseControllerCreateTests"

# Start the local MySQL database
docker-compose -f docker/docker-compose.yml up -d
```

Integration tests use Testcontainers and spin up their own MySQL container automatically — no manual database setup needed to run tests.

## Architecture

The solution follows a strict **layered architecture** with four horizontal layers:

```
API (Controllers + Middleware)
 └─ Service (Business Logic)
     └─ Repository (Dapper + MySQL)
         └─ Domain (Entities + Validators)
```

Each domain module owns all four layers. Currently one functional module exists: **Workout** (`FitHub.Platform.Workout.*`). The **Agglestone** module (`Fithub.Platform.Agglestone.*`) is a stub for authentication integration and is not yet functional.

### Common/shared projects

- `FitHub.Platform.Common.Domain` — `BaseEntity` (Id, CreatedOn, ModifiedOn) used by all entities
- `FitHub.Platform.Common.Repository` — `BaseRepository<T>` generic CRUD + pagination via Dapper
- `FitHub.Platform.Common.Service` — `IValidatorService` / `ValidatorService` wrapping FluentValidation
- `FitHub.Platform.Common` — `GlobalExceptionHandler`, `ProblemDetails` mappings

### Key patterns

**Repository pattern** — `BaseRepository<T>` handles raw SQL via Dapper. Entity-specific repositories extend `IBaseRepository<T>` and add query methods. Always raw SQL, no EF Core.

**Validation pipeline** — DTOs have paired FluentValidation validators. `FluentValidationAutoValidation` middleware validates controller inputs automatically. `ValidatorService` is used in the service layer for explicit validation.

**AutoMapper** — All DTO ↔ Entity mappings go through `WorkoutProfile`. Inject `IMapper` to map objects; never map manually.

**DI registration** — Each module has a `DependencyInjection.cs` static class with extension methods (e.g., `AddWorkoutModule()`). New services, repositories, and validators must be registered there.

**Exception handling** — Throw typed exceptions (`NotFoundException`, `ArgumentException`, `ValidationException`). `GlobalExceptionHandler` maps these to HTTP status codes (404, 400, 422, 500).

**OData** — The `GET /odata/exercises` endpoint supports `$filter`, `$orderby`, `$top`, `$skip`, `$select`, `$expand` via `MongoDB.AspNetCore.OData`.

### Authentication

Authentication uses **OpenID Connect + Cookie** against the Agglestone identity provider. Configuration in `appsettings.json`:
- `AgglestoneSettings:TenantId` — tenant identifier
- Authority resolves to `https://auth.agglestone.com/tenant/{TenantId}/v2/auth`
- PKCE flow; cookies named `AggleStone.Auth`, HttpOnly, Secure, SameSite=Strict, 1-hour sliding expiry

### Database

MySQL 8.0. Schema managed by **FluentMigrator** migrations in `FitHub.Platform.Workout.Repository/Migrations/`. Local connection string: `server=localhost;uid=fithub_admin;pwd=Testing99;database=fithub`. Test databases are ephemeral Testcontainer instances.

### Integration test setup

`CustomWebApplicationFactory` in `FitHub.Platform.Workout.API.Tests` starts a MySQL Testcontainer, runs FluentMigrator migrations, and overrides `IConfiguration` with the container's connection string. Tests share a single container per collection (via `[Collection]` + `ICollectionFixture`). Respawn resets database state between tests.

## CI/CD

GitHub Actions (`.github/workflows/main.yml`) runs on push/PR to `main`: clean → restore → build Release → test. Currently only `FitHub.Platform.Workout.Service.Tests` runs in CI.
