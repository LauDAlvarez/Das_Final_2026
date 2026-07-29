@echo off
setlocal
cd /d "%~dp0"
where sqlcmd >nul 2>nul || goto :no_sqlcmd
echo Creando TechStoreDB en LAUTI con autenticacion de Windows...
sqlcmd -S LAUTI -E -b -i "scripts\sqlserver\01-Crear-TechStoreDB.sql" || goto :failed
echo Cargando datos iniciales...
sqlcmd -S LAUTI -E -b -i "scripts\sqlserver\02-Datos-Ejemplo.sql" || goto :failed
echo Verificando instalacion...
sqlcmd -S LAUTI -E -b -i "scripts\sqlserver\05-Verificar-Instalacion.sql" || goto :failed
echo.
echo TechStoreDB quedo instalada correctamente. Ya puede ejecutar INICIAR-TECHSTORE.cmd.
pause
exit /b 0

:no_sqlcmd
echo ERROR: sqlcmd no esta instalado o no figura en PATH.
echo Puede ejecutar manualmente los archivos 01, 02 y 05 desde SSMS.
pause
exit /b 2

:failed
echo ERROR: SQL Server rechazo uno de los scripts. Revise el mensaje anterior.
pause
exit /b 1
