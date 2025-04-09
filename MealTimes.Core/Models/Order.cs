using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealTimes.Core.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        public int EmployeeID { get; set; }
        public int ChefID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public string DeliveryStatus { get; set; }

        [Required]
        public string PaymentStatus { get; set; }

        // Navigation Properties
        public Employee Employee { get; set; }
        public HomeChef Chef { get; set; }

        public ICollection<OrderMeal> OrderMeals { get; set; }
        public ThirdPartyDeliveryService ThirdPartyDeliveryService { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public Payment Payment { get; set; }
    }
}
