using ProcurementHub.Domain.Models;

namespace ProcurementHub.Domain.Handlers.Queries
{
    public class GetUsersQuery
    {
        public GetUsersQuery(ProcurementHubContext procurementHubContext)
        {
            if (procurementHubContext == null)
                throw new ArgumentNullException(nameof(procurementHubContext));

            Context = procurementHubContext;
        }

        protected ProcurementHubContext Context { get; }

        public IQueryable<Users> Execute()
        {
            return Context.Users;
        }
    }
}
