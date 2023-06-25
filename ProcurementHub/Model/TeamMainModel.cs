using GrpcShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.Model
{
	public class TeamMainModel
    {
		public int ID { get; set; }
		public string TeamName { get; set; }
		public string Description { get; set; }
		public TeamStatusEnum Status { get; set; }
		public TeamRoleEnum Role { get; set; }

		public bool IsAdmin => Role == TeamRoleEnum.TeamAdministrator;
    }
}
