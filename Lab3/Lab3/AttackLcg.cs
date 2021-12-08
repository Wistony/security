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
    }
}