

using GrpcShared;
using ProcurementHub.Domain.Models;

namespace ProcurementHub.Services
{
    public class UsersService
    {
        private readonly Greeter.GreeterClient _greeterClient;

        public UsersService(Greeter.GreeterClient greeterClient)
        {
            _greeterClient = greeterClient;
        }

        readonly List<Users> _userList = new();

        public async Task<List<Users>> GetUsers()
        {
            if (_userList?.Count > 0)
                return _userList;

            var mess = _greeterClient.SayHello(new HelloRequest{ Name = "Name"});

            return _userList;
        }
    }
}
