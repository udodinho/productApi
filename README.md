# Product is an ASP.NET Core 6.0 application that demonstrates a CRUD (Create, Read, Update, Delete) functionality with user authentication. The project uses Entity Framework Core for database operations and JWT for authentication.


### Before you begin, ensure you have met the following requirements:
1. .NET 6 SDK
2. PostgreSQL

### Usage
3. Clone the repo
4. Setup your database

```
appsettings.json
```

```
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=your_database_name;User ID=your_username;Password=your_password;"
  },
```

5. Apply migrations
```
dotnet ef database update
```

6. Run the application
- To make sure there is no error run this command first
```
dotnet build
```
then
```
dotnet run
```

7. Accessing the application
Once the application is running, open your web browser and navigate to:

```
https://localhost:7028/swagger/index.html
```