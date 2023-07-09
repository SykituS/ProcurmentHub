namespace ProcurementHub.Model.Models
{
	public class TeamRestaurantsItems
	{
		public int ID { get; set; }
		public int TeamRestaurantsID { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int CreatedById { get; set; }
		public DateTime CreatedOn { get; set; }
		public int UpdatedById { get; set; }
		public DateTime UpdatedOn { get; set; }

		public TeamRestaurants TeamRestaurants { get; set; }
		public Persons CreatedBy { get; set; }
		public Persons UpdatedBy { get; set; }
	}
}
