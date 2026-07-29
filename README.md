# TechStore S.A. — Sistema de Gestión de Ventas

Aplicación Windows Forms para productos, categorías, clientes, sucursales, inventario, vendedores, ventas, facturas, cuenta corriente, anulaciones y reportes. **Toda la persistencia productiva está centralizada en Microsoft SQL Server**.

## Requisitos

- Windows 10/11 y Visual Studio 2022 o SDK .NET 8.
- SQL Server y acceso mediante SSMS.
- Servidor configurado: `LAUTI`.
- Base de datos: `TechStoreDB`.
- Autenticación: Windows Authentication (`LAUTI\lauta`, según la sesión mostrada en SSMS).

## Preparar SQL Server desde SSMS

Conéctese al servidor `LAUTI` con **Windows Authentication**. Abra y ejecute desde la raíz del repositorio, en este orden:

1. `scripts/sqlserver/01-Crear-TechStoreDB.sql`: crea la base, tablas, relaciones, índices y restricciones exactas que consume la aplicación.
2. `scripts/sqlserver/02-Datos-Ejemplo.sql`: carga categorías, sucursales, productos, clientes, vendedores e inventario inicial.
3. `scripts/sqlserver/05-Verificar-Instalacion.sql`: confirma servidor, base, usuario, cantidades e inventario.

Si tiene `sqlcmd` instalado, puede realizar los tres pasos automáticamente haciendo doble clic en `INSTALAR-DB-SQLSERVER.cmd`.

Scripts adicionales:

- `03-Registrar-Venta.sql`: ejemplo transaccional de una venta con control y descuento de stock.
- `04-Reportes.sql`: ventas, productos más vendidos y cuentas corrientes.
- `99-Recrear-TechStoreDB.sql`: elimina completamente la base para reiniciar desarrollo. **No ejecutar en producción.**

## Conexión de la aplicación

La cadena está en `TechStore.App/appsettings.json`:

```text
Server=LAUTI;Database=TechStoreDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True
```

Para cambiarla sin modificar archivos, defina `TECHSTORE_CONNECTION_STRING`; esta variable tiene prioridad. No se crea ni se utiliza `techstore.db`: altas, modificaciones, inventario, ventas y cuentas corrientes se guardan en SQL Server. En una base vacía la aplicación puede crear y sembrar automáticamente el esquema, aunque para una instalación verificable se recomienda usar los scripts anteriores.

## Compilar y ejecutar

Desde la carpeta que contiene `TechStore.sln`, puede hacer doble clic en `INICIAR-TECHSTORE.cmd` o ejecutar:

```bat
.\INICIAR-TECHSTORE.cmd
```

El iniciador valida el proyecto y después restaura dependencias/herramientas,
elimina binarios antiguos, actualiza SQLite, ejecuta las pruebas e inicia la
aplicación recién compilada. Esto evita abrir por error una versión anterior que
todavía muestre el mensaje informativo en lugar de los formularios ABM.

La alternativa manual es:

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project TechStore.App
```

Si falla la conexión, verifique que SQL Server esté iniciado, que `LAUTI` sea alcanzable, que su usuario de Windows tenga acceso y que `TechStoreDB` exista. La aplicación muestra el error original de SQL Server y la ubicación donde se configura la conexión.

## Verificación del proyecto correcto

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\Verificar-Entorno.ps1
```

La salida esperada indica `TechStore.App / net8.0-windows / EF Core SQL Server`. En Visual Studio establezca **TechStore.App** como proyecto de inicio. No ejecute proyectos antiguos `net472` ni ejecutables conservados en otras carpetas.

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

## Arquitectura y módulos

Productos, categorías, clientes, sucursales, inventario, vendedores, nueva venta, historial/anulación, factura e impresión, pagos de cuenta corriente, reportes e indicadores de inicio. Los catálogos permiten alta, edición, búsqueda y activación/desactivación real; las altas de productos y sucursales crean automáticamente las filas de inventario faltantes con stock cero. Las bajas son lógicas y las operaciones financieras/de stock usan transacciones.
