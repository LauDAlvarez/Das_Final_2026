using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TechStore.App.Data;

#nullable disable

namespace TechStore.App.Migrations;

[DbContext(typeof(TechStoreDbContext))]
[Migration("202607290001_InitialCreate")]
public sealed class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE "Branches" ("Id" INTEGER NOT NULL CONSTRAINT "PK_Branches" PRIMARY KEY AUTOINCREMENT, "Name" TEXT NOT NULL, "Address" TEXT NOT NULL, "Phone" TEXT NOT NULL, "IsActive" INTEGER NOT NULL);
            CREATE TABLE "Categories" ("Id" INTEGER NOT NULL CONSTRAINT "PK_Categories" PRIMARY KEY AUTOINCREMENT, "Name" TEXT NOT NULL, "Description" TEXT NULL, "IsActive" INTEGER NOT NULL);
            CREATE TABLE "Customers" ("Id" INTEGER NOT NULL CONSTRAINT "PK_Customers" PRIMARY KEY AUTOINCREMENT, "DocumentNumber" TEXT NULL, "BusinessName" TEXT NOT NULL, "Email" TEXT NULL, "Phone" TEXT NULL, "Address" TEXT NULL, "CustomerType" INTEGER NOT NULL, "DiscountPercentage" TEXT NOT NULL, "CurrentAccountBalance" TEXT NOT NULL, "IsActive" INTEGER NOT NULL, "CreatedAt" TEXT NOT NULL);
            CREATE TABLE "Sellers" ("Id" INTEGER NOT NULL CONSTRAINT "PK_Sellers" PRIMARY KEY AUTOINCREMENT, "Name" TEXT NOT NULL, "DocumentNumber" TEXT NOT NULL, "Email" TEXT NULL, "Phone" TEXT NULL, "IsActive" INTEGER NOT NULL);
            CREATE TABLE "Products" ("Id" INTEGER NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY AUTOINCREMENT, "Code" TEXT NOT NULL, "Name" TEXT NOT NULL, "Description" TEXT NULL, "CategoryId" INTEGER NOT NULL, "Price" TEXT NOT NULL, "IsActive" INTEGER NOT NULL, "CreatedAt" TEXT NOT NULL, "UpdatedAt" TEXT NOT NULL, CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE);
            CREATE TABLE "Inventories" ("Id" INTEGER NOT NULL CONSTRAINT "PK_Inventories" PRIMARY KEY AUTOINCREMENT, "ProductId" INTEGER NOT NULL, "BranchId" INTEGER NOT NULL, "Stock" INTEGER NOT NULL, "MinimumStock" INTEGER NOT NULL, CONSTRAINT "FK_Inventories_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE, CONSTRAINT "FK_Inventories_Branches_BranchId" FOREIGN KEY ("BranchId") REFERENCES "Branches" ("Id") ON DELETE CASCADE);
            CREATE TABLE "Sales" ("Id" INTEGER NOT NULL CONSTRAINT "PK_Sales" PRIMARY KEY AUTOINCREMENT, "InvoiceNumber" TEXT NOT NULL, "Date" TEXT NOT NULL, "CustomerId" INTEGER NOT NULL, "BranchId" INTEGER NOT NULL, "SellerId" INTEGER NOT NULL, "Subtotal" TEXT NOT NULL, "DiscountAmount" TEXT NOT NULL, "Total" TEXT NOT NULL, "PaymentMethod" INTEGER NOT NULL, "PaymentStatus" INTEGER NOT NULL, "Notes" TEXT NULL, "IsCancelled" INTEGER NOT NULL, CONSTRAINT "FK_Sales_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE RESTRICT, CONSTRAINT "FK_Sales_Branches_BranchId" FOREIGN KEY ("BranchId") REFERENCES "Branches" ("Id") ON DELETE RESTRICT, CONSTRAINT "FK_Sales_Sellers_SellerId" FOREIGN KEY ("SellerId") REFERENCES "Sellers" ("Id") ON DELETE RESTRICT);
            CREATE TABLE "SaleItems" ("Id" INTEGER NOT NULL CONSTRAINT "PK_SaleItems" PRIMARY KEY AUTOINCREMENT, "SaleId" INTEGER NOT NULL, "ProductId" INTEGER NOT NULL, "Quantity" INTEGER NOT NULL, "UnitPrice" TEXT NOT NULL, "Subtotal" TEXT NOT NULL, CONSTRAINT "FK_SaleItems_Sales_SaleId" FOREIGN KEY ("SaleId") REFERENCES "Sales" ("Id") ON DELETE CASCADE, CONSTRAINT "FK_SaleItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE);
            CREATE TABLE "CurrentAccountMovements" ("Id" INTEGER NOT NULL CONSTRAINT "PK_CurrentAccountMovements" PRIMARY KEY AUTOINCREMENT, "CustomerId" INTEGER NOT NULL, "SaleId" INTEGER NULL, "Date" TEXT NOT NULL, "MovementType" INTEGER NOT NULL, "Description" TEXT NOT NULL, "Debit" TEXT NOT NULL, "Credit" TEXT NOT NULL, "Balance" TEXT NOT NULL, CONSTRAINT "FK_CurrentAccountMovements_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE, CONSTRAINT "FK_CurrentAccountMovements_Sales_SaleId" FOREIGN KEY ("SaleId") REFERENCES "Sales" ("Id") ON DELETE RESTRICT);
            CREATE UNIQUE INDEX "IX_Products_Code" ON "Products" ("Code");
            CREATE INDEX "IX_Products_CategoryId" ON "Products" ("CategoryId");
            CREATE UNIQUE INDEX "IX_Inventories_ProductId_BranchId" ON "Inventories" ("ProductId", "BranchId");
            CREATE INDEX "IX_Inventories_BranchId" ON "Inventories" ("BranchId");
            CREATE UNIQUE INDEX "IX_Customers_DocumentNumber" ON "Customers" ("DocumentNumber");
            CREATE UNIQUE INDEX "IX_Sellers_DocumentNumber" ON "Sellers" ("DocumentNumber");
            CREATE UNIQUE INDEX "IX_Sales_InvoiceNumber" ON "Sales" ("InvoiceNumber");
            CREATE INDEX "IX_Sales_CustomerId" ON "Sales" ("CustomerId");
            CREATE INDEX "IX_Sales_BranchId" ON "Sales" ("BranchId");
            CREATE INDEX "IX_Sales_SellerId" ON "Sales" ("SellerId");
            CREATE INDEX "IX_SaleItems_SaleId" ON "SaleItems" ("SaleId");
            CREATE INDEX "IX_SaleItems_ProductId" ON "SaleItems" ("ProductId");
            CREATE INDEX "IX_CurrentAccountMovements_CustomerId" ON "CurrentAccountMovements" ("CustomerId");
            CREATE INDEX "IX_CurrentAccountMovements_SaleId" ON "CurrentAccountMovements" ("SaleId");
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("CurrentAccountMovements");
        migrationBuilder.DropTable("Inventories");
        migrationBuilder.DropTable("SaleItems");
        migrationBuilder.DropTable("Sales");
        migrationBuilder.DropTable("Products");
        migrationBuilder.DropTable("Branches");
        migrationBuilder.DropTable("Customers");
        migrationBuilder.DropTable("Sellers");
        migrationBuilder.DropTable("Categories");
    }
}
