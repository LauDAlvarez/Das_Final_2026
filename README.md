# TechStore S.A. — Sistema de Gestión de Ventas

Aplicación de escritorio nativa para Windows con persistencia SQLite. Incluye inicio operativo, catálogos, inventario, clientes, vendedores, ventas transaccionales, factura imprimible, cuenta corriente, anulación y reportes.

## Tecnologías y requisitos

C# 12, .NET 8, Windows Forms, Entity Framework Core 8, SQLite, LINQ y xUnit. Requiere Windows 10/11 y SDK .NET 8; `dotnet-ef` es necesario para administrar migraciones.

## Restaurar, crear, compilar y ejecutar

```bash
dotnet restore
dotnet ef database update --project TechStore.App
dotnet build
dotnet test
dotnet run --project TechStore.App
```

La aplicación crea e inicializa automáticamente `techstore.db` junto al ejecutable en el primer inicio. La carga inicial contiene 4 categorías, 10 productos, 3 sucursales con inventario, 6 clientes, 3 vendedores, 5 ventas y cuentas corrientes.

## Estructura

`TechStore.App` contiene `Models`, `Views`, `Controllers`, `Services`, `Data`, `DTOs`, `Enums` y `Migrations`. `TechStore.Tests` prueba reglas críticas con una base SQLite aislada en memoria. `Docs/Formularios` describe las pantallas.

## Módulos

Productos, categorías, clientes, sucursales, inventario, vendedores, nueva venta, historial/anulación, factura e impresión, pagos de cuenta corriente, reportes e indicadores de inicio. Las bajas son lógicas y las operaciones financieras/de stock usan transacciones.
