# TODO App - Backend

This is the backend API for the TODO App, designed to manage tasks through CRUD operations. Built with **.NET 8** and **SQLite**, this API efficiently handles requests from the frontend, ensuring data integrity and seamless functionality. Below is an overview of the backend setup, features, and technologies used.

## Table of Contents

1. **Technologies Used**
2. **Features**
3. **Project Structure**
4. **Setup and Installation**
5. **API Endpoints**
6. **Error Handling and Validation**
7. **Testing**
8. **Deployment**
9. **Contributing**

---

## Technologies Used

- **.NET 8** - Core framework for building and running the backend application.
- **SQLite** - Lightweight, serverless SQL database for storing tasks.
- **Docker & Docker Compose** - Used for containerizing the application, facilitating deployment, and managing multi-container setups.
- **ECS** (Optional) - For deploying Docker containers on AWS.
- **Entity Framework Core 8** - ORM for database operations.
- **Swashbuckle (Swagger)** - For API documentation.
- **Moq** - For unit testing and mocking dependencies.

## Features

- **CRUD Operations**: Create, Read, Update, Delete tasks for users.
- **Input Validation**: Enforces data integrity by validating inputs.
- **Error Handling**: Returns meaningful HTTP status codes (e.g., 200, 201, 400, 404, 500).
- **RESTful API Design**: Follows REST principles for intuitive usage.
- **Containerization**: Uses Docker and ECS for deployment and scalability.

## Project Structure

```plaintext
TodoApp.Backend/
├── Controllers       # Contains API controllers for handling requests
├── Models            # Defines `TodoItem` model and data annotations for validation
├── Data              # Configures the database context and migration setup for SQLite
├── Migrations        # Holds database migration files
├── Services          # Contains business logic and service layers if applicable
├── Dockerfile        # Docker configuration for containerization
├── appsettings.json  # Configuration file for connection strings, logging, and environment settings
└── Program.cs        # Main entry point for the application

```
# Setup and Installation

## Prerequisites

- **.NET SDK 8**
- **Docker and Docker Compose**

## Steps

1. **Clone the repository**:
   ```bash
   git clone https://github.com/AlejoAndrade89/TodoApp-Backend.git
2. Navigate to the backend folder:
   ```bash
   cd TodoApp-Backend

3. Set up the database (SQLite):
   ```bash
   dotnet ef database update

4. Run the application:
   ```bash
   dotnet run

5. (Optional) Docker Setup:
   ```bash
   docker-compose up --build

   The server should now be running on http://localhost:5000 (or Docker’s configured port).
   API Endpoints
# Method	Endpoint	Description
* GET	/api/todos	Retrieves all tasks
* GET	/api/todos/{id}	Retrieves a specific task by ID
* POST	/api/todos	Creates a new task
* PUT	/api/todos/{id}	Updates an existing task
* DELETE	/api/todos/{id}	Deletes a task by ID
  
  Each endpoint returns appropriate HTTP status codes for successful or failed operations.

  Error Handling and Validation
  The API incorporates standardized error handling:

* Validation Errors (400): Input data issues are flagged with 400 Bad Request.
* Not Found (404): Returned when a resource is missing.
* Internal Server Errors (500): Unhandled exceptions produce a 500 Internal Server Error.
* Data annotations in the TodoItem model enforce field-specific validation.

# Testing
Unit tests are provided for API endpoints to ensure consistent functionality. Tests are defined using the Xunit framework with Moq for mocking dependencies.
To run tests:
```bash
dotnet test
```
## Deployment
  The backend can be containerized using Docker and deployed via ECS on AWS. Refer to the Dockerfile and docker-compose.yml for detailed deployment configurations.

## Contributing
  We welcome contributions! Please create a pull request for any bug fixes, features, or improvements.




