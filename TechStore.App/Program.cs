using TechStore.App.Data; using TechStore.App.Views;
namespace TechStore.App;
internal static class Program { [STAThread] static async Task Main(){ApplicationConfiguration.Initialize();try{await DatabaseInitializer.InitializeAsync();Application.Run(new MainForm());}catch(Exception ex){MessageBox.Show($"No se pudo conectar con SQL Server.\n\nServidor configurado: LAUTI\nBase: TechStoreDB\n\nDetalle: {ex.GetBaseException().Message}\n\nRevise TechStore.App\\appsettings.json o la variable TECHSTORE_CONNECTION_STRING.","Error de conexión",MessageBoxButtons.OK,MessageBoxIcon.Error);}} }
