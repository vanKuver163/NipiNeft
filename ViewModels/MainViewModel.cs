using System.Collections.ObjectModel;
using System.Windows.Input;
using Snipineft.Commands;
using Snipineft.Contracts;
using Snipineft.Models;

namespace Snipineft.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IXmlParserService _xmlParser;
    private readonly IFileService _fileService;
    private ObservableCollection<Payroll> _payrolls;

    public ObservableCollection<Payroll> Payrolls
    {
        get => _payrolls;
        set => SetProperty(ref _payrolls, value);
    }
    
    public ICommand OpenFileCommand { get; }

    public MainViewModel(IXmlParserService xmlParser, IFileService fileService)
    {
        _xmlParser = xmlParser;
        _fileService = fileService;
        OpenFileCommand = new RelayCommand(() => _fileService.OpenFile(Payrolls));
    }
}