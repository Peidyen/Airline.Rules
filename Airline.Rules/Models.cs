using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline.Rules
{
    public class App
    {
        
    }

    public class AirlineAccount
    {
        public int NumberOfMiles { get; set; }

        public string AccountStatus { get; set; }
    }

    public class RecentFlight
    {
        public int Miles { get; set; }
        public string SeatClass { get; set; }
        public bool IsPartnerAirline { get; set; }
        public bool BasicAdded { get; set; }
        public bool BonusAdded { get; set; }
    }
}
