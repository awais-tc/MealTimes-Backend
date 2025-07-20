
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealTimes.Core.Models
{
    public class HomeChef
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChefID { get; set; }

        [MaxLength(100)]
        public required string FullName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public required string PhoneNumber { get; set; }

        public required string Address { get; set; }

        public double Rating { get; set; } = 0.0;

        public int? LocationID { get; set; }
        [ForeignKey("LocationID")]
        public virtual Location? Location { get; set; }

        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public ICollection<Meal> Meals { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
