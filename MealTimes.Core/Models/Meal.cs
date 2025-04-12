using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MealTimes.Core.Models
{
    public class Meal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MealID { get; set; }

        public int ChefID { get; set; }
        [ForeignKey("ChefID")]
        public HomeChef Chef { get; set; }

        [Required]
        public string MealName { get; set; }

        public string MealDescription { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string MealCategory { get; set; }

        public int PreparationTime { get; set; }
        public string? ImageUrl { get; set; }
        public bool Availability { get; set; } = true;
        public double Rating { get; set; } = 0.0;

        // Navigation Properties

        // Navigation property for many-to-many relationship with Order
        public ICollection<OrderMeal> OrderMeals { get; set; }
    }
}
