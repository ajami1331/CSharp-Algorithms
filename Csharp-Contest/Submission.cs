// Program.cs
// Author: Araf Al Jami
// Last Updated: 29-08-2565 21:49

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

    internal static class Program
    {
        private const int NumberOfTestCase = 3;
        private const int StackSize = 64 * (1 << 20);
        private const int Sz = (int)2e5 + 10;
        private const int MxAdd = 1000000000;
        private const int Mod = 998244353;

        private static void Solve()
        {
            var t = NextInt();
            var dy = new[] {1, 0, -1, 0};
            (int x, int y) st = (1, 1);
            for (var cs = 1; cs <= t; cs++)
            {
                int n = NextInt();
                int m = NextInt();
                int sx = NextInt();
                int sy = NextInt();
                int d = NextInt();
                (int x, int y) s = (sx, sy);
                (int x, int y) en = (n, m);
                bool possible = false;
                possible = BottomRight(n, m, sx, sy, d) || RightBottom(n, m, sx, sy, d);
                if (possible)
                {
                    OutputPrinter.WriteLine(Distance(st, en));
                }
                else
                {
                    OutputPrinter.WriteLine(-1);
                }
            }
        }

        private static bool BottomRight(int n, int m, int sx, int sy, int d)
        {
            (int x, int y) s = (sx, sy);
            for (int i = 1; i <= n; i++)
            {
                if (Distance((i, 1), s) <= d)
                {
                    return false;
                }
            }

            for (int i = 1; i <= m; i++)
            {
                if (Distance((n, i), s) <= d)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool RightBottom(int n, int m, int sx, int sy, int d)
        {
            (int x, int y) s = (sx, sy);
            for (int i = 1; i <= m; i++)
            {
                if (Distance((1, i), s) <= d)
                {
                    return false;
                }
            }

            for (int i = 1; i <= n; i++)
            {
                if (Distance((i, m), s) <= d)
                {
                    return false;
                }
            }

            return true;
        }

        private static int Distance((int x, int y) a, (int x, int y) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public static void Main(string[] args)
        {
            Thread t = new Thread(Solve, StackSize);
            t.Start();
            t.Join();
            OutputPrinter.Flush();
            ErrorPrinter.Flush();
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


        private static class Reader
        {
            private static readonly Queue<string> Param = new Queue<string>();
            private static readonly StreamReader InputReader = new StreamReader(Console.OpenStandardInput());
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

            public static string ReadLine() => InputReader.ReadLine();

            public static int Read() => InputReader.Read();
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
                this.AutoFlush = false;
            }

            public override IFormatProvider FormatProvider => CultureInfo.InvariantCulture;
        }
    }
}
