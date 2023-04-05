namespace FoodsAPI.Models.DTOs
{
    public class FoodUpdateDTO
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public double Calories { get; set; }
    }
}
