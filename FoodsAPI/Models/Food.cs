namespace FoodsAPI.Models
{
    public class Food
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public double Calories { get; set; }
    }
}
