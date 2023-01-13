using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.Model
{
    public class Orders
    {
        int ID { get; set; }
        int RestaurantID { get; set; }
        int TeamID { get; set; }
        int UserID { get; set; }
        int Quantity { get; set; }
        int OrderTimestamp { get; set; }
        int Total { get; set; }
        ICollection<RestaurantsItemMenu> RestaurantItemsMenu { get; set; }

        Restaurants Restaurant { get; set; }
        Teams Team { get; set; }
        Users User { get; set; }

    }
}
