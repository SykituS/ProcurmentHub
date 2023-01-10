using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Restaurants
    {
        int ID { get; set; }
        string Name { get; set; }
        string Location { get; set; }
        int PhoneNumber { get; set; }
        string Website { get; set; }
        float Rating { get; set; }
    }
}
