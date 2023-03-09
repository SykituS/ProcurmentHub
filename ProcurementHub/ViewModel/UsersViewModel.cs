using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.Services;

namespace ProcurementHub.ViewModel
{
    [QueryProperty(nameof(Users), "Users")]
    public partial class UsersViewModel : BaseViewModel
    {
        //public ObservableCollection<Users> Users { get; set; } = new();

        public UsersViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
            Title = "Users";
        }

        [ObservableProperty] private Users _users;

        //[RelayCommand]
        //async Task GetUsers()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;

        //    try
        //    {
        //        Users.Clear();
        //        var users = await new UsersService(ProcurementClient).GetUsers();
        //        foreach (var user in users)
        //        {
        //            Users.Add(user);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}
    }
}
