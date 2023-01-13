using ProcurementHub.Model;

namespace ProcurementHub.Services
{
    public class UsersService
    {

        public UsersService()
        {
        }

        List<Users> userList = new();

        public async Task<List<Users>> GetUsers()
        {
            if (userList?.Count > 0)
                return userList;

            //userList = await Task.Run(() => _getUsersQuery.Execute().ToList());

            return userList;
        }
    }
}
