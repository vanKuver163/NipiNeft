using System.Collections.ObjectModel;
using System.Windows.Input;
using Snipineft.Commands;
using Snipineft.Contracts;
using Snipineft.Models;

namespace Snipineft.ViewModels;

public class MainViewModel : BaseViewModel
{
    private ObservableCollection<Payroll> _payrolls = new ObservableCollection<Payroll>();

    public ObservableCollection<Payroll> Payrolls
    {
        get => _payrolls;
        set => SetProperty(ref _payrolls, value);
    }

    public ICommand OpenFileCommand { get; }

    public MainViewModel(IFileService fileService)
    {
        OpenFileCommand = new RelayCommand(() => Payrolls = new ObservableCollection<Payroll>(fileService.OpenFile()));
    }
}