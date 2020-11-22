namespace Csharp_Contest
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
        private const int STACK_SIZE = 64 * (1 << 20);
        private const int sz = (int)1e5 + 10;
        private static long[] ar;

        private static void Solve()
        {
            int t = NextInt();
            while (t-- > 0)
            {
                int n = NextInt();
                int k = NextInt();
                string a = NextLine();
                string b = NextLine();
                Dictionary<char, int> aDict = new Dictionary<char, int>();
                Dictionary<char, int> bDict = new Dictionary<char, int>();
                for (char c = 'a'; c <= 'z'; c++)
                {
                    aDict.Add(c, 0);
                    bDict.Add(c, 0);
                }

                for (int i = 0; i < n; i++)
                {
                    aDict[a[i]]++;
                    bDict[b[i]]++;
                }

                bool fl = true;

                for (char c = 'a'; c <= 'z'; c++)
                {
                    int h = aDict[c] - bDict[c];
                    if (h < 0)
                    {
                        fl = false;
                        break;
                    }

                    if (c != 'z')
                    {
                        int nA = (h % k) + bDict[c];
                        aDict[(char)(c + 1)] += aDict[c] - nA;
                        aDict[c] = nA;
                    }
                }

                for (char c = 'a'; c <= 'z'; c++)
                {
                    if (aDict[c] != bDict[c])
                    {
                        fl = false;
                    }
                }

                Console.WriteLine(fl ? "Yes" : "No");
            }
        }

        public static void Main(string[] args)
        {
#if CLown1331
            for (int testCase = 0; testCase < 1; testCase++)
            {
                Solve();
            }

            Utils.CreateFileForSubmission();
            if (Debugger.IsAttached) Thread.Sleep(Timeout.Infinite);
#else
                if (args.Length == 0)
                {
                    Console.SetOut(new Printer(Console.OpenStandardOutput()));
                }
     
                Thread t = new Thread(Solve, STACK_SIZE);
                t.Start();
                t.Join();
                Console.Out.Flush();
#endif
        }

        static int NextInt() => int.Parse(Console_.NextString());
        static long NextLong() => long.Parse(Console_.NextString());
        static double NextDouble() => double.Parse(Console_.NextString());
        static string NextString() => Console_.NextString();
        static string NextLine() => Console.ReadLine();
        static IEnumerable<T> OrderByRand<T>(this IEnumerable<T> x) => x.OrderBy(_ => xorshift);
        static long Count<T>(this IEnumerable<T> x, Func<T, bool> pred) => Enumerable.Count(x, pred);
        static IEnumerable<T> Repeat<T>(T v, long n) => Enumerable.Repeat<T>(v, (int)n);
        static IEnumerable<int> Range(long s, long c) => Enumerable.Range((int)s, (int)c);
        static uint xorshift { get { _xsi.MoveNext(); return _xsi.Current; } }
        static IEnumerator<uint> _xsi = _xsc(); static IEnumerator<uint> _xsc() { uint x = 123456789, y = 362436069, z = 521288629, w = (uint)(DateTime.Now.Ticks & 0xffffffff); while (true) { var t = x ^ (x << 11); x = y; y = z; z = w; w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)); yield return w; } }

        static class Console_
        {
            static Queue<string> param = new Queue<string>();
            public static string NextString()
            {
                if (param.Count == 0)
                {
                    foreach (string item in NextLine().Split(' '))
                    {
                        if (string.IsNullOrWhiteSpace(item))
                        {
                            continue;
                        }

                        param.Enqueue(item);
                    }
                }
                return param.Dequeue();
            }
        }
        class Printer : StreamWriter
        {
            public override IFormatProvider FormatProvider => CultureInfo.InvariantCulture;
            public Printer(Stream stream) : base(stream, new UTF8Encoding(false, true)) { base.AutoFlush = false; }
            public Printer(Stream stream, Encoding encoding) : base(stream, encoding) { base.AutoFlush = false; }
        }
    }
}