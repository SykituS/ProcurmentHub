

using GrpcShared;

namespace ProcurementHub.Services
{
    public class UsersService
    {
        private readonly Procurement.ProcurementClient _procurementClient;

        public UsersService(Procurement.ProcurementClient procurementClient)
        {
            _procurementClient = procurementClient;
        }

        //readonly List<Users> _userList = new();

        //public async Task<List<Users>> GetUsers()
        //{
        //    if (_userList?.Count > 0)
        //        return _userList;

        //    var mess = _procurementClient.SayHello(new HelloRequest{ Name = "Name"});

        //    return _userList;
        //}
    }
}
