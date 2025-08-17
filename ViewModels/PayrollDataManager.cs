using System.Collections.ObjectModel;
using Snipineft.Contracts;
using Snipineft.Models;

namespace Snipineft.ViewModels;

public class PayrollDataManager : BaseViewModel, IPayrollDataManager
{
    private readonly IFileService _fileService;
    private ObservableCollection<Payroll> _payrolls = new ObservableCollection<Payroll>();
    private double _totalCost;

    public ObservableCollection<Payroll> Payrolls
    {
        get => _payrolls;
        private set
        {
            if (SetProperty(ref _payrolls, value))
            {
                PayrollsChanged?.Invoke();
                TotalCost = Payrolls.Sum(x => x.Total);
            }
        }
    }
    
    public void UpdateItem(Payroll item)
    {
        item.Total = item.Quantity * item.Price;
        var tempList = new ObservableCollection<Payroll>(Payrolls);
        Payrolls = tempList;
    }

    public double TotalCost
    {
        get => _totalCost;
        private set
        {
            if (SetProperty(ref _totalCost, value)) TotalCostChanged?.Invoke();
        }
    }
    
    public event Action? PayrollsChanged;
    public event Action? TotalCostChanged;

    public PayrollDataManager(IFileService fileService)
    {
        _fileService = fileService;
    }

    public void LoadPayrolls() => Payrolls = new ObservableCollection<Payroll>(_fileService.OpenFile());

    public void SortPayrolls<TKey>(Func<Payroll, TKey> keySelector) =>
        Payrolls = new ObservableCollection<Payroll>(Payrolls.OrderBy(keySelector));
}