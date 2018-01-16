using System;
using NUnit.Framework;

namespace Echelon.Tests.Core
{
    public class FuncTests
    {
        [Test]
        public void Method_Scenario_Result()
        {
            var shopper = new Shopper();

            var getNetwork = shopper.UseNetwork(connection =>
            {
                connection += "asd";
                return connection;
            });

            Console.WriteLine(getNetwork);
        }
    }

    public class Shopper
    {
        public string UseNetwork(Func<string,string> connection)
        {
            return connection("dd-88wvw");
        }
    }
}