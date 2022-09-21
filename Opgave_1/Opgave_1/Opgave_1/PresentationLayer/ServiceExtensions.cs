using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.ViewModels;
using PresentationLayer.Views;

namespace PresentationLayer;
public static class ServiceExtensions
{

    public static void AddPresentationServices(this ServiceCollection services)
    {
        // Register the classes that need to be injected as singleton or transient (or scoped).

        services.AddSingleton<MainWindow>();
        services.AddTransient<MainViewModel>();
    }
}