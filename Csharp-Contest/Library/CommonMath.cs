// CommonMath.cs
// Author: Araf Al Jami
// Last Updated: 07-09-2565 03:03

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
                    ans = ans * num % mod;
                }

                num = num * num % mod;
            }

            return ans;
        }
    }
}
