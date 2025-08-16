using Microsoft.Extensions.DependencyInjection;
using Snipineft.Contracts;
using Snipineft.Services;
using Snipineft.ViewModels;

namespace Snipineft.Locator;

public class ViewModelLocator
{
    private readonly ServiceProvider _serviceProvider;

    public ViewModelLocator()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
        ServiceLocator.Initialize(this);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IXmlParserService, XmlParserService>();
        services.AddSingleton<IFileService, FileService>();
        
        services.AddSingleton<MainViewModel>();
    }

    public ServiceProvider GetServiceProvider() => _serviceProvider;
    public MainViewModel MainViewModel => _serviceProvider.GetService<MainViewModel>()!;
}