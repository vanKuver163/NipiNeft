using Snipineft.Models;

namespace Snipineft.Contracts;

public interface IXmlParserService
{
    IEnumerable<Payroll> ParseXml(string filePath);
}