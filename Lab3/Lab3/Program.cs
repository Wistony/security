using System;
using System.Collections.Generic;

namespace Lab3
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var acc = Casino.CreateAccount();
            
            var nums = GetThreeLcgNum(acc);

            var lcg = new AttackLcg(nums);
            while (!lcg.findParameters())
            {
                nums = GetThreeLcgNum(acc);
                lcg = new AttackLcg(nums);
            }

            var predictedVal = lcg.GetPredictedValue(nums[2]);
            var i = 0;
            while (i < 10)
            {
                var bet = Casino.MakeBet("Lcg", acc.id, 100, predictedVal);
                Console.WriteLine("-----------");
                Console.WriteLine("Id: " + acc.id);
                Console.WriteLine("Message: " + bet.message);
                Console.WriteLine("Money: " + bet.account.money);
                Console.WriteLine("Predicted val: " + predictedVal);
                Console.WriteLine("Real val: " + bet.realNumber);
                predictedVal = lcg.GetPredictedValue(predictedVal);
                i++;
            }
         


        }

        private static List<int> GetThreeLcgNum(Account acc)
        {
            var nums = new List<int>(3);
            for (var i = 0; i < 3; i++)
            {
                var bet = Casino.MakeBet("Lcg", acc.id, 10, 3);
                nums.Add(bet.realNumber);
            }

            return nums;
        }
        
    }
}