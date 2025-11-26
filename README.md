# Bank Management System

The **Bank Management System** is developed using **ASP.NET Core** with a **Three-Tier Architecture**, aiming to provide a professional solution for managing customers and employees in a secure and scalable environment.  
The project follows the **Repository Pattern** and a **Business Logic Layer** to separate application logic, data access, and presentation, ensuring a well-structured, maintainable, and extensible codebase.

---

## System Features

### 1. Advanced User Management
- Allows the admin to create employee accounts and assign precise permissions using a **Binary Permissions system (1, 2, 4, 8, 16)**, providing high flexibility in controlling the available actions for each employee.

### 2. Employee Permissions
Employees can perform only the banking operations allowed by their permissions, including:
- Creating customer accounts
- Editing customer information
- Deleting customers
- Deposit operations
- Withdraw operations
- Viewing the customer list

### 3. Customer Management
- The system provides a well-organized interface for managing customer data and bank accounts, with full tracking and recording of essential operations.

---

## Technical Details
- Built with **ASP.NET Core** to deliver a secure and fast web interface.
- Implemented **Three-Tier Architecture** (Presentation – Business – Data) to separate concerns.
- Developed an **Interface Layer** defining contracts for repositories and services.
- Implemented **Data Access Layer (DAL)** using Entity Framework and configuration files.
- Developed **Business Logic Layer (BLL)** to handle business rules and manage permissions.
- Adheres to modern coding standards, making the system maintainable and easily extensible.

---

## Project Highlights
This system demonstrates expertise in:
- Designing scalable software architectures
- Managing permissions using **Binary Flags**
- Structuring code with professional design patterns
- Building robust web applications with **ASP.NET Core**
- Separating responsibilities for high maintainability and clean code organization
