````    
    dotnet ef migrations add "Intial_v1" --project src/pizzapantry_backend.Infrastructure --startup-project src/pizzapantry_backend.WebApi
    dotnet ef database update --project src/pizzapantry_backend.Infrastructure --startup-project src/pizzapantry_backend.WebApi
    dotnet ef migrations remove --project src/pizzapantry_backend.Infrastructure --startup-project src/pizzapantry_backend.WebApi
````    
