using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.ViewModel
{
    internal class Orders
    {
        int ID { get; set; }
        int RestaurantID { get; set; }
        int TeamID { get; set; }
        int UserID { get; set; }
        int RestaurantMenuID { get; set; }
        int Quantity { get; set; }
        int OrderTimestamp { get; set; }
        int Total { get; set; }

        Restaurants Restaurant { get; set; }
        RestaurantsMenu RestaurantMenu { get; set; }
        Teams Team { get; set; }
        Users User { get; set; }
        
    }
}
