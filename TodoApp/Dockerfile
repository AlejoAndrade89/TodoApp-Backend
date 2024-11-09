# Usar la imagen base de .NET SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivo .csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código y construir la aplicación
COPY . .
RUN dotnet publish -c Release -o out

# Imagen final para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "TodoApp.dll"]
