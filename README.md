# Task #04 - User Management System

## Overview
Full-stack **User Management Web Application** with registration, authentication, and admin panel.  
Built as per iTransition Task #04 requirements.

## Tech Stack
- **Backend**: ASP.NET Core 8 + EF Core + PostgreSQL
- **Frontend**: React + Vite + TypeScript + Bootstrap
- **Authentication**: JWT
- **Database**: PostgreSQL (Render)
- **Deployment**: Render.com

## Features Implemented
- User Registration & Login
- Email verification (basic link)
- User Management (Block, Unblock, Delete, Delete Unverified)
- Protected routes with JWT
- Blocked user middleware (Requirement 5)
- Unique index on Email in database
- Responsive dashboard with checkboxes & toolbar
- Multiple selection + Select All
- Table sorted by Last Login Time

## API Documentation
- Swagger UI: [https://itransition-task04-backendwebservices.onrender.com/swagger](https://itransition-task04-backendwebservices.onrender.com/swagger)
- Postman Collection: Available in repository

## How to Run Locally

1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run migrations:
   ```bash
   dotnet ef database update
