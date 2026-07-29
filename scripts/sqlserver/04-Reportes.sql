USE TechStoreDB;
GO
DECLARE @Desde date=DATEADD(month,-1,CAST(GETDATE() AS date)),@Hasta date=CAST(GETDATE() AS date);
SELECT v.NumeroFactura,v.Fecha,s.Nombre Sucursal,c.NombreRazonSocial Cliente,ve.Nombre Vendedor,v.TotalNeto,v.MetodoPago,v.Anulada FROM dbo.Ventas v JOIN dbo.Sucursales s ON s.IdSucursal=v.IdSucursal JOIN dbo.Clientes c ON c.IdCliente=v.IdCliente JOIN dbo.Vendedores ve ON ve.IdVendedor=v.IdVendedor WHERE v.Fecha>=@Desde AND v.Fecha<DATEADD(day,1,@Hasta) ORDER BY v.Fecha DESC;
SELECT d.CodigoProducto,p.Descripcion,SUM(d.Cantidad) CantidadVendida,SUM(d.Subtotal) Importe FROM dbo.DetalleVenta d JOIN dbo.Ventas v ON v.IdVenta=d.IdVenta JOIN dbo.Productos p ON p.Codigo=d.CodigoProducto WHERE v.Anulada=0 AND v.Fecha>=@Desde AND v.Fecha<DATEADD(day,1,@Hasta) GROUP BY d.CodigoProducto,p.Descripcion ORDER BY CantidadVendida DESC;
SELECT c.IdCliente,c.NombreRazonSocial,SUM(m.Debe-m.Haber) Saldo FROM dbo.Clientes c LEFT JOIN dbo.MovimientosCuentaCorriente m ON m.IdCliente=c.IdCliente GROUP BY c.IdCliente,c.NombreRazonSocial HAVING SUM(m.Debe-m.Haber)<>0;
GO
