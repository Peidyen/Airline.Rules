using System;
using Airline.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NRules;
using NRules.Fluent;

namespace Airline.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_NonPartner_Gold()
        {
            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof(SetPartnerReward).Assembly));

            //Compile rules
            var factory = repository.Compile();

            //Create a working session
            var session = factory.CreateSession();

            //Load domain model
            var customer = new AirlineAccount { AccountStatus = "BASIC", NumberOfMiles = 150000 };
            var flight = new RecentFlight { IsPartnerAirline = false, Miles = 2500, SeatClass = "REGULAR" };
            //Insert facts into rules engine's memory
            session.Insert(customer);
            session.Insert(flight);

            //Start match/resolve/act cycle
            session.Fire();

            Assert.IsTrue(flight.BasicAdded);
            Assert.IsFalse(flight.BonusAdded);
            Assert.AreEqual("GOLD", customer.AccountStatus);
            Assert.AreEqual(152500, customer.NumberOfMiles);
        }

        [TestMethod]
        public void Test_NonPartner_SilverToGold()
        {
            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof(SetPartnerReward).Assembly));

            //Compile rules
            var factory = repository.Compile();

            //Create a working session
            var session = factory.CreateSession();

            //Load domain model
            var customer = new AirlineAccount { AccountStatus = "BASIC", NumberOfMiles = 99999 };
            var flight = new RecentFlight { IsPartnerAirline = false, Miles = 2500, SeatClass = "REGULAR" };
            //Insert facts into rules engine's memory
            session.Insert(customer);
            session.Insert(flight);

            //Start match/resolve/act cycle
            session.Fire();

            Assert.IsTrue(flight.BasicAdded);
            Assert.IsFalse(flight.BonusAdded);
            Assert.AreEqual("GOLD", customer.AccountStatus);
            Assert.AreEqual(99999 + (int)Math.Ceiling(flight.Miles * .2), customer.NumberOfMiles);
        }
    }
}
