USE TechStoreDB;
GO
/* Ejemplo equivalente al flujo transaccional de TechStore.App. PaymentMethod: 0 efectivo, 1 tarjeta, 2 transferencia, 3 cuenta corriente. */
DECLARE @BranchId int=1,@CustomerId int=2,@SellerId int=1,@ProductId int=1,@Quantity int=2,@PaymentMethod int=3,@SaleId int,@Price decimal(18,2),@Discount decimal(5,2),@Subtotal decimal(18,2),@DiscountAmount decimal(18,2),@Total decimal(18,2),@Invoice nvarchar(30);
SET XACT_ABORT ON; BEGIN TRAN;
SELECT @Price=Price FROM dbo.Products WHERE Id=@ProductId AND IsActive=1;
SELECT @Discount=DiscountPercentage FROM dbo.Customers WHERE Id=@CustomerId AND IsActive=1;
IF @Price IS NULL OR @Discount IS NULL OR @Quantity<=0 THROW 50002,N'Datos de venta inválidos.',1;
IF NOT EXISTS(SELECT 1 FROM dbo.Inventories WITH(UPDLOCK,HOLDLOCK) WHERE BranchId=@BranchId AND ProductId=@ProductId AND Stock>=@Quantity) THROW 50003,N'Stock insuficiente.',1;
SET @Subtotal=@Price*@Quantity; SET @DiscountAmount=ROUND(@Subtotal*@Discount/100,2); SET @Total=@Subtotal-@DiscountAmount;
SET @Invoice=N'FAC-'+RIGHT(REPLICATE('0',8)+CONVERT(varchar(8),ISNULL((SELECT MAX(Id) FROM dbo.Sales WITH(UPDLOCK,HOLDLOCK)),0)+1),8);
INSERT dbo.Sales(InvoiceNumber,Date,CustomerId,BranchId,SellerId,Subtotal,DiscountAmount,Total,PaymentMethod,PaymentStatus,IsCancelled) VALUES(@Invoice,SYSDATETIME(),@CustomerId,@BranchId,@SellerId,@Subtotal,@DiscountAmount,@Total,@PaymentMethod,IIF(@PaymentMethod=3,1,0),0);
SET @SaleId=SCOPE_IDENTITY(); INSERT dbo.SaleItems(SaleId,ProductId,Quantity,UnitPrice,Subtotal) VALUES(@SaleId,@ProductId,@Quantity,@Price,@Subtotal);
UPDATE dbo.Inventories SET Stock=Stock-@Quantity WHERE BranchId=@BranchId AND ProductId=@ProductId;
IF @PaymentMethod=3 BEGIN UPDATE dbo.Customers SET CurrentAccountBalance=CurrentAccountBalance+@Total WHERE Id=@CustomerId; INSERT dbo.CurrentAccountMovements(CustomerId,SaleId,Date,MovementType,Description,Debit,Credit,Balance) SELECT @CustomerId,@SaleId,SYSDATETIME(),0,N'Venta '+@Invoice,@Total,0,CurrentAccountBalance FROM dbo.Customers WHERE Id=@CustomerId; END;
COMMIT; SELECT * FROM dbo.Sales WHERE Id=@SaleId;
GO
