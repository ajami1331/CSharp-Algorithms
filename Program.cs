using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Csharp_Contest
{
    static class Program
    {
        static void Solve()
        {
            int n = NextInt();
            for (int i = 0; i < n; i++) 
            {
                int a = NextInt();
                int b = NextInt();
                int rem = (a % b == 0) ? 0 : (((a / b) + 1) * b) - a;
                Console.WriteLine(rem);
            }
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.SetOut(new Printer(Console.OpenStandardOutput()));
            }
            var t = new Thread(Solve, 134217728); 
            t.Start(); 
            t.Join(); 
            Console.Out.Flush();
        }

        static int NextInt () => int.Parse(Console_.NextString());
        static long NextLong () => long.Parse(Console_.NextString());
        static double NextDouble () => double.Parse(Console_.NextString());
        static string NextString () => Console_.NextString();
        static string NextLine () => Console.ReadLine();
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
                    foreach (var item in NextLine().Split(' '))
                    {
                        param.Enqueue(item);
                    }
                }
                return param.Dequeue();
            }
        }
        class Printer : StreamWriter
        {
            public override IFormatProvider FormatProvider { get { return CultureInfo.InvariantCulture; } }
            public Printer(Stream stream) : base(stream, new UTF8Encoding(false, true)) { base.AutoFlush = false; }
            public Printer(Stream stream, Encoding encoding) : base(stream, encoding) { base.AutoFlush = false; }
        }
    }
}