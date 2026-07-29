using TechStore.App.Data; using TechStore.App.Views;
namespace TechStore.App;
internal static class Program { [STAThread] static async Task Main(){ApplicationConfiguration.Initialize();try{await DatabaseInitializer.InitializeAsync();Application.Run(new MainForm());}catch(Exception ex){MessageBox.Show($"No se pudo iniciar TechStore: {ex.Message}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);}} }
