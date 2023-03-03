using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.Services;

namespace ProcurementHub.ViewModel
{
    public partial class PersonsViewModel : BaseViewModel
    {
        public ObservableCollection<Persons> Persons { get; set; } = new();


        public PersonsViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
            Title = "GRPCPerson";
        }

        [RelayCommand]
        async Task GetAllPersons()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Persons.Clear();
                var perons = await new PersonsService(ProcurementClient).GetPersonsAsync();

                foreach (var peron in perons)
                {
                    Persons.Add(new Persons
                    {
                        Id = peron.Id,
                        FirstName = peron.FirstName,
                        LastName = peron.LastName,
                        Email = peron.Email
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
