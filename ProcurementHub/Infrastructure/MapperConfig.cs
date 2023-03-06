using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;

namespace ProcurementHub.Infrastructure
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GRPCUser, Users>();
                cfg.CreateMap<GRPCPerson, Persons>();
                cfg.CreateMap<GRPCLoggedUser, Users>()
                    .ForMember(dest => dest.Person, act => act.MapFrom(src => src.Person))
                    ;
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
