namespace ProcurementHub.Domain.Models
{
    public class RestaurantsItemMenu
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
