using GrpcShared;
using ProcurementService.Data;
using ProcurementService.Data.Query;
using ProcurementService.Functions;

namespace ProcurementService.Services
{
    public partial class ProcurementService : Procurement.ProcurementBase
    {
        private readonly ILogger<ProcurementService> _logger;
        private readonly ProcurementContext _context;
        private readonly VerifyLogin _verifyLogin;
        private readonly CodeGenerator _codeGenerator;
        private readonly GetUsersQuery _usersQuery;

        public ProcurementService(ILogger<ProcurementService> logger, ProcurementContext context,
            VerifyLogin verifyLogin, CodeGenerator codeGenerator, GetUsersQuery usersQuery)
        {
            _logger = logger;
            _context = context;
            _verifyLogin = verifyLogin;
            _codeGenerator = codeGenerator;
            _usersQuery = usersQuery;
        }
    }

    
}