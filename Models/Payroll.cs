namespace Snipineft.Models;

public class Payroll
{
    public int Id { get; set; }
    public string Number { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string Material { get; init; } = string.Empty;
    public string Units { get; init; } = string.Empty;
    public double Price { get; set; }
    public double Quantity { get; set; }
    public double Total { get; set; }
}