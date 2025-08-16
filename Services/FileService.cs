using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using Snipineft.Contracts;
using Snipineft.Models;

namespace Snipineft.Services;
 
public class FileService(IXmlParserService xmlParser) : IFileService
{
    public void OpenFile(ObservableCollection<Payroll> payrolls)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
            Title = "Выберите XML файл"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var parsedPayrolls = xmlParser.ParseXml(openFileDialog.FileName);
                payrolls = new ObservableCollection<Payroll>(parsedPayrolls);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}