USE TechStoreDB;
GO
SELECT @@SERVERNAME Servidor,DB_NAME() BaseActual,SUSER_SNAME() UsuarioActual;
SELECT N'Categories' Tabla,COUNT(*) Registros FROM dbo.Categories UNION ALL SELECT N'Products',COUNT(*) FROM dbo.Products UNION ALL SELECT N'Branches',COUNT(*) FROM dbo.Branches UNION ALL SELECT N'Inventories',COUNT(*) FROM dbo.Inventories UNION ALL SELECT N'Customers',COUNT(*) FROM dbo.Customers UNION ALL SELECT N'Sellers',COUNT(*) FROM dbo.Sellers UNION ALL SELECT N'Sales',COUNT(*) FROM dbo.Sales;
SELECT p.Code,p.Name,b.Name Sucursal,i.Stock,i.MinimumStock FROM dbo.Inventories i JOIN dbo.Products p ON p.Id=i.ProductId JOIN dbo.Branches b ON b.Id=i.BranchId ORDER BY p.Code,b.Name;
GO
