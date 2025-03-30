---

# **EnhancedPensionSystem** 🏦🔐

## **Overview**

EnhancedPensionSystem is a **modular, scalable, and maintainable** pension management system built using **.NET Core**. The system efficiently handles **employer and member management**, benefit eligibility, transactions, and notifications with **background job processing and validation**.

## **Architecture**

This project follows a **Clean Architecture** approach, ensuring separation of concerns with distinct **Application, Domain, Infrastructure, and Web API** layers.

### **Key Architectural Patterns Used**

1. **Unit of Work (UoW) Pattern** 🛠

   - Ensures that multiple operations within a request are handled in a single transaction.
   - Helps maintain consistency across repositories.

2. **Generic Repository Pattern** 📚

   - Provides a reusable data access layer.
   - Simplifies CRUD operations across entities.

3. **Fluent API Configuration** 🏗

   - Used for **database schema definitions and relationships**.
   - Ensures consistency and maintainability in **Entity Framework Core**.

4. **Fluent Validation** ✅

   - Used for validating **DTOs** and ensuring **data integrity**.
   - Provides clear and structured validation rules.

5. **Hangfire for Background Jobs** ⏳

   - Handles background tasks like **asynchronous notifications** and **report generation**.

---

## **Project Structure** 📂

```
EnhancedPensionSystem
│── EnhancedPensionSystem_Application  # Application Layer (Business Logic)
│   ├── Helpers
│   ├── Services
│   │   ├── Abstractions
│   │   ├── Implementations
│   ├── UnitOfWork
│   │   ├── Abstraction
│   │   ├── Implementations
│
│── EnhancedPensionSystem_Domain  # Domain Layer (Core Business Entities)
│   ├── Enums
│   ├── Models
│
│── EnhancedPensionSystem_Infrastructure  # Data & Persistence Layer
│   ├── DataContext
│   ├── Repository
│   │   ├── Abstractions
│   │   ├── Implementations
│
│── EnhancedPensionSystem_Tests  # Unit & Integration Tests (To be added)
│
│── EnhancedPensionSystem_WebAPP  # Presentation Layer (Web API)
│   ├── Controllers
│   ├── Extensions
│   ├── LoggerConfig
```

---

## **Technologies & Packages Used** 🏗

| **Technology**            | **Purpose**                   |
| ------------------------- | ----------------------------- |
| **.NET Core 8**           | Backend Framework             |
| **Entity Framework Core** | ORM for Database Interactions |
| **Fluent Validation**     | DTO Validation                |
| **Fluent API**            | Database Schema Configuration |
| **Hangfire**              | Background Jobs               |
| **Microsoft Identity**    | User Authentication           |
| **Swagger / Swashbuckle** | API Documentation             |
|                           |                               |
| **NLog**                  | Logging                       |
|                           |                               |

---

## **Getting Started 🚀**

### **1. Prerequisites**

Ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Hangfire Dashboard](https://www.hangfire.io/)
- [Postman](https://www.postman.com/) (for API testing)

### **2. Setup**

#### **Clone Repository**

```bash
git clone https://github.com/Victor-Ndulue/EnhancedPensionSystem.git
cd EnhancedPensionSystem
```

#### **Install Dependencies**

```bash
dotnet restore
```

#### **Database Migration**

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### **Run the Application**

```bash
dotnet run --project EnhancedPensionSystem_WebAPP
```

---

## **API Documentation 📝**

Swagger is enabled for API documentation.\
Once the app is running, navigate to:

```
http://localhost:<port>/swagger
```

This provides an interactive UI to test all available API endpoints.

---

## **Testing 🧪**

Unit tests and integration tests will be added using **xUnit** and **Moq**.
Run tests with:

```bash
dotnet test
```

---

## **Contributors 👥**

- \*\*Victor-Ndulue \*\*- Lead Developer 👨‍💻

---

## **License 📜**

This project is licensed under the **MIT License**.

---
