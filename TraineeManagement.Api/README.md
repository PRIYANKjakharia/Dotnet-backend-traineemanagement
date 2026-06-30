### Trainee Management API
 
# Overview
 
Trainee Management API is a backend training management system built using ASP.NET Core Web API and Entity Framework Core.
 
# The application supports:
 
- Trainee Management
- Mentor Management
- Learning Task Management
- Task Assignment Workflow
- Submission Tracking
- Review Management
- JWT Authentication & Authorization
- MySQL Database Persistence
- Pagination, Filtering, and Search
- Structured Logging
- Global Exception Handling
 
---
 
# Technology Stack
 
- ASP.NET Core Web API
- C#
- Entity Framework Core
- MySQL
- JWT Authentication
- BCrypt Password Hashing
- Swagger
- Postman
 
---
 
Project Setup
 
1. Clone Repository
 
- git clone <https://github.com/PRIYANKjakharia/TraineeManagement.git>
 
2. Restore Packages
 
- dotnet restore
 
3. Configure Database
 
- Update "appsettings.json":
 
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=TraineeManagement;user=root;password=root;"
}
 
4. Apply Migrations
 
- dotnet ef database update
 
5. Run Application
 
- dotnet run
 
6. Open Swagger
 
- https://localhost:<7200>/swagger
 
---
 
## Migration Commands
 
- Create Migration:
 
- dotnet ef migrations add MigrationName
 
- Apply Migration:
 
- dotnet ef database update
 
- Remove Last Migration:
 
- dotnet ef migrations remove
 
---
 
## Authentication
 
Login API
 
POST /api/auth/login
 
Request:
 
{
  "username": "admin",
  "password": "Admin@123"
}
 
Response:
 
{
  "token": "jwt-token",
  "expiresIn": 3600
}
 
---
 
## JWT Usage
 
Include token in request headers:
 
Authorization: Bearer <jwt-token>
 
Protected APIs require a valid JWT token.
 
---
 
## Available APIs
 
# Health
 
GET /api/health
 
# Authentication
 
POST /api/auth/login
 
# Trainees
 
GET /api/trainees
GET /api/trainees/{id}
POST /api/trainees
PUT /api/trainees/{id}
DELETE /api/trainees/{id}
 
# Mentors
 
GET /api/mentors
GET /api/mentors/{id}
POST /api/mentors
PUT /api/mentors/{id}
DELETE /api/mentors/{id}
 
# Learning Tasks
 
GET /api/learning-tasks
GET /api/learning-tasks/{id}
POST /api/learning-tasks
PUT /api/learning-tasks/{id}
DELETE /api/learning-tasks/{id}
 
# Task Assignments
 
GET /api/task-assignments
GET /api/task-assignments/{id}
POST /api/task-assignments
PUT /api/task-assignments/{id}
DELETE /api/task-assignments/{id}
 
# Submissions
 
GET /api/submissions
GET /api/submissions/{id}
POST /api/submissions
PUT /api/submissions/{id}
DELETE /api/submissions/{id}
 
# Reviews
 
GET /api/reviews
GET /api/reviews/{id}
POST /api/reviews
PUT /api/reviews/{id}
DELETE /api/reviews/{id}
 
---
 
# Pagination Example
 
GET /api/trainees?pageNumber=1&pageSize=10&search=amit&status=Active
 
---
 
## Security Features
 
# Implemented:
 
- JWT Authentication
- JWT Authorization
- BCrypt Password Hashing
- DTO-based Data Exposure Control
- Global Exception Middleware
- Structured Logging
- Restricted CORS Configuration
- Entity Framework Core (No Raw SQL)
 
---
 
# Known Limitations
 
- Role-based authorization is not implemented.
- File uploads are not supported.
- Refresh tokens are not implemented.
- Audit history tracking is not implemented.
 
---
 
# Future Improvements
 
- React Frontend Integration
- Role-Based Access Control
- Refresh Token Support
- File Upload Support
- Dashboard & Reporting
- Notification System