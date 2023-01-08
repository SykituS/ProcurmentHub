using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.ViewModel
{
    internal class RestaurantsMenu
    {
        int ID { get; set; }
        int RestaurantID { get; set; }
        string ItemName { get; set; }
        int ItemPrice { get; set; }
        string ItemDescription { get; set; }
        string ItemCategory { get; set; }

        Restaurants Restaurant { get; set; }
    }
}
