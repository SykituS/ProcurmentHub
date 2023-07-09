namespace ProcurementHub.Model.Models
{
	public class TeamOrdersItems
	{
		public int ID { get; set; }
		public Guid TeamOrdersID { get; set; }
		public int TeamRestaurantsItemsID { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPriceOfItem { get; set; }
		public Guid? DivideToken { get; set; }
		public int? DivideOnNumberOfPersons { get; set; }
		public decimal? DividedPrice { get; set; }

		public TeamOrders TeamOrders { get; set; }
		public TeamRestaurantsItems TeamRestaurantsItems { get; set; }
	}
}
