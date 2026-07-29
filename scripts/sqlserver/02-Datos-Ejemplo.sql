USE TechStoreDB;
GO
INSERT dbo.Sucursales(Nombre,Direccion,Telefono) VALUES(N'Casa Central',N'Av. Tecnología 100',N'11-4000-1000'),(N'Sucursal Centro',N'Belgrano 450',N'11-4000-2000'),(N'Sucursal Norte',N'Mitre 800',N'11-4000-3000');
INSERT dbo.Categorias(Nombre,Descripcion) VALUES(N'Computación',N'Equipos informáticos'),(N'Periféricos',N'Dispositivos de entrada y salida'),(N'Telefonía',N'Teléfonos y comunicación'),(N'Accesorios',N'Accesorios tecnológicos');
INSERT dbo.Productos(Codigo,Descripcion,IdCategoria,Precio) VALUES(N'TS-001',N'Notebook',1,850000),(N'TS-002',N'Monitor',1,240000),(N'TS-003',N'Teclado',2,45000),(N'TS-004',N'Mouse',2,25000),(N'TS-005',N'Teléfono',3,420000);
INSERT dbo.Clientes(CUIT,NombreRazonSocial,Email,DescuentoPorcentaje) VALUES(N'30-70000001-1',N'Cliente Minorista',N'minorista@correo.com',0),(N'30-70000002-2',N'Cliente Mayorista',N'mayorista@correo.com',10);
INSERT dbo.Vendedores(Nombre,Legajo,IdSucursal,Activo) VALUES(N'Fernando',N'LEG-0001',1,1),(N'Lucía Pérez',N'LEG-0002',2,1),(N'Martín Suárez',N'LEG-0003',3,1);
INSERT dbo.StockSucursal(IdSucursal,CodigoProducto,Stock,StockMinimo) SELECT s.IdSucursal,p.Codigo,20,5 FROM dbo.Sucursales s CROSS JOIN dbo.Productos p;
GO
