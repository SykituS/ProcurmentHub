using ProcurementHub.Model.Enums;
using ProcurementHub.Model.Models;

namespace ProcurementHub.Model.CustomModels
{
    public class TeamSettingsModel
    {
	    public int ID { get; set; }
	    public string Description { get; set; }
	    public TeamStatusEnum Status { get; set; }
	    public string TeamJoinCode { get; set; }
	    public string TeamJoinPassword { get; set; }
	}
}
