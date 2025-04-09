using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.Models
{
    public class ThirdPartyDeliveryService
    {
        [Key]
        public int DeliveryID { get; set; }

        public int OrderID { get; set; }

        [Required]
        public string DeliveryStatus { get; set; }

        public TimeSpan EstimatedDeliveryTime { get; set; }

        [Required]
        public string DeliveryPartnerName { get; set; }

        // Navigation Property
        public Order Order { get; set; }
    }

}
