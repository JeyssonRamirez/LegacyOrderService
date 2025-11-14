

## ⚙️ Layers Description

| Layer            | Responsibility                                                                    |
| ---------------- | --------------------------------------------------------------------------------- |
| **Presentation** | Console app for user input/output. Depends on `IOrderAppService`.                 |
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