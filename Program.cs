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
 * #import_FenwickTree.cs
 */
    static class Program
    {
        private static int mod = (int)(1e9) + 7;

        static void Solve()
        {
            int n = NextInt();
            int q = NextInt();
            char[] s = NextLine().ToCharArray();
            FenwickTree<int>[] trees = Repeat(0, 26).Select(_ => new FenwickTree<int>(n + 1, Add, CompareInt)).ToArray();
            for (int i = 0; i < s.Length; i++)
            {
                trees[s[i] - 'a'].Update(i + 1, 1);
            }

            while (q-- > 0)
            {
                int type = NextInt();
                if (type == 1)
                {
                    int index = NextInt();
                    char c = NextString().First();
                    trees[s[index - 1] - 'a'].Update(index, -1);
                    s[index - 1] = c;
                    trees[s[index - 1] - 'a'].Update(index, 1);
                }
                else
                {
                    int l = NextInt();
                    int r = NextInt();
                    char c = NextString().First();
                    Console.WriteLine(trees[c - 'a'].Query(r) - trees[c - 'a'].Query(l - 1));
                }
            }
        }

        private static int CompareInt(int arg1, int arg2)
        {
            return Math.Sign(arg1 - arg2);
        }

        private static int Add(int arg1, int arg2)
        {
            return arg1 + arg2;
        }

        public static void Main(string[] args)
        {
#if CLown1331
            for (int testCase = 0; testCase < 1; testCase++)
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

            Thread t = new Thread(Solve, 134217728);
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
        static IEnumerator<uint> _xsi = _xsc();
        static IEnumerator<uint> _xsc() { uint x = 123456789, y = 362436069, z = 521288629, w = (uint)(DateTime.Now.Ticks & 0xffffffff); while (true) { var t = x ^ (x << 11); x = y; y = z; z = w; w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)); yield return w; } }

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