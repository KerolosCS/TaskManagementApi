# 🗂️ Task Management API - ASP.NET Core (Repository Pattern)

A RESTful API built with ASP.NET Core for managing tasks, boards, users, and comments.
🚀 **Live API:** [Hosted on MonsterASP.net](🔗_https://task-management-api.runasp.net/index.html_)
---

## 🚀 Features

* 🔐 JWT Authentication & Authorization
* 👥 User Management
* 📋 Boards & Tasks Management
* 💬 Comments System (linked to Users & Tasks)
* ✅ Model Validation (Data Annotations)
* ⚙️ Global Exception Handling (ProblemDetails)
* 📦 Unified API Response Using Result Filters
* 📄 Swagger UI for API testing

---

## 🏗️ Tech Stack

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* Swagger / OpenAPI
* Repository Pattern for clean data access abstraction, improved testability, and maintainability

---

## ⚙️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/KerolosCS/TaskManagementApi.git
cd TaskManagementApi
```

---

### 2. Configure Database

Update your connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TaskDb;Trusted_Connection=True;"
}
```

---

### 3. Run Migrations

```bash
dotnet ef database update
```

---

### 4. Run the Application

```bash
dotnet run
```

---

### 5. Open Swagger

```
https://localhost:xxxx/
```

---

## 🔐 Authentication

This API uses JWT Bearer Authentication.


1. Register User
2. Login → access Token
3. use token in swagger

```
Authorization: Bearer YOUR_TOKEN
```

---

## 📌 API Endpoints

### 🧑 Users

* `POST /api/auth/register`
* `POST /api/auth/login`


### 💬 Boards
* `GET /api/boards/{boardId}`
* `POST /api/boards`
* `PUT /api/boards/{boardId}`
* `Delete /api/boards/{boardId}`
* `GET /api/boards/{boardId}`

### 📋 Tasks

* `GET /api/tasks/board/{boardId}`
* `POST /api/tasks`
* `PUT /api/tasks/{taskId}`
* `Delete /api/tasks/{taskId}`

### 💬 Comments

* `POST /api/comments`
* `PUT /api/comments/{commentId}`
* `GET /api/comments/task/{taskId}`
* `DELETE /api/comments/{commentId}`

---

## 🧪 Sample Response

### ✅ Success

```json
{
  "success": true,
  "message": "Success",
  "data": {}
}
```

---

### ❌ Error (ProblemDetails)

```json
{
  "type": "NotFoundException",
  "title": "Not Found",
  "status": 404,
  "detail": "Resource not found",
  "traceId": "..."
}
```

---

## 🧠 Key Concepts Implemented

* Result Filters for response wrapping
* Global Exception Handling `IExceptionHandler`
* Clean separation between DTOs & Models
* Secure endpoints  `[Authorize]`

---

## 👨‍💻 Author

**Kerolos Fady**

---

## ⭐ Support

If you like this project, consider giving it a ⭐ on GitHub!
