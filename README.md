# QuizMaker

QuizMaker is a monolithic application built following **Clean Architecture** principles, separating concerns into distinct layers while maintaining a cohesive project structure. The project provides a quiz management system with an API backend.

---

## **Project Structure**

- **QuizMaker.Api** – API layer exposing HTTP endpoints
- **QuizMaker.Application** – Validations, Custom Exceptions, DTOs and service interfaces.
- **QuizMaker.Domain** – Core entities
- **QuizMaker.Infrastructure** – Database context, migrations and service implementations.

The architecture ensures a clear separation of concerns, testability, and maintainability.

---

## **🔌 Extensibility & Plug-in Architecture**

The application features a **dynamic plug-in system** using **MEF (Managed Extensibility Framework)**. This allows the API to support new export formats without being recompiled or modified.



### **How it works:**
- The API scans the `Exporters/` folder at startup for `.dll` files implementing the `IQuizExporter` interface.
- New formats can be added by simply placing a compiled DLL into the folder and restarting the service.
- **No recompilation** of the main API project is required to add new functionality.

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
   ```
2. Adjust the connection string in appsettings.Development.json to point to your local SQL Server
3. Run Api project (QuizMaker.Api)
4. Access SwaggerAccess Swagger UI:
-  HTTPS: https://localhost:5001/swagger/index.html
-  HTTP: http://localhost:5000/swagger/index.html

### **2. Docker Setup (Recommended for Testing Plugins)**

Run QuizMaker using Docker with SQL Server and Seq logging. This setup demonstrates the ability to load plugins via **Docker Volumes** without rebuilding the API image.

**Requirements:**
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (Linux containers)
- Microsoft SQL Server instance
- Ports available: `5000`, `5001`, `5341`, `1433`

**Steps:**

1. **Clone the repository** and navigate to the folder containing `docker-compose.yml`.

2. Adjust the connection string in `docker-compose.yml`.
   > **Note on Database:** The default connection string uses `host.docker.internal` to connect to a local SQL Server instance. Ensure your SQL Server allows TCP/IP connections and that the user provided has sufficient permissions to create the database and run migrations.

3. **Prepare Plugins:** Build the desired exporter project (e.g., `QuizMaker.Exporters.Csv`) in Visual Studio. This will trigger the build task that copies the DLL to the API's exporter folder.

4. **Start the containers:**
   - **Visual Studio:** Open the solution, set the startup project to `docker-compose`, and press **Start**.
   - **Command Line:**
     ```bash
     docker-compose up -d
     ```

5. **Plugin Test (Step-by-Step)**

   To truly see the power of the MEF implementation and Docker volumes, follow these steps:

   - **Initial State:** Start the containers. Open Swagger and call `GET /api/quizzes/export-formats`.
     - **Result:** You will receive an empty list `[]`. This proves the API is running without any exporters pre-installed.
   - **Plug-in Injection:** In Visual Studio, right-click the **QuizMaker.Exporters.CsvExporter** project and select **Build** (or **Rebuild**).
     - *This copies the DLL to the shared Docker volume folder automatically.*
   - **Activation:** Restart only the API container to trigger a new scan:
     ```bash
     docker restart quiz-api
     ```
   - **Final Result:** Refresh Swagger and call the same endpoint again.
   - **Result:** You will now see `["CSV"]`. The API successfully discovered and loaded the new logic at runtime!

6. **Access the services:**
   - **API Swagger:** [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
   - **Seq Logs:** [http://localhost:5341/](http://localhost:5341/)

---

## **Monitoring & Logging**

The application uses **Serilog** with **Seq** for centralized logging. 
- All API requests, database migrations, and **MEF loading events** are logged.
- To view logs, open `http://localhost:5341` after starting the Docker containers.
