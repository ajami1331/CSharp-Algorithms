namespace CLown1331
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    /*
     *
     */

    static class Program
    {
        private const int NumberOfTestCase = 3;
        private const int StackSize = 256 * (1 << 20);
        private const int Sz = (int)2e5 + 10;
        private const int Mod = (int)1e9 + 7;
        private static int n;
        private static int m;
        private static int k;
        private static int[,] ij1;
        private static int[,] i1j;
        private static int[,,] dp;
        private static int[,,] vp;
        private static int vv;
        private static int mx = (int) 1e8;

        private static void Solve()
        {
            n = NextInt();
            m = NextInt();
            k = NextInt();
            i1j = new int[n, m];
            ij1 = new int[n, m];
            dp = new int[505, 505, 22];
            vp = new int[505, 505, 22];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j + 1 < m; j++)
                {
                    ij1[i, j] = NextInt();
                }
            }

            for (int i = 0; i + 1 < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    i1j[i, j] = NextInt();
                }
            }

            int ans;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    vv = 1;
                    ans = Recur(k, i, j);
                    OutputPrinter.Write(ans >= mx ? -1 : ans);
                    OutputPrinter.Write(' ');
                }

                OutputPrinter.Write('\n');
            }
        }

        private static int Recur(int kv, int ox, int oy)
        {
            if (ox < 0 || ox >= n || oy < 0 || oy >= m || kv < 0 || kv % 2 == 1)
            {
                return mx;
            }

            if (kv == 0)
            {
                return 0;
            }

            if (vp[ox, oy, kv] == vv)
            {
                return dp[ox, oy, kv];
            }

            vp[ox, oy, kv] = vv;
            dp[ox, oy, kv] = mx;

            if (oy + 1 < m)
            {
                dp[ox, oy, kv] = Math.Min(dp[ox, oy, kv], Recur(kv - 2, ox, oy + 1) + ij1[ox, oy] * 2);
            }

            if (oy > 0)
            {
                dp[ox, oy, kv] = Math.Min(dp[ox, oy, kv], Recur(kv - 2, ox, oy - 1) + ij1[ox, oy - 1] * 2);
            }

            if (ox + 1 < n)
            {
                dp[ox, oy, kv] = Math.Min(dp[ox, oy, kv], Recur(kv - 2, ox + 1, oy) + i1j[ox, oy] * 2);
            }

            if (ox > 0)
            {
                dp[ox, oy, kv] = Math.Min(dp[ox, oy, kv], Recur(kv - 2, ox - 1, oy) + i1j[ox - 1, oy] * 2);
            }

            return dp[ox, oy, kv];
        }

        public static void Main(string[] args)
        {
#if CLown1331
            for (int testCase = 0; testCase < NumberOfTestCase; testCase++)
            {
                Solve();
                OutputPrinter.WriteLine("--------");
            }

            Utils.CreateFileForSubmission();
            if (Debugger.IsAttached) Thread.Sleep(Timeout.Infinite);
#else
            Thread t = new Thread(Solve, StackSize);
            t.Start();
            t.Join();
#endif
            OutputPrinter.Flush();
            ErrorPrinter.Flush();
        }

        private static int NextInt() => int.Parse(Reader.NextString());

        private static long NextLong() => long.Parse(Reader.NextString());

        private static double NextDouble() => double.Parse(Reader.NextString());

        private static string NextString() => Reader.NextString();

        private static string NextLine() => Reader.ReadLine();

        private static IEnumerable<T> OrderByRand<T>(this IEnumerable<T> x) => x.OrderBy(_ => XorShift);

        private static long Count<T>(this IEnumerable<T> x, Func<T, bool> pred) => Enumerable.Count(x, pred);

        private static IEnumerable<T> Repeat<T>(T v, long n) => Enumerable.Repeat<T>(v, (int)n);

        private static IEnumerable<int> Range(long s, long c) => Enumerable.Range((int)s, (int)c);

        private static readonly IEnumerator<uint> Xsi = Xsc();

        private static uint XorShift
        {
            get
            {
                Xsi.MoveNext();
                return Xsi.Current;
            }
        }

        private static IEnumerator<uint> Xsc()
        {
            uint x = 123456789, y = 362436069, z = 521288629, w = (uint)(DateTime.Now.Ticks & 0xffffffff);
            while (true)
            {
                uint t = x ^ (x << 11);
                x = y;
                y = z;
                z = w;
                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
                yield return w;
            }
        }

        private static class Reader
        {
            private static readonly Queue<string> Param = new Queue<string>();
#if CLown1331
            private static readonly StreamReader InputReader = new StreamReader("input.txt");
#else
            private static readonly StreamReader InputReader = new StreamReader(Console.OpenStandardInput());
#endif

            public static string NextString()
            {
                if (Param.Count == 0)
                {
                    foreach (string item in ReadLine().Split(' '))
                    {
                        if (string.IsNullOrWhiteSpace(item))
                        {
                            continue;
                        }

                        Param.Enqueue(item);
                    }
                }

                return Param.Dequeue();
            }

            public static string ReadLine()
            {
                return InputReader.ReadLine();
            }
        }

        private static readonly Printer OutputPrinter = new Printer(Console.OpenStandardOutput());

        private static readonly Printer ErrorPrinter = new Printer(Console.OpenStandardError());

        private sealed class Printer : StreamWriter
        {
            public Printer(Stream stream)
                : base(stream, new UTF8Encoding(false, true))
            {
                this.AutoFlush = false;
            }

            public Printer(Stream stream, Encoding encoding)
                : base(stream, encoding)
            {
                this.AutoFlush = false;
            }

            public override IFormatProvider FormatProvider => CultureInfo.InvariantCulture;
        }
    }
}
