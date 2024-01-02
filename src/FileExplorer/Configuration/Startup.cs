using FileExplorer.Core;
using FileExplorer.FileHandling;
using FileExplorer.PluginHandlers;
using FileExplorer.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileExplorer.Configuration;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(string[] args)
    {
        Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var appConfig = Configuration.GetSection("AppConfig").Get<AppConfig>();

        services.AddSingleton<IConsoleInterface, ConsoleInterface>();
        services.AddSingleton<IFileSearcher, FileSearcher>();
        services.AddSingleton<IPluginManager, PluginManager>();
        services.AddSingleton(appConfig);
        services.AddSingleton<App>();
    }
}
