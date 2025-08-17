namespace Snipineft.Models;

public class Payroll
{
   
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public string Units { get; set; } = string.Empty;
    public double Price { get; set; }
    public double Quantity { get; set; }
    public double Total { get; set; }
}