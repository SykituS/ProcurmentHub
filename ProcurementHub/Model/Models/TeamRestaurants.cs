namespace ProcurementHub.Model.Models
{
	public class TeamRestaurants
	{
		public int ID { get; set; }
		public int TeamID { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string? Description { get; set; }
		public int CreatedById { get; set; }
		public DateTime CreatedOn { get; set; }
		public int UpdatedById { get; set; }
		public DateTime UpdatedOn { get; set; }

		public Teams Team { get; set; }
		public Persons CreatedBy { get; set; }
		public Persons UpdatedBy { get; set; }
	}
}
