using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.Model.CustomModels
{
    public class TeamRestaurantItemsModel
    {
	    public int ID { get; set; }
	    public int TeamRestaurantsID { get; set; }
	    public string Name { get; set; }
	    public string Description { get; set; }
	    public decimal Price { get; set; }
		public string CurrencyType { get; set; }
		public bool IsDeleted { get; set; }
	    public DateTime CreatedOn { get; set; }
	    public DateTime UpdatedOn { get; set; }
		
	    public PersonsModel CreatedBy { get; set; }
	    public PersonsModel UpdatedBy { get; set; }
	}
}
