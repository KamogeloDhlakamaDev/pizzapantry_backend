# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and project folders
COPY pizzapantry_backend.sln ./
COPY src/pizzapantry_backend.Application ./src/pizzapantry_backend.Application
COPY src/pizzapantry_backend.Domain ./src/pizzapantry_backend.Domain
COPY src/pizzapantry_backend.Infrastructure ./src/pizzapantry_backend.Infrastructure
COPY src/pizzapantry_backend.WebApi ./src/pizzapantry_backend.WebApi

WORKDIR /app/src/pizzapantry_backend.WebApi
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/src/pizzapantry_backend.WebApi/out ./

ENV PORT=5000
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

ENTRYPOINT ["dotnet", "pizzapantry_backend.WebApi.dll"]




