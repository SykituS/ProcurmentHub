using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcShared;
using ProcurementHub.Model.Models;

namespace ProcurementHub.Services
{
    public class PersonsService : BaseServices
    {
        public PersonsService(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }

        readonly List<Persons> _persons = new List<Persons>();

        //public async Task<List<Persons>> GetPersonsAsync()
        //{
        //    var result = await ProcurementClient.Get(new GRPCStatus() { Code = 200 });

        //    foreach (var person in result.Persons)
        //    {
        //        _persons.Add(new Persons
        //            {
        //                Id = person.Id,
        //                FirstName = person.FirstName,
        //                LastName = person.LastName,
        //                Email = person.Email
        //            });
        //    }

        //    return _persons;
        //}

        
    }
}
