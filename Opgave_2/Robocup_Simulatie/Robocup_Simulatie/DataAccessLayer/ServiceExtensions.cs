using Globals.Entities;
using Globals.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer;
public static class ServiceExtensions
{

    public static void AddDataAccessServices(this ServiceCollection services)
    {
        // Register the classes that need to be injected as singleton or transient (or scoped).

        //services.AddSingleton<ILogic, GameLogic>();
    }
}
