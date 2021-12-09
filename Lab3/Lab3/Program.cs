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
            var acc = Casino.CreateAccount();
            AttackMt mtt = new AttackMt();

            var seed = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds()p;
            var mt = new MersenneTwister(seed);
            var bet = Casino.MakeBet("Mt", acc.id, 1000, mt.extract_number());
            for (var i = 0; i < 624; i++)
            {
                Console.WriteLine(bet.message);
                Console.WriteLine("MySeed " + seed);
                mtt.Attack((uint)bet.realNumber, seed + 5);
            }
            
            if (bet.account.money >= 1000000)
            {
                Console.WriteLine("gg you win million");
            }
        }
    }
}

