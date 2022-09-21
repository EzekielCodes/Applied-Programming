using Globals.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LogicLayer;
public static class ServiceExtensions
{

    public static void AddLogicServices(this ServiceCollection services)
    {
        // Register the classes that need to be injected as singleton or transient (or scoped).

        services.AddSingleton<ILogic, Logic>();
    }
}
