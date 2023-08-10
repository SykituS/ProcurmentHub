using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.Model.CustomModels
{
    public class OrderItemsModel
    {
        public int ID { get; set; }
        public Guid TeamOrdersID { get; set; }
        public int TeamRestaurantsItemID { get; set; }
        public string TeamRestaurantsItemName { get; set; }
        public string TeamRestaurantsItemDescription { get; set; }
        public decimal TeamRestaurantsItemPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPriceOfItem { get; set; }
        public Guid? DivideToken { get; set; }
        public int? DivideOnNumberOfPersons { get; set; }
        public decimal? DividedPrice { get; set; }
    }
}
