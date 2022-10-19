using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.ViewModels;
using PresentationLayer.Views;
using Wpf3dTools.Factories;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace PresentationLayer;
public static class ServiceExtensions
{

    public static void AddPresentationServices(this ServiceCollection services)
    {
        // Register the classes that need to be injected as singleton or transient (or scoped).

        services.AddSingleton<MainWindow>();
        services.AddTransient<MainViewModel>();
    }

    public static void AddWpf3dServices(this ServiceCollection services)
    {
        services.AddTransient<ISphericalCameraController, SphericalCameraController>();
        services.AddSingleton<IShapesFactory, ShapesFactory>();
    }
}