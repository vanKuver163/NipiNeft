using Snipineft.Models;

namespace Snipineft.Contracts;

public interface IExcelService
{
   bool ExportToExcel(IEnumerable<Payroll> payrolls);
}