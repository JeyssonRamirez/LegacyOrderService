# LegacyOrderService - Refactored Solution

## Overview

This C# (.NET 8) console application was originally built to meet immediate business needs. While functional, the original codebase had limitations in scalability, maintainability, and testability.  

This refactored version implements a **clean, N-layer architecture** designed with **SOLID principles**, making the system more robust, modular, and easier to extend as the business grows.

Key goals of this refactor:
- Identify and fix bugs or runtime issues
- Refactor poor architecture and code smells
- Apply appropriate design patterns and modern C# best practices
- Improve performance, resilience, scalability, and testability
- Make engineering trade-offs aligned with real-world scenarios

> This project treats the codebase as a real production system you’ve inherited: no detailed requirements, no documentation — just an opportunity to evolve a legacy system.

---

## Architecture

The solution now follows a **clear N-layer structure** with separation of concerns:

1. **Presentation Layer**
   - Handles user interaction via the console application.
   - Delegates business operations to the Application layer.

2. **Application Layer**
   - Coordinates use cases and orchestrates business logic.
   - Calls domain services from the Core layer.
   - Contains DTOs, commands, and queries for communication between layers.

3. **Core Layer**
   - Represents the domain model and business rules.
   - Contains entities, value objects, interfaces, and domain services.
   - Pure C# logic, independent of frameworks or infrastructure.

4. **Data Layer**
   - Handles data access using repositories and EF Core.
   - Isolated from the domain, ensuring testability and modularity.

5. **Cross-Cutting Layer**
   - Includes shared concerns like logging, validation, and utility functions.

6. **IoC / Dependency Injection Layer**
   - Centralized configuration for dependency injection.
   - Supports easy swapping of implementations and decoupling.

## 🧩 Project Structure

```
ClockPatience/
├── Presentation.LegacyOrderConsole before LegacyOrderService/
│   └── Program.cs
│   └── Startup.cs
│   └── appsettings.json
├── Application/
│   ├── Application.Definition (Interfaces)/
│   │   └── IOrderAppService.cs
│   └── Application.Implementation (Implementation)/
│       └── MapperHelper.cs
│       └── OrderAppService.cs

├── Core/
│   ├── Entities/
│   │   ├── Order.cs
│   │   ├── Product.cs
│   │   ├── StatusType.cs
│   │   ├── Entity.cs
│   │   ├── ActionType.cs
│   │   ├── etc...
├── Core/
│   ├── Dtos/
│   │   ├── ModifyOrderDTO.cs
│   │   ├── RecordApiResult.cs
│   └── Repositories (Interfaces)
│          └── IOrderRepository.cs 
│          └── IProductRepository.cs
│          └── etc...
├── Data/  (implementation data access)
│   ├── Common/
│   └── IGenericRepository.cs (Interface)
│   └── IUnitOfWork.cs (Interface)
│   ├── DataAccess.Provider.SQLite (SQLite Project)
│   └── Repositories 
│          └── OrderRepository.cs (Implementation)
│          └── ProductRepository.cs (Implementation)
│          └── etc...
├── Crosscutting/ (Tools)
│   ├── Extension/
│   │   ├── CustomEnumExtensions.cs
│   │   ├── StringValueAttribute.cs
├── IoC/
│   └── DependencyInjection.cs
```

---


## ⚙️ Layers Description

| Layer            | Responsibility                                                                    |
| ---------------- | --------------------------------------------------------------------------------- |
| **Presentation** | Console app for user input/output. Depends on `IOrderAppService`. project nenamed |
| **Application**  | Implements methods orchestration and exposes use cases.                           |
| **Core**         | Contains the domain entities and business logic (game rules).                     |
| **Data**         | Handles input deck reading from console.                                          |
| **Crosscutting** | Shared utilities, constants, and helpers.                                         |
| **IoC**          | Configures Dependency Injection using `Microsoft.Extensions.DependencyInjection`. |

---

## 🧠 Technologies Used

* **.NET 8 / C# 12**
* **xUnit** for unit testing
* **FluentAssertions** for expressive assertions
* **Moq** for mocking interfaces
* **SOLID principles** applied to all layers
* **Dependency Injection (IoC)** via `ServiceCollection`



## 📐 SOLID Principles Used

* **S**ingle Responsibility — each class has one purpose (OrderRepository, ProductRepository, AppService, etc.)
* **O**pen/Closed — logic is extensible without modifying core entities
* **L**iskov Substitution — abstractions (interfaces) are replaceable
* **I**nterface Segregation — clean interfaces for specific responsibilities
* **D**ependency Inversion — high-level modules depend on abstractions, not concrete implementations

---

## 🧑‍💻 Author

**Jeysson Ramirez**
Senior Software Developer & Technical Leader
.NET | C# | Azure | Clean Architecture | SOLID

---

## 🏁 License

This project is open-source under the **MIT License**.