using System.Collections.ObjectModel;
using System.Windows.Input;
using Snipineft.Commands;
using Snipineft.Contracts;
using Snipineft.Models;

namespace Snipineft.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IPayrollDataManager _payrollDataManager;
    private string? _selectedSort = string.Empty;
    public ObservableCollection<Payroll> Payrolls => _payrollDataManager.Payrolls;
    public ObservableCollection<string> Sorts { get; set; } = ["Код материала", "Материал"];

    public string? SelectedSort
    {
        get => _selectedSort;
        set
        {
            SetProperty(ref _selectedSort, value);
            switch (SelectedSort)
            {
                case "Код материала":
                    _payrollDataManager.SortPayrolls(p=> p.Code);
                    break;
                case "Материал":
                    _payrollDataManager.SortPayrolls(p=> p.Material);
                    break;
            }
        }
    }

    public ICommand OpenFileCommand { get; }

    public MainViewModel(IPayrollDataManager payrollDataManager)
    {
        _payrollDataManager = payrollDataManager;
        _payrollDataManager.PayrollsChanged += () =>  OnPropertyChanged(nameof(Payrolls));
        OpenFileCommand = new RelayCommand(_payrollDataManager.LoadPayrolls);
    }
}