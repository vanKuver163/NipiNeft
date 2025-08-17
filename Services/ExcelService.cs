using System.IO;
using System.Reflection;
using ClosedXML.Excel;
using Snipineft.Contracts;
using Snipineft.Models;
using Microsoft.Win32;
using System.Windows;

namespace Snipineft.Services;

public class ExcelService : IExcelService
{
    public bool ExportToExcel(IEnumerable<Payroll> payrolls)
    {
        if (payrolls == null || !payrolls.Any())
        {
            MessageBox.Show("Нет данных для экспорта", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var templateStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Template.XLSX")
                                       ?? throw new FileNotFoundException("Шаблон не найден в ресурсах");

            var saveDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                DefaultExt = ".xlsx",
                FileName = $"Payrolls_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                Title = "Сохранить отчет по payrolls"
            };

            if (saveDialog.ShowDialog() != true) return false;

            using var workbook = new XLWorkbook(templateStream);
            var worksheet = workbook.Worksheet(1)
                            ?? throw new InvalidOperationException("Лист не найден в шаблоне");

            int startRow = 13;
            foreach (var payroll in payrolls)
            {
                var row = worksheet.Row(startRow++);
                row.Cell(1).Value = payroll.Id;
                row.Cell(2).Value = payroll.Number;
                row.Cell(3).Value = payroll.Code;
                row.Cell(4).Value = payroll.Material;
                row.Cell(5).Value = payroll.Units;
                row.Cell(6).Value = payroll.Quantity;
                row.Cell(7).Value = payroll.Price;
                row.Cell(8).Value = payroll.Total;
                row.Cell(9).Value = payroll.Total;
                row.Cell(10).Value = string.Empty;
            }

            var totalCost = payrolls.Sum(x => x.Total);
            var nds = totalCost * 0.2;
            var totalCostWithNds = totalCost + nds;

            var totalRow = startRow + 2;
            AddSummaryRow(worksheet, totalRow, "ИТОГО по разделу 1", "X", "X", "X", totalCost, totalCost);

            var ndsRow = totalRow + 1;
            AddSummaryRow(worksheet, ndsRow, "НДС", "%", string.Empty, "X", nds, nds);

            var totalWithNdsRow = ndsRow + 1;
            AddSummaryRow(worksheet, totalWithNdsRow,
                "ВСЕГО ПО РАЗДЕЛУ 1 МТР поставки оператора \"Подрядчик\" с учетом НДС",
                "Х", "Х", "X", totalCostWithNds, totalCostWithNds);

            FormatWorksheet(worksheet, startRow - 1, totalWithNdsRow);

            workbook.SaveAs(saveDialog.FileName);
            MessageBox.Show($"Отчет успешно сохранен:\n{saveDialog.FileName}", "Успех");
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении отчета:\n{ex.Message}", "Ошибка");
            return false;
        }
    }

    private void AddSummaryRow(IXLWorksheet worksheet, int rowNum, string col4Text,
        string col5Text, string col6Text, string col7Text, double col8Value, double col9Value)
    {
        var row = worksheet.Row(rowNum);
        row.Cell(4).Value = col4Text;
        row.Cell(5).Value = col5Text;
        row.Cell(6).Value = col6Text;
        row.Cell(7).Value = col7Text;
        row.Cell(8).Value = col8Value;
        row.Cell(9).Value = col9Value;
        row.Cell(4).Style.Font.Bold = true;
    }

    private void FormatWorksheet(IXLWorksheet worksheet, int lastDataRow, int lastRow)
    {
        worksheet.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        for (int col = 1; col <= 10; col++)
        {
            if (col != 4) 
            {
                worksheet.Column(col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }
        }
        var dataRange = worksheet.Range(13, 1, lastRow, 10);
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        worksheet.Columns().AdjustToContents();
    }
}