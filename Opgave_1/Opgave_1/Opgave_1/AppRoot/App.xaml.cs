
using Globals;
using AudioTools;
using LogicLayer;
using Microsoft.Extensions.DependencyInjection;
using PresentationLayer;
using PresentationLayer.Views;
using System.Windows;
using PresentationLayer.ViewModels;
using Globals.Interfaces;

namespace AppRoot;
/// <summary>
/// Composition root for application
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    private void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection.AddAudioServices();
        serviceCollection.AddTransient<MainViewModel>();
        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddLogicServices();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow?.Show();
    }
}
