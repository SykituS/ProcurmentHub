namespace ProcurementHub.Domain.Models
{
    public class RestaurantsItemMenu
    {
        public int ID { get; set; }
        public int RestaurantID { get; set; }
        public string ItemName { get; set; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; set; }
        public string ItemCategory { get; set; }
         
        public Restaurants Restaurant { get; set; }
    }
}
