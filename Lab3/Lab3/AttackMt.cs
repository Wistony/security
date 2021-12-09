using System;

namespace Lab3
{
    public class AttackMt
    {
        public void Attack(uint realNumber, uint s)
        {
            var seed = s;
            var isTrue = true;
            while (isTrue)
            {
                var mt = new MersenneTwister(seed); 
                for (var j = 0; j < 624; j++)
                {
                    var num = mt.Temper();
                    if (num == realNumber)
                    {
                        Console.WriteLine("Real seed: " + seed);
                        isTrue = false;
                    }
                }
                seed--;
            }
        }
    }
}