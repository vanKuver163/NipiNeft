using System.Collections.ObjectModel;
using Snipineft.Models;

namespace Snipineft.Contracts;

public interface IPayrollDataManager
{
    ObservableCollection<Payroll> Payrolls { get;}
    void LoadPayrolls();
    void SortPayrolls<TKey>(Func<Payroll, TKey> keySelector);
    event Action? PayrollsChanged;
}