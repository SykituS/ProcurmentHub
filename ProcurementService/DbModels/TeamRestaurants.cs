using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcurementService.DbModels;

namespace GrpcShared.Models
{
	public class TeamRestaurants
	{
		public int ID { get; set; }
		public int TeamID { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string? Description { get; set; }
		public bool IsDeleted { get; set; }
		public int CreatedById { get; set; }
		public DateTime CreatedOn { get; set; }
		public int UpdatedById { get; set; }
		public DateTime UpdatedOn { get; set; }

		//TODO: Add geo location for restaurant?

		public Teams Team { get; set; }
		public Persons CreatedBy { get; set; }
		public Persons UpdatedBy { get; set; }
	}
}
