using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Snipineft.Commands;
using Snipineft.Contracts;
using Snipineft.Models;
using System.IO;

namespace Snipineft.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IPayrollDataManager _payrollDataManager;
    private string? _selectedSort = string.Empty;
    public ObservableCollection<Payroll> Payrolls => _payrollDataManager.Payrolls;
    public double TotalCost => _payrollDataManager.TotalCost;
    public ObservableCollection<string> Sorts { get; } = ["Код материала", "Материал"];

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

    public void UpdateItem(Payroll item) => _payrollDataManager.UpdateItem(item);
    public ICommand OpenFileCommand { get; }
    public ICommand SaveCommand { get; }

    public MainViewModel(IPayrollDataManager payrollDataManager, IExcelService excelService)
    {
        _payrollDataManager = payrollDataManager;
        _payrollDataManager.PayrollsChanged += () => OnPropertyChanged(nameof(Payrolls)); 
        _payrollDataManager.TotalCostChanged += () => OnPropertyChanged(nameof(TotalCost));
        OpenFileCommand = new RelayCommand(_payrollDataManager.LoadPayrolls);
        SaveCommand = new RelayCommand(() => excelService.ExportToExcel(Payrolls));
    }
}