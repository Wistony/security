using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace Lab3
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //AttackLcg.Run();
            
            
            /*var acc = Casino.CreateAccount();
            AttackMt mtt = new AttackMt();

            var seed = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var mt = new MersenneTwister(seed);
            var bet = Casino.MakeBet("Mt", acc.id, 1000, mt.Temper());
            for (var i = 0; i < 1000; i++)
            {
                bet = Casino.MakeBet("Mt", acc.id, 1, mt.Temper());
                Console.WriteLine(bet.message);
                Console.WriteLine("MySeed " + seed);
               // mtt.Attack((uint)bet.realNumber, seed + 5);
            }
            
            if (bet.account.money >= 1000000)
            {
                Console.WriteLine("gg you win million");
            }*/

            
            
            /*
            var acc = Casino.CreateAccount();
            var states = new List<uint>();
            for (var i = 0; i < 624; i++)
            {
                Console.WriteLine(i);
                var bet = Casino.MakeBet("BetterMt", acc.id, 1, i);
                states.Add((uint)bet.realNumber);
            }

            var mt = new MersenneTwister(5);
            mt.ChangeState(states);

            for (var i = 0; i < 1000; i++)
            {
                var predictedValue = mt.Temper();
                var bet = Casino.MakeBet("BetterMt", acc.id, 5, predictedValue);
                Console.WriteLine(bet.message);
            }*/
        }
    }
}

