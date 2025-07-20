using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.DTOs
{
    public class OrderCreationDto
    {
        public int EmployeeID { get; set; }

        // List of selected meals (only one of each is allowed)
        public List<SelectedMealDto> Meals { get; set; }
    }

    public class SelectedMealDto
    {
        public int MealID { get; set; }
        // Quantity is assumed 1 due to meal limit enforcement
    }

    public class OrderResponseDto
    {
        public int OrderID { get; set; }
        public int EmployeeID { get; set; }
        public int ChefID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? TrackingNumber { get; set; }

        public string DeliveryStatus { get; set; }     // Enum as string
        public string PaymentStatus { get; set; }      // Enum as string

        public List<MealSummaryDto> Meals { get; set; }
    }

    public class MealSummaryDto
    {
        public int MealID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateOrderDto
    {
        public int EmployeeID { get; set; }
        public List<MealOrderDto> Meals { get; set; }
        public double? MaxDeliveryDistanceKm { get; set; } = 20; // Default 20km radius
    }

    public class MealOrderDto
    {
        public int MealID { get; set; }
    }

    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryStatus { get; set; }
        public string PaymentStatus { get; set; }
        public List<MealDto> Meals { get; set; }
    }

    public class UpdateOrderStatusByChefDto
    {
        public int OrderId { get; set; }
        public string NewStatus { get; set; } = null!;
        public int ChefId { get; set; } // (optional: extract from JWT if already logged-in chef)
    }

    public class OrderTrackingDto
    {
        public int OrderId { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? PickedUpAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
