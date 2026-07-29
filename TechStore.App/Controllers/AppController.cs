using Microsoft.EntityFrameworkCore;
using TechStore.App.Data;

namespace TechStore.App.Controllers;

public sealed class AppController
{
    public async Task<List<object>> ProductsAsync(string search = "")
    {
        await using var db = new TechStoreDbContext();
        return await db.Products.AsNoTracking()
            .Where(x => x.Code.Contains(search) || x.Name.Contains(search))
            .OrderBy(x => x.Name)
            .Select(x => (object)new
            {
                x.Id,
                Código = x.Code,
                Nombre = x.Name,
                Categoría = x.Category.Name,
                Precio = x.Price,
                Stock = db.Inventories
                    .Where(inventory => inventory.ProductId == x.Id)
                    .Sum(inventory => (int?)inventory.Stock) ?? 0,
                Activo = x.IsActive
            }).ToListAsync();
    }

    public async Task<List<object>> EntitiesAsync(string module, string search = "")
    {
        await using var db = new TechStoreDbContext();
        return module switch
        {
            "Categorías" => await db.Categories.AsNoTracking().Where(x => x.Name.Contains(search)).OrderBy(x => x.Name).Select(x => (object)new { x.Id, x.Name, x.Description, x.IsActive }).ToListAsync(),
            "Clientes" => await db.Customers.AsNoTracking().Where(x => x.BusinessName.Contains(search) || (x.DocumentNumber ?? "").Contains(search)).OrderBy(x => x.BusinessName).Select(x => (object)new { x.Id, x.DocumentNumber, x.BusinessName, x.CustomerType, x.DiscountPercentage, x.CurrentAccountBalance, x.IsActive }).ToListAsync(),
            "Sucursales" => await db.Branches.AsNoTracking().Where(x => x.Name.Contains(search)).OrderBy(x => x.Name).Select(x => (object)new { x.Id, x.Name, x.Address, x.Phone, x.IsActive }).ToListAsync(),
            _ => await db.Sellers.AsNoTracking().Where(x => x.Name.Contains(search) || x.DocumentNumber.Contains(search)).OrderBy(x => x.Name).Select(x => (object)new { x.Id, x.Name, x.DocumentNumber, x.Email, x.Phone, x.IsActive }).ToListAsync()
        };
    }
}
