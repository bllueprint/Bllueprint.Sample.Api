# Bllueprint

A sample **.NET 10** RESTful API demonstrating **Clean Architecture** principles using **CQRS** (Command Query Responsibility Segregation) via **MediatR**. The project is built around a simple task management domain and serves as a reference implementation for the `Bllueprint.Core` framework packages.

---

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Domain Model](#domain-model)
- [API Endpoints](#api-endpoints)
- [Key Dependencies](#key-dependencies)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Code Quality](#code-quality)
- [License](#license)

---

## Overview

Bllueprint is a **sample application** that showcases how to build a well-structured ASP.NET Core Web API using:

- **Clean Architecture** — strict separation between Domain, Application, Infrastructure, and API layers.
- **CQRS with MediatR** — commands and queries are isolated handlers, keeping business logic focused and testable.
- **Domain-Driven Design (DDD)** — the `TaskItem` aggregate encapsulates its own state transitions with enforced invariants.
- **Bllueprint.Core packages** — a set of opinionated base libraries (`Core.Domain`, `Core.Application`, `Core.Api`) that provide building blocks like `Aggregate<T>`, `CommandHandler<TCommand, TResult>`, transitions, and notification contexts.

---

## Architecture

The solution follows a classic **4-layer Clean Architecture** layout:

```
┌─────────────────────────────────┐
│           Bllueprint.Api        │  ← ASP.NET Core Web API, controllers
├─────────────────────────────────┤
│       Bllueprint.Application    │  ← CQRS commands/queries, use-case orchestration
├─────────────────────────────────┤
│         Bllueprint.Domain       │  ← Aggregates, domain entities, business rules
├─────────────────────────────────┤
│      Bllueprint.Infrastructure  │  ← Repository implementations, data access
└─────────────────────────────────┘
```

Dependencies flow **inward only** — Infrastructure depends on Application, Application depends on Domain; the Api layer composes them all.

---

## Project Structure

```
Bllueprint.slnx
├── Directory.Build.props          # Shared build config (TargetFramework, Nullable, analyzers)
├── Directory.Packages.props       # Centralized NuGet package version management
├── nuget.config                   # Package source configuration
└── src/
    ├── Bllueprint.Api/
    │   ├── Program.cs             # App bootstrap & DI composition root
    │   ├── TaskController.cs      # REST endpoints for task management
    │   ├── appsettings.json
    │   └── Properties/
    │       └── launchSettings.json
    │
    ├── Bllueprint.Application/
    │   ├── CreateTaskCommand.cs   # Create a new task
    │   ├── StartTaskCommand.cs    # Transition task → InProgress
    │   ├── CompleteTaskCommand.cs # Transition task → Done (with access validation)
    │   ├── ReopenTaskCommand.cs   # Reopen a completed or cancelled task
    │   ├── RenameTaskCommand.cs   # Rename a task (stub)
    │   ├── GetAllTasksQuery.cs    # Query all tasks
    │   ├── ITaskRepository.cs     # Repository abstraction
    │   ├── IAccessValidator.cs    # Access control abstraction
    │   ├── AccessValidator.cs     # Access control implementation
    │   └── ServiceCollectionExtensions.cs
    │
    ├── Bllueprint.Domain/
    │   ├── TaskItem.cs            # Core aggregate with state machine transitions
    │   ├── TaskStatus.cs          # Task status enum
    │   └── ServiceCollectionExtensions.cs
    │
    └── Bllueprint.Infrastructure/
        ├── TaskRepository.cs      # In-memory repository implementation
        └── ServiceCollectionExtensions.cs
```

---

## Domain Model

### `TaskItem` Aggregate

The `TaskItem` aggregate is the central domain entity. It manages its own lifecycle through a set of **type-safe, rule-enforced transitions** provided by the `Bllueprint.Core.Domain` framework.

#### Status Lifecycle

```
          ┌──────────────────────────────┐
          │                              ▼
  [ToDo] ──Start()──► [InProgress] ──Complete()──► [Done]
    ▲         │                │                      │
    │       Cancel()         Cancel()              Reopen()
    │         │                │                      │
    └─────────┴───── [Cancelled] ◄───────────────────-┘
          Reopen()
```

#### Properties

| Property      | Type              | Description                         |
|---------------|-------------------|-------------------------------------|
| `Id`          | `Guid`            | Unique identifier (auto-generated)  |
| `Title`       | `string`          | Task title                          |
| `Status`      | `TaskStatus`      | Current lifecycle status            |
| `StartedAt`   | `DateTimeOffset?` | When the task was started           |
| `CompletedAt` | `DateTimeOffset?` | When the task was completed         |

#### Task Status Enum

```csharp
public enum TaskStatus
{
    ToDo,
    InProgress,
    Done,
    Cancelled
}
```

#### Transition Rules

| Method       | Allowed From                  | Not Allowed If              |
|--------------|-------------------------------|-----------------------------|
| `Start()`    | `ToDo`                        | Any other status            |
| `Complete()` | `InProgress`                  | Not started yet             |
| `Cancel()`   | `ToDo`, `InProgress`          | Already `Done`              |
| `Reopen()`   | `Done`, `Cancelled`           | Still active                |
| `Rename()`   | `ToDo`, `InProgress`          | `Done` or `Cancelled`       |

Violating any rule throws a domain exception with a descriptive message, enforced at the aggregate level — never in the application layer.

---

## API Endpoints

Base URL: `http://localhost:5297` (HTTP) or `https://localhost:7187` (HTTPS)

| Method   | Route                         | Description                          |
|----------|-------------------------------|--------------------------------------|
| `GET`    | `/api/task`                   | Retrieve all tasks                   |
| `POST`   | `/api/task?title={title}`     | Create a new task with a given title |
| `PUT`    | `/api/task/{id}/start`        | Start a task (→ InProgress)          |
| `PUT`    | `/api/task/{id}/complete`     | Complete a task (→ Done)             |
| `PUT`    | `/api/task/{id}/reopen`       | Reopen a completed/cancelled task    |

### Interactive API Explorer

When running in Development mode, an **OpenAPI spec** and the **Scalar API Reference UI** are available at:

```
https://localhost:7187/openapi/v1.json   # OpenAPI spec
https://localhost:7187/scalar            # Scalar interactive UI
```

---

## Key Dependencies

All package versions are managed centrally via `Directory.Packages.props`.

| Package                              | Version       | Purpose                                      |
|--------------------------------------|---------------|----------------------------------------------|
| `MediatR`                            | 14.1.0        | CQRS mediator pattern                        |
| `Microsoft.AspNetCore.OpenApi`       | 10.0.7        | OpenAPI document generation                  |
| `Scalar.AspNetCore.Microsoft`        | 2.14.9        | Interactive API reference UI                 |
| `Bllueprint.Core.Domain`             | 1.0.0         | Aggregate base, transitions, domain services |
| `Bllueprint.Core.Application`        | 1.0.0         | `CommandHandler`, `ICommandResult`, notifications |
| `Bllueprint.Core.Api`                | 1.0.0         | `AppController` base class                   |
| `SonarAnalyzer.CSharp`              | 10.25.0       | Static code analysis                         |
| `StyleCop.Analyzers`                 | 1.2.0-beta    | Code style enforcement                       |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Clone & Run

```bash
git clone https://github.com/your-org/bllueprint.git
cd bllueprint

dotnet restore
dotnet run --project src/Bllueprint.Api
```

The API will be available at `http://localhost:5297`.

### Example Usage

```bash
# Create a task
curl -X POST "http://localhost:5297/api/task?title=My+first+task"

# List all tasks
curl http://localhost:5297/api/task

# Start a task (replace {id} with the actual GUID)
curl -X PUT "http://localhost:5297/api/task/{id}/start"

# Complete the task
curl -X PUT "http://localhost:5297/api/task/{id}/complete"
```

---

## Configuration

### `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### NuGet Sources (`nuget.config`)

All packages are resolved exclusively from `nuget.org` via **package source mapping**, ensuring supply-chain integrity.

---

## Code Quality

All projects share a common `Directory.Build.props` enforcing:

- **Target Framework**: `net10.0`
- **Nullable reference types**: enabled
- **Implicit usings**: enabled
- **Warnings as errors**: enabled — the build fails on any warning
- **SonarAnalyzer.CSharp**: static analysis for bug patterns and code smells
- **StyleCop.Analyzers**: consistent code style enforcement across all projects

---

## License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

Copyright © 2026 bllueprint