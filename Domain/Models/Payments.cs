using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Payments
    {
        int ID { get; set; }
        int TeamID { get; set; }
        int UserID { get; set; }
        int OrderID { get; set; }
        int Amount { get; set; }
        int PaymentTimestamp { get; set; }
        int PaymentMethod { get; set; }

        Teams Team { get; set; }
        Users User { get; set; }
        Orders Order { get; set; }
    }
}
