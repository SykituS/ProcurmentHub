using GrpcShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.Model.CustomModels
{
    public class TeamMembersModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public decimal SpendAmount { get; set; }
        public decimal PayedAmount { get; set; } 
        public decimal PayedSpendRation { get; set; }

        private string _ratioColor;
        public string RatioColor
        {
            get
            {
                switch (PayedSpendRation)
                {
                    case var expression when PayedSpendRation > 100:
                        return _ratioColor = "#2E7F18";
                    case var expression when PayedSpendRation > 50:
                        return _ratioColor = "#45731E";
                    case var expression when PayedSpendRation > 25:
                        return _ratioColor = "#675E24";
                    case var expression when PayedSpendRation == 0:
                        return _ratioColor = "Black";
                    case var expression when PayedSpendRation < -25:
                        return _ratioColor = "#8D472B";
                    case var expression when PayedSpendRation < -50:
                        return _ratioColor = "#B13433";
                    case var expression when PayedSpendRation < -100:
                        return _ratioColor = "#C82538";
                    default:
                        return _ratioColor = "Black";
                }
            }
            set => _ratioColor = value;
        }

        public bool IsGroupCreator { get; set; }

        public string FullName => $"{FirstName} {Lastname}";
    }
}
