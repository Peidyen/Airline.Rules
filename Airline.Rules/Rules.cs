using NRules.Fluent.Dsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline.Rules
{
    public class SetGold : Rule
    {
        public override void Define()
        {
            AirlineAccount acc = null;
            When()
                .Match(() => acc, (x) => x.AccountStatus == "SILVER" && x.NumberOfMiles > 100000);

            Then()
                .Do(ctx => SetStatusToGold(acc))
                .Do(ctx => ctx.Update(acc));
        }

        private void SetStatusToGold(AirlineAccount acc)
        {
            acc.AccountStatus = "GOLD";
        }
    }

    public class SetSilver : Rule
    {
        public override void Define()
        {
            AirlineAccount acc = null;
            When()
                .Match(() => acc, (x) => x.AccountStatus == "BASIC" && x.NumberOfMiles > 25000);

            Then()
                .Do(ctx => SetStatusToSilver(acc))
                .Do(ctx => ctx.Update(acc));
        }

        private void SetStatusToSilver(AirlineAccount acc)
        {
            acc.AccountStatus = "SILVER";
        }
    }

    public class SetPartnerReward : Rule
    {
        public override void Define()
        {
            RecentFlight flight = null;
            AirlineAccount acc = null;
            When()
                .Match(() => flight, (x) => x.IsPartnerAirline && !x.BasicAdded)
                .Match(() => acc, (x) => true);

            Then()
                .Do(ctx => AddFullMiles(acc, flight))
                .Do(ctx => ctx.Update(acc))
                .Do(ctx => ctx.Update(flight));
        }

        private void AddFullMiles(AirlineAccount acc, RecentFlight flight)
        {
            acc.NumberOfMiles += flight.Miles;
            flight.BasicAdded = true;
        }
    }

    public class SetBusinessLevelPartnerReward : Rule
    {
        public override void Define()
        {
            RecentFlight flight = null;
            AirlineAccount acc = null;
            When()
                .Match(() => flight, (x) => x.IsPartnerAirline && !x.BonusAdded && (x.SeatClass == "BUSINESS" || x.SeatClass == "FIRST"))
                .Match(() => acc, (x) => true);

            Then()
                .Do(ctx => AddFullMiles(acc, flight))
                .Do(ctx => ctx.Update(acc))
                .Do(ctx => ctx.Update(flight));
        }

        private void AddFullMiles(AirlineAccount acc, RecentFlight flight)
        {
            acc.NumberOfMiles += flight.Miles;
            flight.BonusAdded = true;
        }
    }

    public class SetGoldNonPartnerReward : Rule
    {
        public override void Define()
        {
            RecentFlight flight = null;
            AirlineAccount acc = null;
            When()
                .Match(() => flight, (x) => !x.IsPartnerAirline && !x.BasicAdded)
                .Match(() => acc, (x) => x.AccountStatus == "GOLD");

            Then()
                .Do(ctx => AddFullMiles(acc, flight))
                .Do(ctx => ctx.Update(acc))
                .Do(ctx => ctx.Update(flight));
        }

        private void AddFullMiles(AirlineAccount acc, RecentFlight flight)
        {
            acc.NumberOfMiles += flight.Miles;
            flight.BasicAdded = true;
        }
    }

    public class SetSilverNonPartnerReward : Rule
    {
        public override void Define()
        {
            RecentFlight flight = null;
            AirlineAccount acc = null;
            When()
                .Match(() => flight, (x) => !x.IsPartnerAirline && !x.BasicAdded)
                .Match(() => acc, (x) => x.AccountStatus == "SILVER");

            Then()
                .Do(ctx => AddTwentyPercentMiles(acc, flight))
                .Do(ctx => ctx.Update(acc))
                .Do(ctx => ctx.Update(flight));
        }

        private void AddTwentyPercentMiles(AirlineAccount acc, RecentFlight flight)
        {
            acc.NumberOfMiles += (int)Math.Ceiling(flight.Miles *0.2) ;
            flight.BasicAdded = true;
        }
    }
}
