using System.Collections.ObjectModel;
using Snipineft.Models;

namespace Snipineft.Contracts;

public interface IFileService
{
    IEnumerable<Payroll> OpenFile();
}