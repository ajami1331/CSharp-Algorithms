﻿// Program.cs
// Author: Araf Al Jami
// Last Updated: 10-09-2565 20:57

namespace CLown1331
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using CLown1331.Library.PriorityQueue;

    internal static class Program
    {
        private const int NumberOfTestCase = 3;
        private const int StackSize = 32 * (1 << 20);
        private const int Sz = (int)1e5 + 10;
        private const int Mod = (int)1e9 + 7;
        private static List<KeyValuePair<int, int>>[] G;
        private static int[] parent;
        private static long[] distance;

        private static void Solve()
        {
            int n = NextInt();
            int m = NextInt();
            G = Repeat(0, n + 1).Select(_ => new List<KeyValuePair<int, int>>()).ToArray();
            parent = Repeat(-1, n + 1).ToArray();
            distance = Repeat(long.MaxValue, n + 1).ToArray();
            for (var i = 0; i < m; i++)
            {
                int u = NextInt();
                int v = NextInt();
                int c = NextInt();
                G[u].Add(new KeyValuePair<int, int>(v, c));
                G[v].Add(new KeyValuePair<int, int>(u, c));
            }

            if (Dijkstra(n))
            {
                Print(n);
                OutputPrinter.WriteLine();
            }
            else
            {
                OutputPrinter.WriteLine(-1);
            }
        }

        private static bool Dijkstra(int n)
        {
            distance[1] = 0;
            var priorityQueue = new PriorityQueue<KeyValuePair<int, long>>((x, y) => x.Value.CompareTo(y.Value));
            priorityQueue.Enqueue(new KeyValuePair<int, long>(1, 0));
            while (!priorityQueue.Empty())
            {
                KeyValuePair<int, long> u = priorityQueue.Dequeue();
                foreach (KeyValuePair<int, int> v in G[u.Key])
                {
                    if (u.Value + v.Value < distance[v.Key])
                    {
                        distance[v.Key] = u.Value + v.Value;
                        priorityQueue.Enqueue(new KeyValuePair<int, long>(v.Key, distance[v.Key]));
                        parent[v.Key] = u.Key;
                    }
                }
            }

            return parent[n] != -1;
        }

        private static void Print(int x)
        {
            if (parent[x] != -1)
            {
                Print(parent[x]);
            }

            OutputPrinter.Write(x + " ");
        }

        public static void Main(string[] args)
        {
#if CLown1331
            var stopWatch = new System.Diagnostics.Stopwatch();
            long totalTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Start();
            for (var testCase = 1; !Reader.IsEndOfStream; testCase++)
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
            if (System.Diagnostics.Debugger.IsAttached) Thread.Sleep(Timeout.Infinite);
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
                uint t = x ^ x << 11;
                x = y;
                y = z;
                z = w;
                w = w ^ w >> 19 ^ t ^ t >> 8;
                yield return w;
            }
        }

#if CLown1331
        private static void Debug<T>(
            IEnumerable<T> args,
            int len = int.MaxValue,
            [System.Runtime.CompilerServices.CallerLineNumber]
            int callerLinerNumber = default(int),
            [System.Runtime.CompilerServices.CallerMemberName]
            string callerMemberName = default(string))
        {
            var count = 0;
            foreach (T arg in args)
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
            foreach (object arg in args)
            {
                ErrorPrinter.Write(arg + " ");
            }

            ErrorPrinter.WriteLine();
        }

#endif
        private static class Reader
        {
            private static readonly Queue<string> Param = new Queue<string>();
#if CLown1331
            private static readonly StreamReader InputReader = new StreamReader("input.txt");
#else
            private static readonly StreamReader InputReader = new StreamReader(Console.OpenStandardInput());
#endif
            public static bool IsEndOfStream
            {
                get { return InputReader.EndOfStream; }
            }

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

            public static string ReadLine() => InputReader.ReadLine();

            public static int Read() => InputReader.Read();
        }

#if CLown1331
        private static readonly Printer OutputPrinter = new Printer(
            new MultiStream(
                File.Create(Path.Combine(Utils.GetRootDirectoryPath(), "output.txt")),
                Console.OpenStandardOutput()));

        private static readonly Printer ErrorPrinter = new Printer(
            new MultiStream(
                File.Create(Path.Combine(Utils.GetRootDirectoryPath(), "error.txt")),
                Console.OpenStandardOutput()));
#else
        private static readonly Printer OutputPrinter = new Printer(Console.OpenStandardOutput());

        private static readonly Printer ErrorPrinter = new Printer(Console.OpenStandardError());
#endif

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

            public override IFormatProvider FormatProvider
            {
                get { return CultureInfo.InvariantCulture; }
            }
        }
    }
}
