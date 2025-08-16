using Microsoft.Extensions.DependencyInjection;
using Snipineft.Locator;

namespace Snipineft.Services;

public static class ServiceLocator
{
    private static IServiceProvider? _provider;

    public static void Initialize(ViewModelLocator locator)
    {
        _provider = locator.GetServiceProvider();
    }

    public static T GetService<T>() where T : class
    {
        if (_provider != null) return _provider.GetRequiredService<T>();
        else throw new NullReferenceException("Service not initialized");
    }
}