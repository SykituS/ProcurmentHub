

using ProcurementHub.Domain.Models;

namespace ProcurementHub.Services
{
    public class UsersService
    {
        public UsersService()
        {
        }

        readonly List<Users> _userList = new();

        public async Task<List<Users>> GetUsers()
        {
            if (_userList?.Count > 0)
                return _userList;

            return _userList;
        }
    }
}
