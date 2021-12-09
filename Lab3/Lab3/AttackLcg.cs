using System;
using System.Collections.Generic;
using System.Net;

namespace Lab3
{
    public class AttackLcg
    {
        private int Modulus = (int) Math.Pow(2, 32);
        private int A;
        private int C;

        private int x;
        private int y;
        private int z;

        public AttackLcg(List<int> nums)
        {
            this.x = nums[0];
            this.y = nums[1];
            this.z = nums[2];
        }
        

        public bool findParameters()
        {
            var paramsIsFound = false;
            var z_y = z - y;
            var y_x = y - x;

            var (gcd, inverse, _) = ExtendedEuclideanAlgorithm.GetGcd(y_x, Modulus);
            if (gcd == 1)
            {
                Console.WriteLine("Yeah! Hacking!");
                A = (z_y * inverse) % Modulus;
                C = (y - A * x) % Modulus;
                paramsIsFound = true;
            }
            else
            {
                Console.WriteLine("A and M is not coprime");
            }

            return paramsIsFound;
        }

        public int GetPredictedValue(int prevNum)
        {
            Console.WriteLine("C - " + C);
            Console.WriteLine("A - " + A);
            return (A * prevNum + C) % Modulus;
        }

        public static void Run()
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
                nums.Add((int)bet.realNumber);
            }

            return nums;
        }
    }
}