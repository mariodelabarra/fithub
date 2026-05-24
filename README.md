# fithub

# Overview
The Fithub is a comprehensive application designed to help users achieve their fitness goals by providing personalized workout routines and meal plans. Whether you're looking to build muscle, lose weight, or maintain a healthy lifestyle, this app simplifies planning and tracking your progress.

## Projects

### Platform Projects

| Layer      | Project                               | Purpose                                         |
|------------|---------------------------------------|-------------------------------------------------|
| API        | `Fithub.Platform.API`                 | REST endpoints for all platform operations      |
| Service    | `Fithub.Platform.Services`            | Business logic and orchestration                |
| Domain     | `Fithub.Platform.Domain`              | Core domain entities, DTOs, and validators      |
| Repository | `Fithub.Platform.Repositories`        | EF Core DbContext, entity configurations, and migrations |

### Common (Shared) Projects

| Project                             | Purpose                                         |
|-------------------------------------|-------------------------------------------------|
| `FitHub.Platform.Common`            | Global exception handler and shared middleware  |
| `FitHub.Platform.Common.Domain`     | `BaseEntity` and shared domain abstractions     |
| `FitHub.Platform.Common.Service`    | `IValidatorService` / `ValidatorService` wrapping FluentValidation |
| `FitHub.Platform.Common.Repository` | `BaseRepository<T>` generic CRUD via Dapper     |

## Key Architecture Patterns

The **FitHub** platform implements several key architectural patterns:

1. **Layered Architecture**: Clear separation between API, service, domain, and data access layers  
2. **Domain-Driven Design**: Focus on core domain models and business logic  
3. **Repository Pattern**: Abstraction of data access through repository interfaces  
4. **Dependency Injection**: Loose coupling between components  
5. **Validation Pipeline**: Structured input validation using Fluent Validation
