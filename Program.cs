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
     *  #import_EdmondsKarp.cs
     */
    static class Program
    {
        private static int mod = (int)(1e9) + 7;
        private static int[] from;
        private static int[] to;
        private static double[] cap;

        static void Solve()
        {
            int n = NextInt();
            int m = NextInt();
            int x = NextInt();
            EdmondsKarp flowGraph = new EdmondsKarp(n);
            from = new int[m];
            to = new int[m];
            cap = new double[m];
            for (int i = 0; i < m; i++)
            {
                from[i] = NextInt();
                to[i] = NextInt();
                cap[i] = NextInt();
                --from[i];
                --to[i];
            }

            double lo = 1.0 / x, hi = 1e9, mid = 0;
            int flow;

            for (int iter = 0; iter < 128; iter++)
            {
                mid = (lo + hi) / 2;
                for (int j = 0; j < m; j++)
                {
                    flowGraph.SetEdgeCap(from[j], to[j], (int)(cap[j] / mid), false);
                }

                flow = flowGraph.MaxFlow(0, n - 1);
                if (flow >= x)
                {
                    lo = mid;
                }
                else
                {
                    hi = mid;
                }
            }

            Console.WriteLine("{0:F16}", mid * x);
        }

        public static void Main(string[] args)
        {
#if CLown1331
            for (int testCase = 0; testCase < 2; testCase++)
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

            Thread t = new Thread(Solve, 134217728);
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
        static IEnumerator<uint> _xsi = _xsc();
        static IEnumerator<uint> _xsc() { uint x = 123456789, y = 362436069, z = 521288629, w = (uint)(DateTime.Now.Ticks & 0xffffffff); while (true) { var t = x ^ (x << 11); x = y; y = z; z = w; w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)); yield return w; } }

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