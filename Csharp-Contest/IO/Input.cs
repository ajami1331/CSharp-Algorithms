// Input.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 17:51

namespace CLown1331.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CLown1331.Helper;

    public static class Input
    {
        public static int NextInt() => int.Parse(Reader.NextString());

        public static long NextLong() => long.Parse(Reader.NextString());

        public static double NextDouble() => double.Parse(Reader.NextString());

        public static string NextString() => Reader.NextString();

        public static string NextLine() => Reader.ReadLine();

        public static IEnumerable<T> OrderByRand<T>(this IEnumerable<T> x) => x.OrderBy(_ => Helper.XorShift);

        public static long Count<T>(this IEnumerable<T> x, Func<T, bool> pred) => Enumerable.Count(x, pred);

        public static IEnumerable<T> Repeat<T>(T v, long n) => Enumerable.Repeat<T>(v, (int)n);

        public static IEnumerable<int> Range(long s, long c) => Enumerable.Range((int)s, (int)c);
    }
}
