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
    using Library.FenwickTree;

    /*
     *
     */

    static class Program
    {
        private const int NumberOfTestCase = 3;
        private const int StackSize = 64 * (1 << 20);
        private const int Sz = (int)2e5 + 10;
        private const int Mod = (int)1e9 + 7;
        private static int n;
        private static int q;
        private static FenwickTree<int> ft;

        private static void Solve()
        {
            n = NextInt();
            q = NextInt();
            ft = new FenwickTree<int>(n + 10, (x, y) => x + y, (x, y) => Math.Sign(x - y));
            int k;
            for (int i = 0; i < n; i++)
            {
                k = NextInt();
                ft.Update(k, 1);
            }
 
            for (int i = 0; i < q; i++)
            {
                k = NextInt();
                if (k < 0)
                {
                    k = ft.LowerBound(-k);
                    ft.Update(k, -1);
                    n--;
                }
                else
                {
                    ft.Update(k, 1);
                    n++;
                }
            }
 
            OutputPrinter.WriteLine(n == 0 ? 0 : ft.LowerBound(1));
        }

        private static int LowerBound<T1, T2>(T1 arr, int l, int r, T2 value, Func<T2, T2, int> comp)
            where T1: IList<T2>
        {
            while (r - l > 4)
            {
                int mid = l + (r - l) / 2;
                T2 sum = arr[mid];
                if (comp(sum, value) < 0)
                {
                    l = mid;
                }
                else
                {
                    r = mid;
                }
            }

            for (; l <= r; l++)
            {
                T2 sum = arr[l];
                if (comp(sum, value) == 0)
                {
                    return l;
                }
            }

            return -1;
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
