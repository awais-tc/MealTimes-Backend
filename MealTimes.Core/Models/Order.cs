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
        public DeliveryStatus DeliveryStatus { get; set; }  // Enum

        [Required]
        public PaymentStatus PaymentStatus { get; set; }    // Enum

        // Navigation
        public Employee Employee { get; set; }
        public HomeChef Chef { get; set; }
        public ICollection<OrderMeal> OrderMeals { get; set; }

        public ThirdPartyDeliveryService? ThirdPartyDeliveryService { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public Payment? Payment { get; set; }
    }

    public enum PaymentStatus
    {
        Pending,
        Succeeded,
        Failed
    }

    public enum DeliveryStatus
    {
        Pending,
        Preparing,
        ReadyForPickup,
        InTransit,
        Delivered
    }

}
