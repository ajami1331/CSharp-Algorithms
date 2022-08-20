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

    static class Program
    {
        private const int NumberOfTestCase = 3;
        private const int StackSize = 64 * (1 << 20);
        private const int Sz = (int) 2e5 + 10;
        private const int MxAdd = 1000000000;
        private const int Mod = 998244353;
        private static int[] ar = new int[Sz];
        private static List<int>[] G;
        private static int[] x = new int[Sz];
        private static int[] y = new int[Sz];
        private static int[] v = new int[Sz];
        private static int[] ans = new int[Sz];
        private static bool[] mk = new bool[Sz];
        private static bool[] vis = new bool[Sz];
        private static int n;
        private static int m;

        private static void Solve()
        {
            int t = 1;
            for (int cs = 1; cs <= t; cs++)
            {
                n = NextInt();
                m = NextInt();
                G = Repeat(0, n + 2).Select(_ => new List<int>()).ToArray();
                for (int i = 0; i <= n; i++)
                {
                    ar[i] = (1 << 30) - 1;
                    ans[i] = 0;
                    mk[i] = false;
                    vis[i] = false;
                }

                for (int i = 1; i <= m; i++)
                {
                    x[i] = NextInt();
                    y[i] = NextInt();
                    v[i] = NextInt();
                    G[Math.Max(x[i], y[i])].Add(Math.Min(x[i], y[i]));
                    ar[x[i]] &= v[i];
                    ar[y[i]] &= v[i];
                    mk[x[i]] = true;
                    mk[y[i]] = true;
                }

                for (int i = 0; i <= n; i++)
                {
                    if (mk[i])
                    {
                        continue;
                    }

                    ar[i] = 0;
                }

                for (int i = n; i > 0; i--)
                {
                    if (ar[i] == 0)
                    {
                        continue;
                    }

                    Dfs(u: i, value: 0);
                }

                for (int i = 1; i <= n; i++)
                {
                    OutputPrinter.Write($"{ans[i]} ");
                }

                OutputPrinter.WriteLine();
            }
        }

        private static void Dfs(int u, int value)
        {
            vis[u] = true;
            ans[u] = ar[u] & ~value;
            value |= ar[u];
            ar[u] = ans[u];
            foreach (int v in G[u].OrderByDescending(xv => xv))
            {
                if (vis[v])
                {
                    continue;
                }

                Dfs(v, value);
            }
        }

        public static void Main(string[] args)
        {
#if CLown1331
            var stopWatch = new Stopwatch();
            var totalTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Start();
            for (int testCase = 1; !Reader.IsEndOfStream; testCase++)
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

        private static IEnumerable<T> Repeat<T>(T v, long n) => Enumerable.Repeat<T>(v, (int) n);

        private static IEnumerable<int> Range(long s, long c) => Enumerable.Range((int) s, (int) c);

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
            uint x = 123456789, y = 362436069, z = 521288629, w = (uint) (DateTime.Now.Ticks & 0xffffffff);
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

        private static void Debug<T>(
            IEnumerable<T> args,
            int len = int.MaxValue,
            [System.Runtime.CompilerServices.CallerLineNumber]
            int callerLinerNumber = default,
            [System.Runtime.CompilerServices.CallerMemberName]
            string callerMemberName = default)
        {
            int count = 0;
            foreach (var arg in args)
            {
                ErrorPrinter.Write(arg + " ");
                if (++count >= len)
                {
                    break;
                }
            }

            ErrorPrinter.WriteLine($"Method: {callerMemberName} Line: {callerLinerNumber}");
        }

        private static void Debug(params object[] args)
        {
            foreach (var arg in args)
            {
                ErrorPrinter.Write(arg + " ");
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
            public static bool IsEndOfStream => InputReader.EndOfStream;

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
                : this(stream, new UTF8Encoding(false, true))
            {
            }

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
