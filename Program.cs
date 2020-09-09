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
     *  #import_DisjointSet.cs
     */
    static class Program
    {
        private const int STACK_SIZE = 32 * (1 << 20);
        private const int sz = (int) 1e5 + 10;
        private static int[] ar;

        static void Solve()
        {
            int n = NextInt();
            int m = NextInt();
            DisjointSet dsu = new DisjointSet(n);
            ar = Repeat(0, n).Select((_, index) => 0).ToArray();
            while (m-- > 0)
            {
                string s = NextString();
                if (s.Equals("join"))
                {
                    int u = NextInt() - 1;
                    int v = NextInt() - 1;
                    dsu.MergeSet(u, v);
                }
                else if (s.Equals("add"))
                {
                    int u = NextInt() - 1;
                    int v = NextInt();
                    ar[dsu[u]] += v;
                }
                else
                {
                    int u = NextInt() - 1;
                    Console.WriteLine($"{Traverse(u, dsu)}");
                }
            }
        }

        private static int Traverse(int u, DisjointSet dsu)
        {
            if (dsu.RealParent[u] == u)
            {
                return ar[u];
            }

            return ar[u] + Traverse(dsu.RealParent[u], dsu);
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