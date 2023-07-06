using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;

namespace ProcurementHub.Services
{
    class TeamRestaurantsService : BaseServices
	{
		public TeamRestaurantsService(Procurement.ProcurementClient procurementClient) : base(procurementClient)
		{
		}
	}
}
