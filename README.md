# BlogAPI
By Andrés Avila
## Local Running steps
* Download and install the [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.405-windows-x64-installer)

* Download and install [SQL Server 2019 Developer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

* The default database connection string under **appsetting.json** uses Trusted Authentication 

* Open a terminal window, navigate to the project root folder, and then **Blog.API/Data**

* Run the command `dotnet ef database update`

* Navigate to the project root folder, and run `dotnet build`.

* Navigate to **BlogAPI/bin/Debug/net6.0** and run `./BlogAPI.exe`

* Browse **https://localhost:5000/swagger/** 

## Azure Endpoint
This API has been deployed to **Azure** using AppService and Azure SQL Database.

The url is **https://blogapiweb.azurewebsites.net/swagger/index.html**

## Users
### Writer One
    "email": "writer@one.com",
    "password": "Writer1*"

### Editor One
    "email": "editor@one.com",
    "password": "Editor1*"

### Public
    "public": "public@one.com"
    "password": "Public1*"

## Person Types

|ID|Person Type|
|----|-----|
|1|Writer|
|2|Editor|
|3|Public|

Time to develop: 24 hours