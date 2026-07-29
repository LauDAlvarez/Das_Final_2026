# TechStore S.A. — Sistema de Gestión de Ventas

Aplicación de escritorio nativa para Windows con persistencia SQLite. Incluye inicio operativo, catálogos, inventario, clientes, vendedores, ventas transaccionales, factura imprimible, cuenta corriente, anulación y reportes.

## Tecnologías y requisitos

C# 12, .NET 8, Windows Forms, Entity Framework Core 8, SQLite, LINQ y xUnit. Requiere Windows 10/11 y SDK .NET 8; `dotnet-ef` es necesario para administrar migraciones.

## Restaurar, crear, compilar y ejecutar

```bash
dotnet restore
dotnet tool restore
dotnet ef database update --project TechStore.App
dotnet build
dotnet test
dotnet run --project TechStore.App
```

La aplicación crea e inicializa automáticamente `techstore.db` junto al ejecutable en el primer inicio. La carga inicial contiene 4 categorías, 10 productos, 3 sucursales con inventario, 6 clientes, 3 vendedores, 5 ventas y cuentas corrientes.

> Ejecute todos los comandos desde la carpeta que contiene `TechStore.sln`,
> `TechStore.App` y `TechStore.Tests`. `dotnet tool restore` instala localmente
> `dotnet-ef`; no hace falta instalar una herramienta global. Si la terminal está
> dentro de `TechStore.App`, vuelva primero a la raíz con `cd ..`.

La migración inicial está incluida en el repositorio. Tanto
`dotnet ef database update --project TechStore.App` como el primer inicio de la
aplicación ejecutan las migraciones pendientes de manera segura. No se debe crear
manualmente el archivo SQLite.

## Diagnóstico de una solución distinta

Este repositorio usa los espacios de nombres `TechStore.App.Models` y las clases
en inglés `Branch` y `Seller`. Si Visual Studio informa errores en
`TechStore.Models.Entities.Sucursal`, `Data/Migrations/Configuration.cs` o en un
proyecto llamado `TechStore.csproj`, se está compilando otra solución. Cierre esa
solución y abra específicamente `TechStore.sln` desde esta carpeta.

## Estructura

`TechStore.App` contiene `Models`, `Views`, `Controllers`, `Services`, `Data`, `DTOs`, `Enums` y `Migrations`. `TechStore.Tests` prueba reglas críticas con una base SQLite aislada en memoria. `Docs/Formularios` describe las pantallas.

## Módulos

Productos, categorías, clientes, sucursales, inventario, vendedores, nueva venta, historial/anulación, factura e impresión, pagos de cuenta corriente, reportes e indicadores de inicio. Las bajas son lógicas y las operaciones financieras/de stock usan transacciones.
