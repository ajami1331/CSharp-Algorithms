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
     *  #import_PushRelabel.cs
     */
    static class Program
    {
        private const int mod = (int)(1e9) + 7;
        private const int sz = (int)4e4;
        private static List<int> primes;
        private static int totalV;
        private static int[] factor;
        private static List<int>[] G;

        static void Solve()
        {
            int n = NextInt();
            int m = NextInt();
            G = Repeat(0, n).Select(_ => new List<int>()).ToArray();
            primes = Sieve(sz);
            factor = new int[sz];
            totalV = 0;
            int src = 0;
            for (int i = 0; i < n; i++)
            {
                int x = NextInt();
                Factorise(i, x);
            }

            int sink = totalV + 1;
            PushRelabel flowGraph = new PushRelabel(sink + 1);

            while (m-- > 0)
            {
                int x = NextInt() - 1;
                int y = NextInt() - 1;
                if (x % 2 == 1)
                {
                    (x, y) = (y, x);
                }

                foreach (int u in G[x])
                {
                    foreach (int v in G[y])
                    {
                        if (factor[u] == factor[v])
                        {
                            flowGraph.AddEdge(u, v, 1);
                        }
                    }
                }

            }

            for (int i = 0; i < n; i++)
            {
                foreach (int u in G[i])
                {
                    flowGraph.AddEdge(i % 2 == 1 ? u : src, i % 2 == 1 ? sink : u, 1);
                }
            }

            Console.WriteLine("{0}", flowGraph.MaxFlow(src, sink));
        }

        private static void Factorise(int i, int num)
        {
            foreach (int x in primes)
            {
                while (num % x == 0)
                {
                    num /= x;
                    totalV++;
                    G[i].Add(totalV);
                    factor[totalV] = x;
                }
            }

            if (num > 1)
            {
                totalV++;
                G[i].Add(totalV);
                factor[totalV] = num;
            }
        }

        static List<int> Sieve(int n)
        {
            List<int> pr = new List<int>();
            bool[] fl = new bool[n];
            for (int i = 4; i < n; i += 2)
            {
                fl[i] = true;
            }

            for (int i = 3; i * i < n; i += 2)
            {
                if (fl[i])
                {
                    continue;
                }

                for (int j = i * i; j < n; j += i)
                {
                    fl[j] = true;
                }
            }

            pr.Add(2);
            for (int i = 3; i < n; i += 2)
            {
                if (fl[i])
                {
                    continue;
                }

                pr.Add(i);
            }

            return pr;
        }

        public static void Main(string[] args)
        {
#if CLown1331
            for (int testCase = 0; testCase < 3; testCase++)
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