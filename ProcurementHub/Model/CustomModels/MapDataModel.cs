using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.Model.CustomModels
{
    public class MapDataModel
    {
        public string Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location Localization { get; set; }
        public string Address { get; set; }
    }
}
