# QuizMaker

QuizMaker is a monolithic application built following **Clean Architecture** principles, separating concerns into distinct layers while maintaining a cohesive project structure. The project provides a quiz management system with an API backend.

---

## **Project Structure**

- **QuizMaker.Api** – API layer exposing HTTP endpoints
- **QuizMaker.Application** – Validations, Custome Exceptions, DTOs and service interfaces.
- **QuizMaker.Domain** – Core entities
- **QuizMaker.Infrastructure** – Database context, migrations and service implementations.

The architecture ensures a clear separation of concerns, testability, and maintainability.

---

## **Running the Application**

### **1. API (Local .NET Development)**

**Requirements:**

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Microsoft SQL Server instance

**Steps:**

1. Clone the repository:

   ```bash
   git clone <repository-url>
   cd QuizMaker
2. Open QuizMaker.Api project
3. Adjust the connection string in appsettings.Development.json to point to your local SQL Server
4. Run Api project
5. Access SwaggerAccess Swagger UI:
-  HTTPS: https://localhost:5001/swagger/index.html
-  HTTP: http://localhost:5000/swagger/index.html

### **2. Docker Setup**

Run QuizMaker using Docker with SQL Server and Seq logging.

**Requirements:**

- [Docker Desktop](https://www.docker.com/products/docker-desktop) (Linux containers)
- Microsoft SQL Server instance
- Ports available:
  - API: `5000` (HTTP), `5001` (HTTPS)
  - Seq: `5341` (HTTP)

**Steps:**

1. Adjust the connection string in `docker-compose.yml`:
2. Start the containers:
-  Open Visual studio, make startup project Docker-compose and press Start or using bash navigate to folder containing docker-compose.yml and run `docker-compose up -d`
3. Access the services:
API:
-  HTTPS: https://localhost:5001/swagger/index.html
-  HTTP: http://localhost:5000/swagger/index.html
Seq logs:
-  http://localhost:5341/
