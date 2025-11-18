# User Management API

Minimal ASP.NET Core Web API for managing users with authentication, logging, and error handling middleware.

## Features

- **CRUD Operations**: Create, Read, Update, Delete users
- **Input Validation**: Email format, required fields, duplicate checks
- **Middleware Pipeline**:
  - Error handling (catches all exceptions)
  - Token authentication (API key validation)
  - Request/response logging
- **In-memory storage** (no database required)

## API Endpoints

All endpoints require authentication header: `Authorization: Bearer techhive-api-key-2024`

### GET /api/user
Get all users

### GET /api/user/{id}
Get user by ID

### POST /api/user
Create new user
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "phone": "1234567890"
}
```

### PUT /api/user/{id}
Update existing user
```json
{
  "name": "John Updated",
  "email": "john@example.com",
  "phone": "1234567890"
}
```

### DELETE /api/user/{id}
Delete user by ID

## Run Locally

```bash
dotnet restore
dotnet run
```

Visit: http://localhost:5000/swagger

## Deploy to Railway

Railway will automatically detect the .NET project and deploy it.

## Authentication

Use the API key in Authorization header:
```
Authorization: Bearer techhive-api-key-2024
```

## Project Structure

```
backend/
├── Controllers/
│   └── UserController.cs       # CRUD endpoints
├── Middleware/
│   ├── ErrorHandlingMiddleware.cs
│   ├── AuthenticationMiddleware.cs
│   └── LoggingMiddleware.cs
├── Models/
│   └── User.cs                 # User model with validation
├── Program.cs                  # App setup & middleware pipeline
├── UserManagementAPI.csproj
└── appsettings.json
```

