using FileExplorer.MainApp;
using FileExplorer.PluginManagement;
using FileExplorer.SearchManagement;
using FileExplorer.UserInterface;
using FileExplorer.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Helpers.Warning("=== BOOTCAMP SEARCH :: An extendible command-line search tool ===\n");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var appConfig = configuration.GetSection("AppConfig").Get<AppConfig>();
var services = new ServiceCollection();
ConfigureServices(services, appConfig);
var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetService<App>();
app.Run();
Helpers.DisplayMessage("Press any key to exit...");
Console.ReadKey();


static void ConfigureServices(IServiceCollection services, AppConfig appConfig)
{
    services.AddSingleton<IConsoleInterface, ConsoleInterface>();
    services.AddSingleton<IFileSearcher, FileSearcher>();
    services.AddSingleton<IPluginManager, PluginManager>();
    services.AddSingleton(appConfig);
    services.AddSingleton<App>();
}
public class AppConfig
{
    public string PluginPath { get; set; }
    public int SearchThreshold { get; set; }
}