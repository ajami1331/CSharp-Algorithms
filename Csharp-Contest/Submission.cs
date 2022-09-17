// Program.cs
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
    using CLown1331.Library.ZAlgorithm;

    internal static class Program
    {
        private const int NumberOfTestCase = 3;
        private const int StackSize = 32 * (1 << 20);
        private const int Sz = (int)1e5 + 10;
        private const int Mod = (int)1e9 + 7;

        private static void Solve()
        {
            int t = NextInt();
            for (int cs = 1; cs <= t; cs++)
            {
                string s = NextString();
                string m = NextString();
                ZAlgorithm<char> zAlgorithm = new ZAlgorithm<char>(m, s, '-');
                OutputPrinter.WriteLine("Case {0}: {1}", cs, zAlgorithm.Occurance[m.Length]);
            }
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

            public override IFormatProvider FormatProvider
            {
                get { return CultureInfo.InvariantCulture; }
            }
        }
    }
}
// ZAlgorithm.cs
// Author: Araf Al Jami
// Last Updated: 11-09-2565 20:09

namespace CLown1331.Library.ZAlgorithm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ZAlgorithm<T>
    {
        private readonly T[] s;
        private readonly int n;

        public int[] Z { get; }

        public int[] Occurance { get; }

        public int MaxZ { get; private set; }

        public ZAlgorithm(IEnumerable<T> a, IEnumerable<T> b, T outOf)
        {
            this.s = a.Concat(new[] {outOf}).Concat(b).ToArray();
            this.n = this.s.Length;
            this.Z = new int[this.n];
            this.Occurance = new int[this.n];
            this.Compute();
        }

        private void Compute()
        {
            var l = 0;
            var r = 0;
            for (var i = 1; i < this.n; i++)
            {
                if (i > r)
                {
                    l = r = i;
                    while (r < this.n && this.s[r - l].Equals(this.s[r]))
                    {
                        r++;
                    }

                    this.Z[i] = r - l;
                    r--;
                }
                else
                {
                    int k = i - l;
                    if (this.Z[k] < r - i + 1)
                    {
                        this.Z[i] = this.Z[k];
                    }
                    else
                    {
                        l = i;
                        while (r < this.n && this.s[r - l].Equals(this.s[r]))
                        {
                            r++;
                        }

                        this.Z[i] = r - l;
                        r--;
                    }
                }
            }

            for (int i = 1; i < this.n; i++)
            {
                this.MaxZ = Math.Max(this.MaxZ, this.Z[i]);
                this.Occurance[this.Z[i]]++;
            }
        }
    }
}
