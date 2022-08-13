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
    using Library.DisjointSet;

    static class Program
    {
        private const int NumberOfTestCase = 3;
        private const int StackSize = 64 * (1 << 20);
        private const int Sz = (int)2e5 + 10;
        private const int Mod = 998244353;
        private static long[] ans;
        private static string[] type;
        private static int[] pos;
        private static int[] hCuts;
        private static int[] vCuts;
        private static long hMax;
        private static long vMax;

        private static void Solve()
        {
            ans = new long[Sz];
            type = new string[Sz];
            pos = new int[Sz];
            hCuts = new int[Sz];
            vCuts = new int[Sz];
            int w = NextInt();
            int h = NextInt();
            int n = NextInt();
            for (int i = 1; i <= n; i++)
            {
                type[i] = NextString();
                pos[i] = NextInt();
                switch (type[i])
                {
                    case "V":
                        vCuts[pos[i]] = pos[i];
                        break;
                    case "H":
                        hCuts[pos[i]] = pos[i];
                        break;
                    default:
                        break;
                }
            }

            DisjointSet vDsu = new DisjointSet(Sz);
            DisjointSet hDsu = new DisjointSet(Sz);
            hMax = long.MinValue;
            vMax = long.MinValue;
            for (int i = w; i > 0; i--)
            {
                if (vCuts[i] == 0 && i + 1 <= w)
                {
                    vDsu.MergeSet(i + 1, i);
                }

                vMax = Math.Max(vMax, vDsu.GetComponentSize(i));
            }

            for (int i = h; i > 0; i--)
            {
                if (hCuts[i] == 0 && i + 1 <= h)
                {
                    hDsu.MergeSet(i + 1, i);
                }

                hMax = Math.Max(hMax, hDsu.GetComponentSize(i));
            }

            for (int i = n; i > 0; i--)
            {
                ans[i] = hMax * vMax;
                switch (type[i])
                {
                    case "V":
                        if (pos[i] + 1 <= w)
                        {
                            vDsu.MergeSet(pos[i] + 1, pos[i]);
                        }

                        vMax = Math.Max(vMax, vDsu.GetComponentSize(pos[i]));
                        break;
                    case "H":
                        if (pos[i] + 1 <= h)
                        {
                            hDsu.MergeSet(pos[i] + 1, pos[i]);
                        }

                        hMax = Math.Max(hMax, hDsu.GetComponentSize(pos[i]));
                        break;
                    default:
                        break;
                }
            }

            for (int i = 1; i <= n; i++)
            {
                OutputPrinter.WriteLine(ans[i]);
            }
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
                ErrorPrinter.WriteLine($"Case: {testCase} -------- {stopWatch.ElapsedMilliseconds}ms");
            }

            ErrorPrinter.WriteLine($"Runtime: {totalTime}ms");
            stopWatch.Restart();
            Utils.CreateFileForSubmission(ErrorPrinter);
            if (Debugger.IsAttached) Thread.Sleep(Timeout.Infinite);
#else
            Thread t = new Thread(Solve, StackSize);
            t.Start();
            t.Join();
            OutputPrinter.Flush();
            ErrorPrinter.Flush();
#endif
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

        private static void Debug(params object[] args)
        {
            foreach (var arg in args)
            {
                ErrorPrinter.Write(arg);
                ErrorPrinter.Write(" ");
            }

            ErrorPrinter.WriteLine();
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

            public static int Read()
            {
                return InputReader.Read();
            }
        }

        private static readonly Printer OutputPrinter = new Printer(Console.OpenStandardOutput());

        private static readonly Printer ErrorPrinter = new Printer(Console.OpenStandardError());

        private sealed class Printer : StreamWriter
        {
            public Printer(Stream stream)
                : this(stream, new UTF8Encoding(false, true)) { }

            public Printer(Stream stream, Encoding encoding)
                : base(stream, encoding)
            {
#if CLown1331
                this.AutoFlush = true;
#else
                this.AutoFlush = false;
#endif
            }

            public override IFormatProvider FormatProvider => CultureInfo.InvariantCulture;
        }
    }
}
// DisjointSet.cs
// Authors: Araf Al-Jami
// Created: 26-08-2020 11:48 PM
// Updated: 08-07-2021 3:44 PM

namespace Library.DisjointSet
{
    public class DisjointSet
    {
        private int size;
        private int[] parent;
        private int[] count;

        public int NumberOfComponent { get; private set; }

        public int[] RealParent => this.parent;

        public int this[int u] => this.GetParent(u);

        public DisjointSet(int size)
        {
            this.size = size;
            this.NumberOfComponent = size;
            this.parent = new int[size];
            this.count = new int[size];
            this.Reset();
        }

        public void Reset()
        {
            for (int i = 0; i < this.size; i++)
            {
                this.count[i] = 1;
                this.parent[i] = i;
            }

            this.NumberOfComponent = this.size;
        }

        public int GetParent(int u)
        {
            if (this.parent[u] == u)
            {
                return u;
            }

            return this.parent[u] = this.GetParent(this.parent[u]);
        }

        public bool IsSameSet(int u, int v)
        {
            return this.GetParent(u) == this.GetParent(v);
        }

        public void MergeSet(int u, int v)
        {
            if (this.IsSameSet(u, v))
            {
                return;
            }

            u = this.GetParent(u);
            v = this.GetParent(v);
            if (this.count[u] < this.count[v])
            {
                (u, v) = (v, u);
            }

            this.parent[u] = this.parent[v];
            this.count[v] += this.count[u];
            this.count[u] = this.count[v];
            this.NumberOfComponent--;
        }

        public int GetComponentSize(int u)
        {
            return this.count[this[u]];
        }
    }
}
