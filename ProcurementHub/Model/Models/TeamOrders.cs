﻿using ProcurementHub.Model.Enums;

namespace ProcurementHub.Model.Models
{
	public class TeamOrders
	{
		public Guid ID { get; set; }
		public int TeamID { get; set; }
		public int TeamRestaurantID { get; set; }
		public TeamOrderStatusEnum Status { get; set; }
		public int OrderStartedByID { get; set; }
		public DateTime OrderStartedOn { get; set; }
		public decimal TotalPriceOfOrder { get; set; }
		public int OrderPayedByID { get; set; }
		public DateTime OrderFinishedOn { get; set; }

		public Teams Team { get; set; }
		public TeamRestaurants TeamRestaurants { get; set; }
		public Persons OrderStartedBy { get; set; }
		public Persons OrderPayedBy { get; set; }
	}

	
}
