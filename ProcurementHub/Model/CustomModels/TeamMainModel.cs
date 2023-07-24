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

		//TODO: Setup information about order if there is any
		public int? OrderId { get; set; }
		public bool IsAnyOrderActive => OrderId != null;

		public bool IsAdmin => Role == TeamRoleEnum.TeamAdministrator;
    }
}
