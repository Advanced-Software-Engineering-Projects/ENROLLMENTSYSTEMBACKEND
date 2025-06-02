### 💻 Backend Setup (ASP.NET Core with Visual Studio) ###

1. Open Solution File  
   Open the file `ENROLLMENTSYSTEMBACKEND.sln` in Visual Studio 2022 or newer.

2. Set Startup Project  
   - In the Solution Explorer, right-click on `ENROLLMENTSYSTEMBACKEND` and select Set as Startup Project.

3. Restore NuGet Packages  
   - In Tools > NuGet Package Manager > Manage NuGet Packages for Solution, click Restore.
   - Or use the terminal:
     bash
     dotnet restore
     

4. Configure Connection String  
   - Open `appsettings.json`
   - Update the connection string under `ConnectionStrings:DefaultConnection` to match your SQL Server setup:
     json
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=EnrollmentSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
     
     > Replace `YOUR_SERVER_NAME` with `localhost`, `localhost\SQLEXPRESS`, or your actual SQL Server instance.

5. Apply Database Migrations  
   Open the Package Manager Console (`Tools > NuGet Package Manager > Package Manager Console`) and run:
   powershell
   Update-Database
   
   > This command applies all pending migrations and creates the database if it doesn’t exist.

6. Run the Backend  
   - Press `F5` or `Ctrl+F5` to start the backend.
   - A browser window will open with the Swagger UI to interact with your API.

---

### 🗄 Database Configuration (SQL Server)

- Ensure SQL Server (LocalDB) or SQL Server Express is installed and running.
- Confirm that `EnrollmentSystemDb` is created after running `Update-Database`.
- You can verify and manage the database using:
  - SQL Server Management Studio (SSMS)
  - Azure Data Studio

> 💡 Tip: The database schema and relationships are defined using Entity Framework Core Code First Migrations.
