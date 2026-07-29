USE TechStoreDB;
GO
DECLARE @Desde date=DATEADD(month,-1,CAST(GETDATE() AS date)),@Hasta date=CAST(GETDATE() AS date);
SELECT s.InvoiceNumber,s.Date,b.Name Sucursal,c.BusinessName Cliente,se.Name Vendedor,s.Total,s.PaymentMethod,s.IsCancelled FROM dbo.Sales s JOIN dbo.Branches b ON b.Id=s.BranchId JOIN dbo.Customers c ON c.Id=s.CustomerId JOIN dbo.Sellers se ON se.Id=s.SellerId WHERE s.Date>=@Desde AND s.Date<DATEADD(day,1,@Hasta) ORDER BY s.Date DESC;
SELECT p.Code,p.Name Producto,SUM(i.Quantity) CantidadVendida,SUM(i.Subtotal) Importe FROM dbo.SaleItems i JOIN dbo.Sales s ON s.Id=i.SaleId JOIN dbo.Products p ON p.Id=i.ProductId WHERE s.IsCancelled=0 AND s.Date>=@Desde AND s.Date<DATEADD(day,1,@Hasta) GROUP BY p.Code,p.Name ORDER BY CantidadVendida DESC;
SELECT c.Id,c.BusinessName Cliente,c.CurrentAccountBalance Saldo FROM dbo.Customers c WHERE c.CurrentAccountBalance>0 ORDER BY c.BusinessName;
GO
