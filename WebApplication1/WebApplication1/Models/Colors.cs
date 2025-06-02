namespace WebApplication1.Models;

public class Color
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Car> Cars { get; set; } = new List<Car>();
}