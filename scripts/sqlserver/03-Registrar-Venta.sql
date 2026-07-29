USE TechStoreDB;
GO
/* Ejemplo transaccional. Sustituya los valores declarados desde ADO.NET. */
DECLARE @Sucursal int=1,@Cliente int=2,@Vendedor int=1,@Codigo nvarchar(30)=N'TS-001',@Cantidad int=2,@Metodo nvarchar(30)=N'Cuenta Corriente',@Venta int,@Precio decimal(18,2),@Descuento decimal(5,2);
SET XACT_ABORT ON;
BEGIN TRAN;
SELECT @Precio=Precio FROM dbo.Productos WITH(UPDLOCK,HOLDLOCK) WHERE Codigo=@Codigo AND Activo=1;
SELECT @Descuento=DescuentoPorcentaje FROM dbo.Clientes WHERE IdCliente=@Cliente AND Activo=1;
IF @Precio IS NULL OR @Cantidad<=0 THROW 50001,N'Producto o cantidad inválidos.',1;
IF NOT EXISTS(SELECT 1 FROM dbo.StockSucursal WITH(UPDLOCK,HOLDLOCK) WHERE IdSucursal=@Sucursal AND CodigoProducto=@Codigo AND Stock>=@Cantidad) THROW 50002,N'Stock insuficiente.',1;
INSERT dbo.Ventas(IdSucursal,IdCliente,IdVendedor,TotalBruto,DescuentoTotal,MetodoPago,EstadoPago) VALUES(@Sucursal,@Cliente,@Vendedor,@Precio*@Cantidad,ROUND(@Precio*@Cantidad*@Descuento/100,2),@Metodo,IIF(@Metodo IN(N'CC',N'Cuenta Corriente'),N'Pendiente',N'Pagado'));
SET @Venta=SCOPE_IDENTITY();
INSERT dbo.DetalleVenta(IdVenta,CodigoProducto,Cantidad,PrecioUnitario) VALUES(@Venta,@Codigo,@Cantidad,@Precio);
UPDATE dbo.StockSucursal SET Stock=Stock-@Cantidad WHERE IdSucursal=@Sucursal AND CodigoProducto=@Codigo;
IF @Metodo IN(N'CC',N'Cuenta Corriente') INSERT dbo.MovimientosCuentaCorriente(IdCliente,IdVenta,Tipo,Descripcion,Debe) SELECT @Cliente,@Venta,N'Cargo',N'Venta '+NumeroFactura,TotalNeto FROM dbo.Ventas WHERE IdVenta=@Venta;
COMMIT;
SELECT * FROM dbo.Ventas WHERE IdVenta=@Venta;
GO
