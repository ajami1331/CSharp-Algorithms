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

    /*
     *
     */

    static class Program
    {
        private const int NumberOfTestCase = 2;
        private const int StackSize = 64 * (1 << 20);
        private const int Sz = (int)2e5 + 10;
        private const int Mod = (int)1e9 + 7;
        private static int n;
        private static int foundCount;
        private static int[] cnt;
        private static int[] ar;
        private static int maxLen;
        private static StringBuilder sb;
        private static LinkedList<string>[] ans;

        private static void Solve()
        {
            n = NextInt();
            cnt = new int[1005];
            maxLen = 0;
            ar = Repeat(0, n).Select(_ =>
            {
                int x = NextInt();
                maxLen = Math.Max(maxLen, x);
                cnt[x]++;
                return x;
            }).ToArray();
            sb = new StringBuilder();
            ans = Repeat(0, 1005).Select(_ => new LinkedList<string>()).ToArray();
            foundCount = 0;
            Traverse();
            if (foundCount == n)
            {
                OutputPrinter.WriteLine("YES");
                foreach (var x in ar)
                {
                    OutputPrinter.WriteLine(ans[x].First());
                    ans[x].RemoveFirst();
                }
            }
            else
            {
                OutputPrinter.WriteLine("NO");
            }
        }

        private static void Traverse()
        {
            if (sb.Length > maxLen || foundCount == n)
            {
                return;
            }

            if (cnt[sb.Length] > 0)
            {
                cnt[sb.Length]--;
                foundCount++;
                ans[sb.Length].AddLast(sb.ToString());
                return;
            }

            sb.Append('0');
            Traverse();
            sb.Remove(sb.Length - 1, 1);
            sb.Append('1');
            Traverse();
            sb.Remove(sb.Length - 1, 1);
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
