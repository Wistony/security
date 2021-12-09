using System;
using System.Collections.Generic;

namespace Lab3
{
    public class MersenneTwister
    {
        public const int w = 32;
        public const uint n = 624;
        public const uint m = 397;
        //public const uint r = 31;
        public const uint a = 0x9908B0DF;
        public const uint d = 0xFFFFFFFF;
        public const uint B = 0x9D2C5680;
        public const uint c = 0xEFC60000;
        public const int u = 11;
        public const int s = 7;
        public const int t = 15;
        public const int l = 18;
        public const uint f = 1812433253u;

        public const uint lower_mask = 0x7fffffff;
        public const uint upper_mask = 0x80000000;

        private uint[] MT = new uint[n];
        private uint _index = n + 1;

        public MersenneTwister(uint seed)
        {
            seed_mt(seed);
        }

        private void seed_mt(uint seed)
        {
            _index = n;
            MT[0] = seed;

            for (uint i = 1; i < n; i++)
            {
                MT[i] = f * (MT[i - 1] ^ (MT[i - 1] >> (w - 2))) + i;
            }
        }

        public void ChangeState(List<uint> states)
        {
            _index = n;
            for (var i = 0; i < n; i++)
            {
                MT[i] = Untemper(states[i]);
            }
        }

        public uint Temper()
        {
            if (_index >= n)
            {
                if (_index > n)
                {
                    throw new Exception("Generator was never seeded");
                }
                Twist();
            }

            var y = MT[_index];
            y = y ^ (y >> u);
            y = y ^ ((y << s) & B);
            y = y ^ ((y << t) & c);
            y = y ^ (y >> l);

            ++_index;

            return y;
        }

        public static uint Untemper(uint val)
        {
            var y3 = val ^ (val >> l);
            var y2 = y3 ^ ((y3 << t) & c);

            var y = val ^ (val >> l);
            y = y ^ ((y << t) & c);
            var mask = 0x7fu;
            for (var i = 0; i < 4; i++)
            {
                var b = B & (uint)(mask << (int)(7 * ((uint)(i) + 1)));
                y = y ^ ((y << s) & b);
            }

            for (var i = 0; i < 3; i++)
            {
                y = y ^ (y >> u);
            }

            return y;
        }

        private void Twist()
        {
            for (uint i = 0; i < n; i++)
            {
                var x = (MT[i] & upper_mask) + MT[(i + 1) % n] & lower_mask;
                var xA = x >> 1;

                if (x % 2 != 0)
                {
                    xA = xA ^ a;
                }

                MT[i] = MT[(i + m) % n] ^ xA;
            }

            _index = 0;
        }
    }
}