namespace FoodsAPI.Data
{
    using FoodsAPI.Models;

    public class FoodData
    {
        public static List<Food> Foods = new()
        {
            new Food
            {
                Id = 1,
                Name = "Bread",
                Description = "Slices of bread made of flour, water, salt, yeast and other ingredients.",
                Calories = 264.6,
            },
            new Food
            {
                Id = 2,
                Name = "Apple",
                Description = "Found on apple trees.",
                Calories = 52.1,
            },
            new Food
            {
                Id = 3,
                Name = "Chicken Breasts",
                Description = "Obtained when killing a chicken.",
                Calories = 164.9,
            }
        };
    }
}
