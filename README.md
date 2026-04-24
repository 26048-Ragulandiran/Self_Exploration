
Create the .env file and add these,

 - DB_SQL_CONNECTION=sql_connection_uri

Execute these commands before running,

For migration,
 - dotnet ef migrations add ItineraryMigration
 - dotnet ef database update

	
Add or install these packages or just build to install automatically through nuget, 

 - Serilog
 - FluentValidation
 - EntityFramework
 - Pomelo [MySQL]
 - Swashbuckle [Swagger]