using Microsoft.EntityFrameworkCore; using TechStore.App.Enums; using TechStore.App.Models;
namespace TechStore.App.Data;
public static class DatabaseInitializer {
 public static async Task InitializeAsync(){await using var db=new TechStoreDbContext(); await db.Database.EnsureCreatedAsync(); if(await db.Categories.AnyAsync())return;
  var cats=new[]{"Computación","Periféricos","Telefonía","Accesorios"}.Select(x=>new Category{Name=x,Description=$"Productos de {x}"}).ToArray(); db.AddRange(cats); await db.SaveChangesAsync();
  var products=Enumerable.Range(1,10).Select(i=>new Product{Code=$"TS-{i:000}",Name=new[]{"Notebook","Monitor","Teclado","Mouse","Teléfono","Auriculares","Webcam","Disco SSD","Memoria RAM","Cargador"}[i-1],CategoryId=cats[(i-1)%4].Id,Price=15000m+i*7250}).ToArray();
  var branches=new[]{new Branch{Name="Casa Central",Address="Av. Tecnología 100",Phone="11-4000-1000"},new Branch{Name="Sucursal Norte",Address="Belgrano 450",Phone="11-4000-2000"},new Branch{Name="Sucursal Sur",Address="Mitre 800",Phone="11-4000-3000"}};
  var customers=Enumerable.Range(1,6).Select(i=>new Customer{DocumentNumber=$"3000000{i}",BusinessName=$"Cliente Demo {i}",Email=$"cliente{i}@correo.com",CustomerType=i%2==0?CustomerType.Mayorista:CustomerType.Minorista,DiscountPercentage=i%2==0?10:0}).ToArray();
  var sellers=Enumerable.Range(1,3).Select(i=>new Seller{Name=$"Vendedor {i}",DocumentNumber=$"2500000{i}",Email=$"vendedor{i}@techstore.com"}).ToArray(); db.AddRange(products);db.AddRange(branches);db.AddRange(customers);db.AddRange(sellers);await db.SaveChangesAsync();
  foreach(var branch in branches) foreach(var product in products) db.Add(new Inventory{BranchId=branch.Id,ProductId=product.Id,Stock=15+product.Id,MinimumStock=5}); await db.SaveChangesAsync();
  for(int i=0;i<5;i++){
   Product product=products[i];
   Customer customer=customers[i%6];
   decimal subtotal=product.Price*(i+1);
   decimal discount=subtotal*customer.DiscountPercentage/100;
   var sale=new Sale{InvoiceNumber=$"FAC-0000000{i+1}",Date=DateTime.Today.AddDays(-i),CustomerId=customer.Id,BranchId=branches[i%3].Id,SellerId=sellers[i%3].Id,Subtotal=subtotal,DiscountAmount=discount,Total=subtotal-discount,PaymentMethod=i<2?PaymentMethod.CuentaCorriente:PaymentMethod.Efectivo,PaymentStatus=i<2?PaymentStatus.Pendiente:PaymentStatus.Pagado};
   sale.Items.Add(new SaleItem{ProductId=product.Id,Quantity=i+1,UnitPrice=product.Price,Subtotal=subtotal});
   db.Add(sale);
   if(i<2){customer.CurrentAccountBalance+=sale.Total;db.Add(new CurrentAccountMovement{Customer=customer,Sale=sale,Date=sale.Date,MovementType=CurrentAccountMovementType.Cargo,Description="Venta inicial en cuenta corriente",Debit=sale.Total,Balance=customer.CurrentAccountBalance});}
  }
  await db.SaveChangesAsync();
 }
}
