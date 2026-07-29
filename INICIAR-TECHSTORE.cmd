@echo off
setlocal
cd /d "%~dp0"

if not exist "TechStore.sln" goto :wrong_folder
if not exist "TechStore.App\TechStore.App.csproj" goto :wrong_folder

findstr /C:"<TargetFramework>net8.0-windows</TargetFramework>" "TechStore.App\TechStore.App.csproj" >nul || goto :wrong_project
findstr /C:"Microsoft.EntityFrameworkCore.Sqlite" "TechStore.App\TechStore.App.csproj" >nul || goto :wrong_project

echo [1/6] Limpiando compilaciones anteriores para evitar abrir una version vieja...
if exist "TechStore.App\bin" rmdir /s /q "TechStore.App\bin"
if exist "TechStore.App\obj" rmdir /s /q "TechStore.App\obj"
if exist "TechStore.Tests\bin" rmdir /s /q "TechStore.Tests\bin"
if exist "TechStore.Tests\obj" rmdir /s /q "TechStore.Tests\obj"
echo [2/6] Restaurando dependencias...
dotnet restore TechStore.sln || goto :failed
echo [3/6] Restaurando dotnet-ef...
dotnet tool restore || goto :failed
echo [4/6] Aplicando migraciones SQLite...
dotnet ef database update --project TechStore.App || goto :failed
echo [5/6] Compilando y ejecutando pruebas...
dotnet test TechStore.sln || goto :failed
echo [6/6] Iniciando TechStore.App actualizado...
dotnet run --project TechStore.App --no-build
exit /b %errorlevel%

:wrong_folder
echo ERROR: Este archivo debe estar junto a TechStore.sln y la carpeta TechStore.App.
echo Si el archivo no estaba disponible, primero actualice o vuelva a clonar el repositorio.
exit /b 2

:wrong_project
echo ERROR: TechStore.App no es la entrega .NET 8 con EF Core SQLite.
echo No inicie el proyecto legado TechStore de net472 y SQL Server.
exit /b 3

:failed
echo ERROR: El paso anterior no pudo completarse. Revise el mensaje mostrado arriba.
exit /b 1
