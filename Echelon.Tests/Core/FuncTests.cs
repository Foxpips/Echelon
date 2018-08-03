using System;
using System.Linq;
using System.Text.RegularExpressions;
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

        [Test]
        public void Method_Scenario_Result2()
        {
            var strings = "one,two,three".Split(',');

            Console.WriteLine(strings.Contains("one"));


            foreach (var s in strings)
            {
                if (Regex.IsMatch(s,"one"))
                {
                    Console.WriteLine(true);
                }
            }
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