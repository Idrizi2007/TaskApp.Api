# TaskApp API

A secure and structured ASP.NET Core Web API for managing tasks with authentication, authorization, and ownership enforcement.

---

## Overview

TaskApp API is a backend service that allows users to:

- Register and authenticate using JWT
- Create and manage personal tasks
- Mark tasks as completed
- Retrieve paginated task lists
- Refresh authentication tokens
- Enforce role-based and ownership-based access control

The system ensures that users can only access their own tasks, while administrators have elevated access.

---

## Architecture

The project follows a layered architecture:

Controllers → Services → Domain → Persistence

- **Controllers** handle HTTP requests and responses  
- **Services** contain business logic  
- **Domain** contains entities and core models  
- **Persistence** handles database access via Entity Framework Core  

DTOs are used to isolate API contracts from domain models.

---

## Authentication & Authorization

The API uses **JWT (JSON Web Tokens)** for authentication.

### Access Token
- Signed using HMAC SHA256
- Contains:
  - User ID
  - Email
  - Role
- Short-lived (configurable)

### Refresh Token
- Secure random token
- Stored in database
- Rotated on refresh
- Revoked on logout

### Authorization Model

- `[Authorize]` protects endpoints
- Role-based authorization supported
- Ownership enforcement ensures:
  - Users can only access their own tasks
  - Admins can access all tasks

---

## Core Features

### User Management
- Register
- Login
- Refresh token
- Logout

### Task Management
- Create task
- Get paginated task list
- Get task by ID
- Mark task as completed
- Ownership validation on all operations

---

## Validation

Input validation is handled using FluentValidation.

- DTO validation rules are separated from controllers
- Automatic validation via ASP.NET Core pipeline

---

## Error Handling

Global exception handling middleware ensures:

- Consistent API error responses
- Clean controller logic
- Centralized error formatting

---

## Database

- SQL Server
- Entity Framework Core
- Code-first migrations
- Owned entity for RefreshToken

---

## Technologies Used

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- BCrypt password hashing
- FluentValidation
- Swagger (OpenAPI)

---

## API Endpoints

### Authentication

POST   /auth/register  
POST   /auth/login  
POST   /auth/refresh  
POST   /auth/logout  

### Tasks

GET    /tasks  
GET    /tasks/{id}  
POST   /tasks  
PATCH  /tasks/{id}/complete  

All task endpoints require authentication.

---

## Configuration

JWT settings are defined in `appsettings.json`:

```json
"Jwt": {
  "Key": "your-secret-key",
  "Issuer": "TaskApp.Api",
  "Audience": "TaskApp.Api",
  "AccessTokenMinutes": 10,
  "RefreshTokenDays": 7
}
```

---

## Running the Project

1. Install .NET 8 SDK  
2. Configure connection string  
3. Apply migrations:

```
dotnet ef database update
```

4. Run:

```
dotnet run
```

Swagger UI will be available at:

```
http://localhost:{port}/swagger
```

---

## Design Goals

- Clean separation of concerns
- Secure authentication flow
- Clear ownership model
- Minimal controller logic
- Structured service layer
- Extensible architecture
