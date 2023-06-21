using Azure.Core;
using GrpcShared;
using Microsoft.EntityFrameworkCore;
using ProcurementService.Data;
using System.Diagnostics;
using System.Net;

namespace ProcurementService.Functions
{
    public class VerifyLogin
    {
        private ProcurementContext _context;

        public VerifyLogin(ProcurementContext context)
        {
            _context = context;
        }

        public ValidationResponse CheckIfUsersExists(string userName, string password, Guid? id = null)
        {
            if (id.HasValue)
            {
                var user = _context.Users.FirstOrDefault(e =>
                    e.Id == id.Value && e.UserName == userName && e.PasswordHash == password);

                if (user == null)
                {
                    return new ValidationResponse
                    {
                        Successful = false
                    };
                }

                return new ValidationResponse
                {
                    Successful = true,
                    Information = user.PersonID.Value.ToString(),
                };
            }
            else
            {
                var user = _context.Users.Where(e =>
                e.UserName.Equals(userName)).ToList().FirstOrDefault(e => BCrypt.Net.BCrypt.Verify(password, e.PasswordHash));

                if (user == null)
                {
                    return new ValidationResponse
                    {
                        Successful = false
                    };
                }
                return new ValidationResponse
                {
                    Successful = true,
                    Information = user.Id.ToString(),
                };
            }
        }
    }
}
