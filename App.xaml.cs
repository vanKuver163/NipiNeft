using System.Windows;
using Snipineft.Locator;

namespace Snipineft;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
       
        var viewModelLocator = new ViewModelLocator();
     
        var mainWindow = new MainWindow
        {
            DataContext = viewModelLocator.MainViewModel
        };
        mainWindow.Show();
    }
}