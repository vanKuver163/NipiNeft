using System.Collections.ObjectModel;
using Snipineft.Models;

namespace Snipineft.Contracts;

public interface IFileService
{
    void OpenFile(ObservableCollection<Payroll> payrolls);
}