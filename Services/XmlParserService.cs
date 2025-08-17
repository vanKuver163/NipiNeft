using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Snipineft.Contracts;
using Snipineft.Models;

namespace Snipineft.Services;

public class XmlParserService : IXmlParserService
{
    public IEnumerable<Payroll> ParseXml(string filePath)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    
        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Ignore
        };
    
        using var reader = XmlReader.Create(filePath, settings);
        var doc = XDocument.Load(reader);
   
        var payrolls = new List<Payroll>();

        foreach (var dataElement in doc.Descendants("Data"))
        {
            var docInfo = dataElement.Element("DocInfo");
            var number = docInfo?.Attribute("Code")?.Value ?? string.Empty;
            
            payrolls.AddRange(ProcessResources(dataElement.Element("Tz"), number, "Трудозатраты"));
            payrolls.AddRange(ProcessResources(dataElement.Element("Mch"), number, "Машины и механизмы"));
            payrolls.AddRange(ProcessResources(dataElement.Element("Mat"), number, "Материалы"));
            payrolls.AddRange(ProcessResources(dataElement.Element("Tr"), number, "Перевозка"));
            payrolls.AddRange(ProcessResources(dataElement.Element("Load"), number, "Погрузка/разгрузка"));
        }
        
        return payrolls;
    }

    private IEnumerable<Payroll> ProcessResources(XElement? parentElement, string number, string sectionName)
    {
        if (parentElement == null) yield break;
        int count = 1;
        foreach (var resource in parentElement.Elements("Resource"))
        {
            yield return new Payroll
            {
                Id = count,
                Number = number,
                Code = resource.Attribute("Code")?.Value ?? string.Empty,
                Material = $"{sectionName}: {resource.Attribute("Caption")?.Value ?? string.Empty}",
                Units = resource.Attribute("Units")?.Value ?? string.Empty,
                Quantity = ParseDouble(resource.Attribute("Quantity")?.Value),
                Price = ParseDouble(resource.Element("Price")?.Attribute("CE")?.Value),
                Total = ParseDouble(resource.Element("Total")?.Attribute("CE")?.Value)
            };
            count++;
        }
    }

    private double ParseDouble(string? value)
    {
        if (string.IsNullOrEmpty(value)) return 0;

        value = value.Replace(",", ".");
        if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }
        return 0;
    }
}