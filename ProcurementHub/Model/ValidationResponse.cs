using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.Model
{
    public class ValidationResponseWithResult<T> : ValidationResponse where T : class
    {
        public T ResultValues { get; set; }
    }

    public class ValidationResponse
    {
        public bool Successful { get; set; }
        public string Information { get; set; }
    }
}
