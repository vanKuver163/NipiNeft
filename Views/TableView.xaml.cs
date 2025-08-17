using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Snipineft.Views;

public partial class TableView
{
    public TableView()
    {
        InitializeComponent();
    }

    private void DataGrid_BeginningEdit(object? sender, DataGridBeginningEditEventArgs e)
    {
        if (e.Column == null || e.Row == null) return;

        if (e.Column.DisplayIndex == 4 || e.Column.DisplayIndex == 5)
        {
            e.Cancel = false;
        }
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        if (!(sender is TextBox textBox)) return;
        try
        {
            var currentText = textBox.Text ?? "";
            var caretIndex = Math.Min(textBox.CaretIndex, currentText.Length);
            caretIndex = Math.Max(0, caretIndex);
            var selectionStart = Math.Min(textBox.SelectionStart, currentText.Length);
            var selectionLength = Math.Min(textBox.SelectionLength, currentText.Length - selectionStart);
            var textWithoutSelection = currentText.Remove(selectionStart, selectionLength);
            var newText = textWithoutSelection.Length == 0
                ? e.Text
                : textWithoutSelection.Insert(Math.Min(caretIndex, textWithoutSelection.Length), e.Text);
            e.Handled = !decimal.TryParse(newText,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out _);
        }
        catch
        {
            e.Handled = true;
        }
    }
}