using GrpcShared;
using Microsoft.EntityFrameworkCore;

namespace ProcurementService.Data.Query
{
    public class GetUsersQuery
    {
        private ProcurementContext _context;

        public GetUsersQuery(ProcurementContext context)
        {
            _context = context;
        }

        public Task<GRPCLoggedUser> GetUserDataFromDb(GRPCLoginInformationForUser request)
        {
            var id = Guid.Parse(request.Id);

            var data = _context.Users.Include(e => e.Person).FirstOrDefault(e => e.Id == id);

            var user = new GRPCUser();
            var person = new GRPCPerson();
            if (data != null)
            {
                user.Id = data.Id.ToString();
                user.UserName = data.UserName;
                user.PasswordHash = data.PasswordHash;
                user.SecurityStamp = data.SecurityStamp;
                user.PersonID = data.PersonID ?? 0;
                user.PrivacyAgreed = data.PrivacyAgreed ?? false;
                user.PrivacyAgreedOn = data.PrivacyAgreedOn.ToString();
                user.Disabled = data.Disabled;
                user.CreatedOn = data.CreatedOn.ToString();
                user.UpdatedOn = data.UpdatedOn.ToString();

                person.Id = data.Person.Id;
                person.Email = data.Person.Email;
                person.FirstName = data.Person.FirstName;
                person.LastName = data.Person.LastName;
            }

            var result = new GRPCLoggedUser()
            {
                User = user,
                Person = person,
                ValidationResponse = new GRPCValidationResponse()
                {
                    Successful = true,
                }
            };

            return Task.FromResult(result);
        }
    }
}
