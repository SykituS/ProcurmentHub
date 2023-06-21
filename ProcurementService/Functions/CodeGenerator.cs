using Microsoft.VisualBasic;
using ProcurementService.Data;

namespace ProcurementService.Functions
{
    public class CodeGenerator
    {
        private ProcurementContext _context;

        public CodeGenerator(ProcurementContext context)
        {
            _context = context;
        }

        public string GenerateRandomCode(int lenght, bool shouldBeUnique)
        {
            var chars = "0123456789QAZWSXEDCRFVTGBYHNUJMIKOLP";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, 6).Select(e => e[random.Next(e.Length)]).ToArray());
            if (shouldBeUnique)
            {
                while (_context.Teams.Any(e => e.TeamJoinCode.Equals(result)))
                {
                    result = new string(Enumerable.Repeat(chars, 6).Select(e => e[random.Next(e.Length)]).ToArray());
                }
            }

            return result.ToString();
        }
    }
}