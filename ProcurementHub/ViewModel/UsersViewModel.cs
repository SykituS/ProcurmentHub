using ProcurementHub.Model;
using ProcurementHub.Services;

namespace ProcurementHub.ViewModel
{
    public partial class UsersViewModel : BaseViewModel
    {
        private readonly UsersService _usersService;
        public ObservableCollection<Users> Users { get; set; } = new();
        public UsersViewModel(UsersService usersService)
        {
            _usersService = usersService;
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
                List<Users> users = new List<Users>
                {
                    new Users()
                    {
                        FirstName = "Mateusz",
                        LastName = "Jaruga"
                    }
                };
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
