using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared.Models;

namespace ProcurementHub.Model
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
