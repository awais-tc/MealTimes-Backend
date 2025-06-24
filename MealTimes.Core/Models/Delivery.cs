using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MealTimes.Core.Models
{
    public class Delivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryID { get; set; }

        public int OrderID { get; set; }
        public int? DeliveryPersonID { get; set; }

        [MaxLength(100)]
        public string? DeliveryServiceName { get; set; }

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;
        public DateTime? PickedUpAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("DeliveryPersonID")]
        public virtual DeliveryPerson? DeliveryPerson { get; set; }
    }
}
