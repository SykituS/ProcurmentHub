using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.ViewModel
{
    internal class UserStatistics
    {
        int ID { get; set; }
        int TeamID { get; set; }
        int UserID { get; set; }
        int OrderID { get; set; }
        int PaymentID { get; set; }
        int RestaurantMenuID { get; set; }
        int Quantity { get; set; }
        int ItemPrice { get; set; }
        int OrderTimestamp { get; set; }
        int PaymentTimestamp { get; set; }
        int PaymentMethod { get; set; }
        
        Teams Team { get; set; }
        Users User { get; set; }
        Orders Order { get; set; }
        Payments Payment { get; set; }
        RestaurantsMenu RestaurantMenu { get; set; }
    }
}
