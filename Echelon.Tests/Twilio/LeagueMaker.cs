using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Echelon.Tests.Twilio
{
    public class Team
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }
    }

    public class League
    {
        public List<Team> Teams { get; set; }

        public League()
        {
            Teams = new List<Team>();
        }
    }

    public class LeagueMaker
    {
        public List<string> Players;

        [Test]
        public void Method_Scenario_Result()
        {
            var league = new League();
            Players = new List<string> {"Simon", "Stefan", "Niall", "Eoin", "Laurent", "Domhnall"};
            while (Players.Count > 0)
            {
                league.Teams.Add(new Team {Player1 = Players.PickRandom(), Player2 = Players.PickRandom()});
            }

            while (league.Teams.Count >= 2)
            {
                var pickRandom = league.Teams.PickRandom();
                var random = league.Teams.PickRandom();
                Console.WriteLine(
                    $"{pickRandom.Player1} and {pickRandom.Player2} Versus {random.Player1} and {random.Player2}");
            }
        }
    }


    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this List<T> source)
        {
            var pickRandom = source.PickRandom(1).Single();
            source.Remove(pickRandom);
            return pickRandom;
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            var pickRandom = source.Shuffle().Take(count);
            return pickRandom;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}