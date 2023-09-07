using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GrpcShared;
using ProcurementHub.Infrastructure;
using ProcurementHub.Model;
using ProcurementHub.Model.CustomModels;

namespace ProcurementHub.Services
{
    public class BaseServices
    {
        protected readonly Procurement.ProcurementClient ProcurementClient;
        protected readonly ValidationResponse ValidationResponse = new();
        protected readonly IMapper _mapper = MapperConfig.CreateMapper();

        public BaseServices(Procurement.ProcurementClient procurementClient)
        {
            ProcurementClient = procurementClient;
        }
    }
}
