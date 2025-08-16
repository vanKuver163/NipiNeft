namespace Snipineft.Models;

public class Payroll
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Component> Components { get; init; } = new List<Component>();
}