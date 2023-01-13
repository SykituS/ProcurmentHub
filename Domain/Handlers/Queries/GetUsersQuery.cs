using ProcurementHub.Domain.Models;

namespace ProcurementHub.Domain.Handlers.Queries
{
    public class GetUsersQuery
    {
        public GetUsersQuery(DomainContext domainContext)
        {
            if (domainContext == null)
                throw new ArgumentNullException(nameof(domainContext));

            Context = domainContext;
        }

        protected DomainContext Context { get; }

        public IQueryable<Users> Execute()
        {
            return Context.Users;
        }
    }
}
