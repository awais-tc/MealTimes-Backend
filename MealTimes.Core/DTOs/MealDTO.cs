
using System.ComponentModel.DataAnnotations;

namespace MealTimes.Core.DTOs
{
    public class MealCreateDto
    {
        public int ChefID { get; set; }
        public string MealName { get; set; }
        public string MealDescription { get; set; }
        public decimal Price { get; set; }
        public string MealCategory { get; set; }
        public int PreparationTime { get; set; }
        public string? ImageUrl { get; set; }  // Can be null initially
        public bool Availability { get; set; } = true;
    }

    public class MealUpdateDto
    {
        public int MealID { get; set; }
        public string MealName { get; set; }
        public string MealDescription { get; set; }
        public decimal Price { get; set; }
        public string MealCategory { get; set; }
        public int PreparationTime { get; set; }
        public string? ImageUrl { get; set; }
        public bool Availability { get; set; }
    }

    public class MealDto
    {
        public int MealID { get; set; }
        public string MealName { get; set; }
        public string MealDescription { get; set; }
        public decimal Price { get; set; }
        public string MealCategory { get; set; }
        public int PreparationTime { get; set; }
        public string ImageUrl { get; set; }
        public string ChefName { get; set; }
        public bool Availability { get; set; }
        public double Rating { get; set; }
    }

    public class MealFilterDto
    {
        public string? DietaryPreference { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Category { get; set; }
        public bool? AvailableOnly { get; set; }
    }

}