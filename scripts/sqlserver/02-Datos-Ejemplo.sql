USE TechStoreDB;
GO
SET XACT_ABORT ON; BEGIN TRAN;
IF EXISTS(SELECT 1 FROM dbo.Categories) THROW 50001,N'Los datos iniciales ya fueron cargados.',1;
INSERT dbo.Categories(Name,Description,IsActive) VALUES(N'Computación',N'Equipos informáticos',1),(N'Periféricos',N'Dispositivos de entrada y salida',1),(N'Telefonía',N'Teléfonos y comunicación',1),(N'Accesorios',N'Accesorios tecnológicos',1);
INSERT dbo.Branches(Name,Address,Phone,IsActive) VALUES(N'Casa Central',N'Av. Tecnología 100',N'11-4000-1000',1),(N'Sucursal Norte',N'Belgrano 450',N'11-4000-2000',1),(N'Sucursal Sur',N'Mitre 800',N'11-4000-3000',1);
INSERT dbo.Products(Code,Name,Description,CategoryId,Price,IsActive,CreatedAt,UpdatedAt) VALUES(N'TS-001',N'Notebook',N'Notebook empresarial',1,850000,1,SYSDATETIME(),SYSDATETIME()),(N'TS-002',N'Monitor',N'Monitor LED',1,240000,1,SYSDATETIME(),SYSDATETIME()),(N'TS-003',N'Teclado',N'Teclado USB',2,45000,1,SYSDATETIME(),SYSDATETIME()),(N'TS-004',N'Mouse',N'Mouse óptico',2,25000,1,SYSDATETIME(),SYSDATETIME()),(N'TS-005',N'Teléfono',N'Teléfono móvil',3,420000,1,SYSDATETIME(),SYSDATETIME());
INSERT dbo.Customers(DocumentNumber,BusinessName,Email,CustomerType,DiscountPercentage,CurrentAccountBalance,IsActive,CreatedAt) VALUES(N'30700000011',N'Cliente Minorista',N'minorista@correo.com',0,0,0,1,SYSDATETIME()),(N'30700000022',N'Cliente Mayorista',N'mayorista@correo.com',1,10,0,1,SYSDATETIME());
INSERT dbo.Sellers(Name,DocumentNumber,Email,IsActive) VALUES(N'Fernando',N'LEG-0001',N'fernando@techstore.com',1),(N'Lucía Pérez',N'LEG-0002',N'lucia@techstore.com',1),(N'Martín Suárez',N'LEG-0003',N'martin@techstore.com',1);
INSERT dbo.Inventories(ProductId,BranchId,Stock,MinimumStock) SELECT p.Id,b.Id,20,5 FROM dbo.Products p CROSS JOIN dbo.Branches b;
COMMIT;
GO
