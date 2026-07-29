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
                Activo = x.IsActive
            }).ToListAsync();
    }

    public async Task<List<object>> EntitiesAsync(string module, string search = "")
    {
        await using var db = new TechStoreDbContext();
        return module switch
        {
            "Categorías" => await db.Categories.AsNoTracking()
                .Where(x => x.Name.Contains(search)).OrderBy(x => x.Name)
                .Select(x => (object)new { x.Id, Nombre = x.Name, Descripción = x.Description, Activa = x.IsActive }).ToListAsync(),
            "Clientes" => await db.Customers.AsNoTracking()
                .Where(x => x.BusinessName.Contains(search) || (x.DocumentNumber ?? "").Contains(search)).OrderBy(x => x.BusinessName)
                .Select(x => (object)new { x.Id, Documento = x.DocumentNumber, RazónSocial = x.BusinessName, Tipo = x.CustomerType, Descuento = x.DiscountPercentage, Saldo = x.CurrentAccountBalance, Activo = x.IsActive }).ToListAsync(),
            "Sucursales" => await db.Branches.AsNoTracking()
                .Where(x => x.Name.Contains(search)).OrderBy(x => x.Name)
                .Select(x => (object)new { x.Id, Nombre = x.Name, Dirección = x.Address, Teléfono = x.Phone, Activa = x.IsActive }).ToListAsync(),
            _ => await db.Sellers.AsNoTracking()
                .Where(x => x.Name.Contains(search) || x.DocumentNumber.Contains(search)).OrderBy(x => x.Name)
                .Select(x => (object)new { x.Id, Nombre = x.Name, Legajo = x.DocumentNumber, Correo = x.Email, Teléfono = x.Phone, Activo = x.IsActive }).ToListAsync()
        };
    }
}
