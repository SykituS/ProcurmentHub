using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcurementHub.Model.Models;

namespace ProcurementHub.Model.CustomModels
{
	public class TeamRestaurantsModel
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }
		public string CreatedOn { get; set; }
		public PersonsModel CreatedBy { get; set; }

		public bool IsUpdated { get; set; }
		public string UpdatedOn { get; set; }
		public PersonsModel UpdatedBy { get; set; }
	}
}
