// Program.cs
// Author: Araf Al Jami
// Last Updated: 30-08-2565 23:31

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
        private static int[] ans = new int[Sz];

        private static void Solve()
        {
            int t = NextInt();
            for (var cs = 1; cs <= t; cs++)
            {
                int n = NextInt();
                int k = (n / 8) * 8;
                for (int i = 0; i < k; i++)
                {
                    ans[i] = i;
                }

                int bk = n - k;

                switch (bk)
                {
                    case 1:
                        ans[k] = NextTwo(k - 1);
                        ans[k - 1] ^= ans[k];
                        break;
                    case 2:
                        ans[k] = NextTwo(k - 1);
                        ans[k + 1] = ans[k] * 2;
                        ans[k - 1] ^= ans[k + 1];
                        ans[k + 1] ^= ans[k];
                        break;
                    case 3:
                        ans[k + 1] = NextTwo(k - 1);
                        ans[k] = ans[k + 1] * 2;
                        ans[k + 2] = ans[k + 1] ^ ans[k];
                        break;
                    case 4:
                        ans[k + 1] = NextTwo(k - 1);
                        ans[k] = ans[k + 1] * 2;
                        ans[k + 2] = ans[k + 1] ^ ans[k];
                        ans[k + 3] = ans[k] * 2;
                        ans[k + 1] ^= ans[k + 3];
                        break;
                    case 5:
                        ans[k + 1] = NextTwo(k - 1);
                        ans[k] = ans[k + 1] * 2;
                        ans[k + 2] = ans[k + 1] ^ ans[k];
                        ans[k + 3] = ans[k] * 2;
                        ans[k + 4] = ans[k + 3] * 2;
                        ans[k] ^= ans[k + 4];
                        ans[k + 1] ^= ans[k + 3];
                        break;
                    case 6:
                        ans[k + 1] = NextTwo(k - 1);
                        ans[k] = ans[k + 1] * 2;
                        ans[k + 2] = ans[k + 1] ^ ans[k];
                        ans[k + 3] = ans[k] * 2;
                        ans[k + 4] = ans[k + 3] * 2;
                        ans[k + 5] = ans[k + 4] * 2;
                        ans[k] ^= ans[k + 4];
                        ans[k + 1] ^= ans[k + 5];
                        ans[k + 1] ^= ans[k + 3];
                        break;
                    case 7:
                        ans[k + 1] = NextTwo(k - 1);
                        ans[k] = ans[k + 1] * 2;
                        ans[k + 2] = ans[k + 1] ^ ans[k];
                        ans[k + 3] = ans[k] * 2;
                        ans[k + 4] = ans[k + 3] * 2;
                        ans[k + 5] = ans[k + 4] * 2;
                        ans[k + 6] = ans[k + 5] * 2;
                        ans[k] ^= ans[k + 4];
                        ans[k] ^= ans[k + 6];
                        ans[k + 1] ^= ans[k + 5];
                        ans[k + 1] ^= ans[k + 3];
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < n; i++)
                {
                    OutputPrinter.Write(ans[i] + " ");
                }
                
                OutputPrinter.WriteLine();

                // int a = 0;
                // int b = 0;
                // for (int i = 0; i < n; i++)
                // {
                //     if (i % 2 == 1) a ^= ans[i];
                //     else b ^= ans[i];
                // }
                //
                // if (a != b)
                // Debug(bk, ": ", a, b, " ss");
            }
        }

        private static int NextTwo(int x)
        {
            int ret = 2;
            while (x > 1)
            {
                x /= 2;
                ret *= 2;
            }
            return ret;
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
