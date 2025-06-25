using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.DTOs
{
    public class DeliveryAssignDto
    {
        public int OrderID { get; set; }
        public int DeliveryPersonID { get; set; } // Required for internal delivery

        public string? DeliveryServiceName { get; set; } // Optional if handled in-house
        public string? TrackingNumber { get; set; }       // Optional
    }

    public class DeliveryStatusUpdateDto
    {
        public int DeliveryID { get; set; }
        public string NewStatus { get; set; } = null!; // Expects values like "InTransit", "Delivered"
    }

    public class DeliveryDto
    {
        public int DeliveryID { get; set; }
        public int OrderID { get; set; }
        public int? DeliveryPersonID { get; set; }

        public string? DeliveryServiceName { get; set; }
        public string? TrackingNumber { get; set; }

        public string Status { get; set; } = null!;
        public DateTime? PickedUpAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
