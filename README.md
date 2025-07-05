# School Management System

This is a simple school management system built with **.NET Core** using clean architecture and JWT authentication.

## ✅ Basic Features

| Feature                    | Role            | Description                          |
|----------------------------|-----------------|--------------------------------------|
| Register/Login             | All             | JWT-based authentication             |
| Create/Update/Delete Courses | Admin, Teacher | Teachers can manage their own courses |
| Enroll Students            | Admin           | Assign students to courses            |
| View Courses               | All             | Students see their courses only       |
| Add Assignments            | Teacher         | Linked to a course                    |
| Submit Assignment          | Student         | Mark assignment as submitted          |
| Grade Assignment           | Teacher         | Assign grades to submissions          |

## ✅ Technical Stack

- ASP.NET Core 8 (Latest LTS)
- Entity Framework Core (Code-First)
- Clean Architecture (Controllers, Services, Repositories)
- IUnitOfWork
- JWT Authentication
- Serilog Logging (Console Logging)
- Paging, Filtering, and Sorting in APIs
- Input Validation using FluentValidation (AutoValidation enabled)
- Basic Unit Tests (xUnit)

## ✅ Setup Instructions

1. Clone the repository:
```bash
git clone https://github.com/yaelsoliman/School_Management_System.git

## ✅ Admin Seeder (Default User)

- **Email:** `admin@gmail.com`
- **Password:** `Passw0rd@123`

## ✅ Authentication Flow

1. You can **register a new user without a token** using the registration endpoint.
2. After registration, you can **login using the new account** to obtain a JWT token.
3. To access protected endpoints:
   - Go to the **Authorization** section in Swagger.
   - Add the token in the following format:
     ```
     Bearer YOUR_TOKEN_HERE
     ```

---
🗄️ Database
Migrations are already applied in the project.

No need to run Add-Migration or Update-Database.

When you start the project, the database will be automatically created and seeded (Admin user will be created). 
