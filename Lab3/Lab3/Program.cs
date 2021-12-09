using System;
using System.Collections.Generic;

namespace Lab3
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //AttackLcg.Run();
            var acc = Casino.CreateAccount();
            var startTime = (ulong) DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 3600;
            var bet = Casino.MakeBet("Mt", acc.id, 10, 5);
            var endTime = (ulong) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
           // var seed = DateTimeOffset.Parse(bet.).ToUnixTimeSeconds(); //- 3600;
            Console.WriteLine(endTime - startTime);
            for (ulong i = 0; i < endTime - startTime; i++)
            {
                var seed = startTime + i;
                var mt = new MersenneTwister(seed);
                for (var j = 0; j < 634; j++)
                {
                    var num = mt.extract_number();
                    if (num == bet.realNumber)
                    {
                        Console.WriteLine("GG");
                    }
                }
            }
            Console.WriteLine("((");
        }
    }
}