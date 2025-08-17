using System.Windows;
using Microsoft.Win32;
using Snipineft.Contracts;
using Snipineft.Models;

namespace Snipineft.Services;
 
public class FileService(IXmlParserService xmlParser) : IFileService
{
    public IEnumerable<Payroll> OpenFile()
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
                return xmlParser.ParseXml(openFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return [];
            }
        }
        else return [];
    }
}