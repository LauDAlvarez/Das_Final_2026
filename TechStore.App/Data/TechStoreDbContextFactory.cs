using Microsoft.EntityFrameworkCore.Design;

namespace TechStore.App.Data;

/// <summary>Permite que dotnet-ef cree el contexto sin iniciar Windows Forms.</summary>
public sealed class TechStoreDbContextFactory : IDesignTimeDbContextFactory<TechStoreDbContext>
{
    public TechStoreDbContext CreateDbContext(string[] args) => new();
}
