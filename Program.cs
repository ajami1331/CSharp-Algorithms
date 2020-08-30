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
     *  #import_Dinic.cs
     */
    static class Program
    {
        private const int STACK_SIZE = 64 * (1 << 20);
        private const int mod = (int)(1e9) + 7;
        private const int sz = (int)4e4;

        static void Solve()
        {
            int[] ar = Repeat(0, 6).Select(_ => NextInt()).ToArray();
            int n = NextInt();
            string[] shirts = new[]
            {
                "S",
                "M",
                "L",
                "XL",
                "XXL",
                "XXXL",
            };
            Dictionary<string, int> mp = new Dictionary<string, int>();
            for (int i = 0; i < 6; i++)
            {
                mp.Add(shirts[i], i + 1);
            }

            Dinic graph = new Dinic(2 + 6 + n);
            int src = 0;
            int sink = 1 + 6 + n;
            for (int i = 0; i < 6; i++)
            {
                graph.AddEdge(src, i + 1, ar[i]);
            }

            for (int i = 1; i <= n; i++)
            {
                foreach (string s in NextLine().Split(','))
                {
                    graph.AddEdge(mp[s], i + 6, 1);
                }

                graph.AddEdge(i + 6, sink, 1);
            }

            string[] ans = new string[n + 8];

            if (graph.MaxFlow(src, sink) == n)
            {
                Console.WriteLine("YES");
                for (int i = 1; i <= 6; i++)
                {
                    foreach (int id in graph[i])
                    {
                        var e = graph.GetEdge(id);
                        if (e.flow == 1)
                        {
                            ans[e.to] = shirts[i - 1];
                        }
                    }
                }

                for (int i = 7; i <= n + 6; i++)
                {
                    Console.WriteLine(ans[i]);
                }
            }
            else
            {
                Console.WriteLine("NO");
            }
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