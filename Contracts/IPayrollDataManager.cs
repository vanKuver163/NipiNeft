using System.Collections.ObjectModel;
using Snipineft.Models;

namespace Snipineft.Contracts;

public interface IPayrollDataManager
{
    ObservableCollection<Payroll> Payrolls { get;}
    double TotalCost { get; }
    void LoadPayrolls();
    void SortPayrolls<TKey>(Func<Payroll, TKey> keySelector);
    event Action? PayrollsChanged;
    event Action? TotalCostChanged;
    void UpdateItem(Payroll item);
}