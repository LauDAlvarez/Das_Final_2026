USE master;
GO
IF DB_ID(N'TechStoreDB') IS NOT NULL BEGIN ALTER DATABASE TechStoreDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE TechStoreDB; END;
GO
/* Después ejecute 01-Crear-TechStoreDB.sql y 02-Datos-Ejemplo.sql. */
