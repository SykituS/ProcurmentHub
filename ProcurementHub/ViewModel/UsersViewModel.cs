using ProcurementHub.Domain.Models;
using ProcurementHub.Services;
using Users = ProcurementHub.Domain.Models.Users;

namespace ProcurementHub.ViewModel
{
    public partial class UsersViewModel : BaseViewModel
    {
        public ObservableCollection<Users> Users { get; set; } = new();
        public UsersViewModel()
        {
            Title = "Users";
        }

        [RelayCommand]
        async Task GetUsers()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Users.Clear();
                var users = await new UsersService().GetUsers();
                foreach (var user in users)
                {
                    Users.Add(user);
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
