using ProcurementHub.Domain;
using ProcurementHub.Domain.Handlers.Queries;
using ProcurementHub.Domain.Models;
using Users = ProcurementHub.Domain.Models.Users;

namespace ProcurementHub.Services
{
    public class UsersService
    {
        private readonly GetUsersQuery _getUsersQuery;
        public UsersService()
        {
            _getUsersQuery = new GetUsersQuery(new ProcurementHubContext());
        }

        List<Users> _userList = new();

        public async Task<List<Users>> GetUsers()
        {
            if (_userList?.Count > 0)
                return _userList;

            _userList = await Task.Run(() => _getUsersQuery.Execute().ToList());

            return _userList;
        }
    }
}
