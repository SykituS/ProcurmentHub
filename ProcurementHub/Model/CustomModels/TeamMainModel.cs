using ProcurementHub.Model.Enums;
using ProcurementHub.Model.Models;

namespace ProcurementHub.Model.CustomModels
{
	public class TeamMainModel
    {
		public int ID { get; set; }
		public string TeamName { get; set; }
		public string Description { get; set; }
		public TeamStatusEnum Status { get; set; }
		public TeamRoleEnum Role { get; set; }
		
		public Guid? OrderId { get; set; }
		public bool IsOrderActive => OrderId != null;
		public bool IsOrderInActive => OrderId == null;
        public DateTime OrderStartedOn { get; set; }

		public bool IsAdmin => Role == TeamRoleEnum.TeamAdministrator;
    }
}
