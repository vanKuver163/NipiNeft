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
        
            //var doc = XDocument.Load(filePath);
        var payrolls = new List<Payroll>();

        foreach (var dataElement in doc.Descendants("Data"))
        {
            var payroll = new Payroll
            {
                Name = dataElement.Attribute("Caption")?.Value ?? string.Empty,
                Components = new List<Component>()
            };
            var docInfo = dataElement.Element("DocInfo");
            var number = docInfo?.Attribute("Code")?.Value ?? string.Empty;
            
            ProcessResources(dataElement.Element("Tz"), payroll.Components, number, "Трудозатраты");
            ProcessResources(dataElement.Element("Mch"), payroll.Components, number, "Машины и механизмы");
            ProcessResources(dataElement.Element("Mat"), payroll.Components, number, "Материалы");
            ProcessResources(dataElement.Element("Tr"), payroll.Components, number, "Перевозка");
            ProcessResources(dataElement.Element("Load"), payroll.Components, number, "Погрузка/разгрузка");
            payrolls.Add(payroll);
        }
        return payrolls;
    }

    private void ProcessResources(XElement? parentElement, ICollection<Component> components, string number,
        string sectionName)
    {
        if (parentElement == null) return;
        
        foreach (var resource in parentElement.Elements("Resource"))
        {
            var component = new Component
            {
                Number = number,
                Code = resource.Attribute("Code")?.Value ?? string.Empty,
                Material = resource.Attribute("Caption")?.Value ?? string.Empty,
                Units = resource.Attribute("Units")?.Value ?? string.Empty,
                Quantity = ParseDouble(resource.Attribute("Quantity")?.Value),
                Price = ParseDouble(resource.Element("Price")?.Attribute("CE")?.Value),
                Total = ParseDouble(resource.Element("Total")?.Attribute("CE")?.Value)
            };

            components.Add(component);
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