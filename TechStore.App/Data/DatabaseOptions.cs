using System.Text.Json;

namespace TechStore.App.Data;

public static class DatabaseOptions
{
    public const string EnvironmentVariable = "TECHSTORE_CONNECTION_STRING";
    const string DefaultConnection = "Server=LAUTI;Database=TechStoreDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

    public static string ConnectionString
    {
        get
        {
            var environmentValue = Environment.GetEnvironmentVariable(EnvironmentVariable);
            if (!string.IsNullOrWhiteSpace(environmentValue)) return environmentValue;

            var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(path)) return DefaultConnection;
            using var document = JsonDocument.Parse(File.ReadAllText(path));
            return document.RootElement.GetProperty("ConnectionStrings").GetProperty("TechStoreDB").GetString()
                   ?? DefaultConnection;
        }
    }
}
