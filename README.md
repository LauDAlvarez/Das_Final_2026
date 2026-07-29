# TechStore S.A. — Sistema de Gestión de Ventas

Aplicación de escritorio nativa para Windows con persistencia SQLite. Incluye inicio operativo, catálogos, inventario, clientes, vendedores, ventas transaccionales, factura imprimible, cuenta corriente, anulación y reportes.

## Tecnologías y requisitos

C# 12, .NET 8, Windows Forms, Entity Framework Core 8, SQLite, LINQ y xUnit. Requiere Windows 10/11 y SDK .NET 8; `dotnet-ef` es necesario para administrar migraciones.

## Restaurar, crear, compilar y ejecutar

Antes de ejecutar cualquier comando, confirme que su copia local contiene los
archivos `TechStore.sln`, `TechStore.App\TechStore.App.csproj`,
`INICIAR-TECHSTORE.cmd` y `scripts\Verificar-Entorno.ps1`. Si alguno no existe,
la copia local todavía no contiene esta entrega: actualice la rama correspondiente
con `git pull` o vuelva a clonar el repositorio. Ningún comando puede ejecutar un
archivo que aún no fue descargado.

La forma más sencilla en Windows es hacer doble clic en
`INICIAR-TECHSTORE.cmd`, o ejecutarlo desde la raíz:

```bat
.\INICIAR-TECHSTORE.cmd
```

El iniciador valida el proyecto y después restaura dependencias/herramientas,
actualiza SQLite, ejecuta las pruebas e inicia la aplicación.

La alternativa manual es:

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

El siguiente comando comprueba automáticamente que se abrió la entrega correcta:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\Verificar-Entorno.ps1
```

Si PowerShell indica que ese archivo no existe, no es un error de PowerShell ni
de permisos: significa que la copia local no tiene el commit que añadió el script,
o que la consola no está ubicada en la raíz de esta entrega. Compruébelo con:

```bat
dir TechStore.sln
dir TechStore.App\TechStore.App.csproj
dir scripts\Verificar-Entorno.ps1
git status -sb
git log -1 --oneline
```

No continúe ejecutando el `.exe` de la subcarpeta legado `TechStore\bin\Debug\net472`.

La salida correcta comienza con `Entorno correcto: TechStore.App / net8.0-windows /
EF Core SQLite`. Si el depurador muestra `CLR v4.0.30319`, `net472`,
`EntityFramework.dll`, `EntityFramework.SqlServer.dll`, `System.Data.SqlClient` o
tablas llamadas `Sucursals`/`Vendedors`, está ejecutando el proyecto legado de
.NET Framework 4.7.2 y SQL Server, no esta aplicación. Esta entrega carga CoreCLR
de .NET 8, `Microsoft.EntityFrameworkCore.Sqlite` y genera
`TechStore.App.exe` dentro de `TechStore.App\bin\Debug\net8.0-windows`.

En Visual Studio, haga clic derecho sobre **TechStore.App** y seleccione
**Establecer como proyecto de inicio**. Si todavía aparece `net472`, cierre Visual
Studio, elimine las carpetas `.vs`, `bin` y `obj` del proyecto legado y vuelva a
abrir solamente `TechStore.sln`.

## Estructura

`TechStore.App` contiene `Models`, `Views`, `Controllers`, `Services`, `Data`, `DTOs`, `Enums` y `Migrations`. `TechStore.Tests` prueba reglas críticas con una base SQLite aislada en memoria. `Docs/Formularios` describe las pantallas.

## Scripts SQL Server del trabajo escrito

La aplicación entregada es autocontenida y usa SQLite, pero se incluyen scripts
T-SQL equivalentes para mantener o migrar el modelo **TechStoreDB** documentado:

1. `scripts/sqlserver/01-Crear-TechStoreDB.sql`: esquema, relaciones, controles y columnas calculadas.
2. `scripts/sqlserver/02-Datos-Ejemplo.sql`: sucursales, categorías, productos, clientes, vendedores y stock.
3. `scripts/sqlserver/03-Registrar-Venta.sql`: ejemplo de alta transaccional con bloqueo y control de stock.
4. `scripts/sqlserver/04-Reportes.sql`: ventas, ranking de productos y cuenta corriente.
5. `scripts/sqlserver/99-Recrear-TechStoreDB.sql`: eliminación controlada para reiniciar un ambiente de desarrollo.

Ejecútelos en ese orden desde SSMS o con `sqlcmd`. El script `99` elimina todos
los datos y nunca debe utilizarse en producción. Los scripts T-SQL son una vía de
despliegue alternativa: no deben ejecutarse contra el archivo `techstore.db` de SQLite.

## Módulos

Productos, categorías, clientes, sucursales, inventario, vendedores, nueva venta, historial/anulación, factura e impresión, pagos de cuenta corriente, reportes e indicadores de inicio. Los catálogos permiten alta, edición, búsqueda y activación/desactivación real; las altas de productos y sucursales crean automáticamente las filas de inventario faltantes con stock cero. Las bajas son lógicas y las operaciones financieras/de stock usan transacciones.
