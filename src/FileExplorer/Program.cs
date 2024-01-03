using FileExplorer.Configuration;
using FileExplorer.Core;
using FileExplorer.Utilities;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("=== BOOTCAMP SEARCH :: An extendible command-line search tool ===\n");
var startup = new Startup(args);
var services = new ServiceCollection();
startup.ConfigureServices(services);

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetService<App>();
app.Run();

Helpers.DisplayMessage("Press any key to exit...");
Console.ReadKey();