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
        private const int NumberOfTestCase = 1;
        private const int StackSize = 64 * (1 << 20);
        private const int Sz = (int)2e5 + 10;
        private const int Mod = (int)1e9 + 7;
        private static int n;
        private static int[,] dp = new int[2001, 2001];
        private static List<int> ar;
        private static List<int> br = new List<int>();

        private static void Solve()
        {
            int t = NextInt();
            for (int cs = 1; cs <= t; cs++)
            {
                n = NextInt();
                ar = Repeat(0, n).Select(_ => NextInt()).ToList();
                br.Clear();
                int _xor = 0;
                br.Add(_xor);
                for (int i = 0; i < n; i++)
                {
                    _xor ^= ar[i];
                    br.Add(_xor);
                }

                for (int i = 0; i <= n; i++)
                {
                    for (int j = 0; j <= n; j++)
                    {
                        dp[i, j] = -1;
                    }
                }

                bool fl = false;
                for (int i = 0; i + 1 < n && fl == false; i++)
                {
                    fl = Possible(i + 2, i + 1) == 1;
                }

                OutputPrinter.WriteLine(fl ? "YES" : "NO");
            }
        }

        private static int Possible(int x, int p)
        {
            if (x > n)
            {
                return 1;
            }

            if (dp[x, p] != -1)
            {
                return dp[x, p];
            }

            dp[x, p] = 0;
            for (int i = x; i <= n; i++)
            {
                if ((br[i] ^ br[x - 1]) == br[p])
                {
                    dp[x, p] |= Possible(i + 1, p);
                }
            }

            return dp[x, p];
        }

        public static void Main(string[] args)
        {
#if CLown1331
            for (int testCase = 0; testCase < NumberOfTestCase; testCase++)
            {
                Solve();
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

        private static uint XorShift
        {
            get
            {
                Xsc().MoveNext();
                return Xsc().Current;
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
