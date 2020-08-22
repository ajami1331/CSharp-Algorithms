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
 * #import_UnsignedMatrix.cs
 */
    static class Program
    {
        private static int mod = (int)(1e9) + 7;
        private static readonly int[] Dx = { 1, 1, 2, 2, -1, -1, -2, -2 };
        private static readonly int[] Dy = { 2, -2, 1, -1, 2, -2, 1, -1 };

        static void Solve()
        {
            long n = NextLong();
            UnsignedMatrix matrix = new UnsignedMatrix(8 * 8 + 1);
            matrix.Reset();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int x = (i * 8) + j;
                    matrix[x, 64] = 1;
                    for (int k = 0; k < Dx.Length; k++)
                    {
                        int nx = i + Dx[k];
                        int ny = j + Dy[k];
                        if (nx >= 0 && nx < 8 && ny >= 0 && ny < 8)
                        {
                            int y = (nx * 8) + ny;
                            matrix[x, y] = 1;
                            matrix[y, x] = 1;
                        }
                    }
                }
            }

            matrix[64, 64] = 1;
            var ans = matrix ^ n + 1;
            Console.WriteLine(ans[0, 64]);
        }

        public static void Main(string[] args)
        {
#if CLown1331
            for (int testCase = 0; testCase < 3; testCase++)
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