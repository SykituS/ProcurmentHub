using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcurementHub.Model.Enums;

namespace ProcurementHub.Model.CustomModels
{
	public class OrderModel
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
		public TeamRestaurantsModel Restaurants { get; set; }

		public PersonsModel OrderStartedBy { get; set; }
		public PersonsModel OrderPayedBy { get; set; }
    }
}
