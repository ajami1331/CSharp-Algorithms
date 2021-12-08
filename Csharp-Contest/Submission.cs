// Program.cs
// Authors: Araf Al-Jami
// Created: 23-11-2020 2:57 AM
// Updated: 08-07-2021 3:44 PM

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
    using Library.EdmondsKarp;

    /*
     *
     */

    static class Program
    {
        private const int NumberOfTestCase = 2;
        private const int StackSize = 64 * (1 << 20);
        private const int Sz = (int)2e5 + 10;
        private const int Mod = (int)1e9 + 7;
        private static int[] from;
        private static int[] to;
        private static double[] cap;

        private static void Solve()
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

            OutputPrinter.WriteLine("{0:F16}", mid * x);
        }

        public static void Main(string[] args)
        {
#if CLown1331
            var stopWatch = new Stopwatch();
            var totalTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Start();
            for (int testCase = 0; testCase < NumberOfTestCase; testCase++)
            {
                stopWatch.Restart();
                Solve();
                stopWatch.Stop();
                totalTime += stopWatch.ElapsedMilliseconds;
                ErrorPrinter.WriteLine($"{testCase} -------- {stopWatch.ElapsedMilliseconds}ms");
            }

            ErrorPrinter.WriteLine($"Runtime: {totalTime}ms");
            stopWatch.Restart();
            Utils.CreateFileForSubmission(ErrorPrinter);
#else
            Thread t = new Thread(Solve, StackSize);
            t.Start();
            t.Join();
#endif
            OutputPrinter.Flush();
            ErrorPrinter.Flush();
            if (Debugger.IsAttached) Thread.Sleep(Timeout.Infinite);
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
// EdmondsKarp.cs
// Authors: Araf Al-Jami
// Created: 28-08-2020 9:14 PM
// Updated: 08-07-2021 3:44 PM

namespace Library.EdmondsKarp
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   <para>Implementation of Edmonds-Karp max flow algorithm. </para>
    ///   <para>Running time:<br />        O(|V|*|E|^2).</para>
    /// </summary>
    public class EdmondsKarp
    {
        private readonly int[] parents;
        private readonly int[,] graph;
        private readonly bool[] visited;

        /// <summary>Initializes a new instance of the <see cref="EdmondsKarp" /> class.</summary>
        /// <param name="nodes">The nodes.</param>
        public EdmondsKarp(int nodes)
        {
            this.Nodes = nodes;
            this.parents = new int[nodes];
            this.visited = new bool[nodes];
            this.graph = new int[nodes, nodes];
        }

        /// <summary>Gets the number of nodes.</summary>
        /// <value>The nodes.</value>
        public int Nodes { get; }

        /// <summary>Adds the edge.</summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bidirectional">if set to <c>true</c> [bidirectional].</param>
        public void AddEdge(int from, int to, int capacity, bool bidirectional)
        {
            this.graph[from, to] += capacity;
            if (bidirectional)
            {
                this.graph[to, from] += capacity;
            }
        }

        /// <summary>Sets the edge capacity.</summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bidirectional">if set to <c>true</c> [bidirectional].</param>
        public void SetEdgeCap(int from, int to, int capacity, bool bidirectional)
        {
            this.graph[from, to] = capacity;
            if (bidirectional)
            {
                this.graph[to, from] = capacity;
            }
        }

        /// <summary>  Computes the maximum flow.</summary>
        /// <param name="src">The source.</param>
        /// <param name="sink">The sink.</param>
        /// <returns>The maximum flow.</returns>
        public int MaxFlow(int src, int sink)
        {
            int maxFlow = 0;
            while (this.Bfs(src, sink))
            {
                int minCap;
                this.AugmentPath(minCap = this.MinValue(sink), sink);
                maxFlow += minCap;
            }

            return maxFlow;
        }

        private bool Bfs(int src, int sink)
        {
            for (int i = 0; i < this.Nodes; i++)
            {
                this.visited[i] = false;
                this.parents[i] = -1;
            }

            this.visited[src] = true;
            Queue<int> q = new Queue<int>();
            q.Enqueue(src);
            while (q.Count > 0)
            {
                int u = q.Dequeue();
                if (u == sink)
                {
                    return true;
                }

                for (int i = 0; i < this.Nodes; i++)
                {
                    if (this.graph[u, i] > 0 && !this.visited[i])
                    {
                        q.Enqueue(i);
                        this.visited[i] = true;
                        this.parents[i] = u;
                    }
                }
            }

            return this.parents[sink] != -1;
        }

        private int MinValue(int i)
        {
            int ret = int.MaxValue;
            for (; this.parents[i] != -1; i = this.parents[i])
            {
                ret = Math.Min(ret, this.graph[this.parents[i], i]);
            }

            return ret;
        }

        private void AugmentPath(int value, int i)
        {
            for (; this.parents[i] != -1; i = this.parents[i])
            {
                this.graph[this.parents[i], i] -= value;
                this.graph[i, this.parents[i]] += value;
            }
        }
    }
}
