// CommonMath.cs
// Authors: Araf Al-Jami
// Created: 08-07-2021 3:43 PM
// Updated: 08-07-2021 3:44 PM

namespace Library.CommonMath
{
    public class CommonMath
    {
        public static long Gcd(long x, long y)
        {
            if (x < 0)
            {
                x = -x;
            }

            if (y < 0)
            {
                y = -y;
            }

            while (x > 0 && y > 0)
            {
                if (x > y)
                {
                    x %= y;
                }
                else
                {
                    y %= x;
                }
            }

            return x | y;
        }

        public static int Gcd(int x, int y)
        {
            if (x < 0)
            {
                x = -x;
            }

            if (y < 0)
            {
                y = -y;
            }

            while (x > 0 && y > 0)
            {
                if (x > y)
                {
                    x %= y;
                }
                else
                {
                    y %= x;
                }
            }

            return x | y;
        }

        public static long ModPower(long num, long power, long mod)
        {
            long ans = 1;
            for (; power > 0; power >>= 1)
            {
                if (power % 2 == 1)
                {
                    ans = (ans * num) % mod;
                }

                num = (num * num) % mod;
            }

            return ans;
        }
    }
}
