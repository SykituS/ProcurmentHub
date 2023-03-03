using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcShared;
using GrpcShared.Models;

namespace ProcurementHub.Services
{
    public class PersonsService
    {
        private readonly Procurement.ProcurementClient _procurementClient;

        public PersonsService(Procurement.ProcurementClient procurementClient)
        {
            _procurementClient = procurementClient;
        }

        readonly List<Persons> _persons = new List<Persons>();

        public async Task<List<Persons>> GetPersonsAsync()
        {
            var result = await _procurementClient.GetPersonsAsync(new GRPCStatus() { Code = 200 });

            foreach (var person in result.Persons)
            {
                _persons.Add(new Persons
                    {
                        Id = person.Id,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Email = person.Email
                    });
            }

            return _persons;
        }
    }
}
