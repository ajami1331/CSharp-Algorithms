namespace Csharp_Contest
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Diagnostics;

    static class Program
    {
        private const long Mod = (long)(1e9 + 7);

        static void Solve()
        {
            int n = NextInt();
            int[] types = new int[n];
            int[] spellPowers = new int[n];
            FenwickTree<long> treeSum = new FenwickTree<long>(
                n + 1, 
                (long l, long r) => l + r, 
                (long l, long r) => Math.Sign(l - r));
            FenwickTree<int> treeCount = new FenwickTree<int>(
                n + 1, 
                (int l, int r) => l + r, 
                (int l, int r) => Math.Sign(l - r));
            SortedSet<int> lightingSpells = new SortedSet<int>();
            List<int> spells = new List<int>();
            Dictionary<int, int> compressed = new Dictionary<int, int>();
            for (int i = 0; i < n; i++)
            {
                types[i] = NextInt();
                spellPowers[i] = NextInt();
                spells.Add(Math.Abs(spellPowers[i]));
            }

            spells = spells.OrderBy(x => x).Distinct().ToList();

            for (int i = 0; i < spells.Count; i++)
            {
                compressed[spells[i]] = i + 1;
                compressed[-spells[i]] = i + 1;
            }

            for (int i = 0; i < n; i++)
            {
                int type = types[i];
                int spellPower = spellPowers[i];
                if (spellPower > 0)
                {
                    treeCount.Update(compressed[spellPower], 1);
                    treeSum.Update(compressed[spellPower], spellPower);
                    if (type == 1)
                    {
                        lightingSpells.Add(spellPower);
                    }
                }
                else
                {
                    if (type == 1)
                    {
                        lightingSpells.Remove(-spellPower);
                    }
                    treeCount.Update(compressed[spellPower], -1);
                    treeSum.Update(compressed[spellPower], spellPower);
                }

                int smallestLightingSpell = lightingSpells.FirstOrDefault();

                if (smallestLightingSpell > 0)
                {
                    treeCount.Update(compressed[smallestLightingSpell], -1);
                    treeSum.Update(compressed[smallestLightingSpell], -smallestLightingSpell);
                }

                long ans = smallestLightingSpell;

                long totalSum = treeSum.Query(n);

                int totalSpell = treeCount.Query(n);

                if (totalSpell > 0)
                {
                    int doubleCount = Math.Min(totalSpell, lightingSpells.Count);

                    int singleCount = totalSpell - doubleCount;

                    int index = treeCount.LowerBound(singleCount);

                    ans = ans + 2 * totalSum - treeSum.Query(index);
                }

                Console.WriteLine(ans);

                if (smallestLightingSpell > 0)
                {
                    treeCount.Update(compressed[smallestLightingSpell], 1);
                    treeSum.Update(compressed[smallestLightingSpell], smallestLightingSpell);
                }
            }
        }

        class FenwickTree<T> 
        {
            private readonly T[] tree;
            private readonly int size;
            private readonly Func<T, T, T> func;
            private readonly int logn;
            private Func<T, T, int> comp;

            public FenwickTree(int size, Func<T, T, T> func, Func<T, T, int> comp)
            {
                this.size = size;
                this.tree = new T[size];
                this.func = func;
                this.logn = (int) Math.Floor(Math.Log(this.size, 2));
                this.comp = comp;
            }

            public void Update(int index, T value)
            {
                for (; index < size; index += index & -index)
                {
                    this.tree[index] = this.func(this.tree[index], value);
                }
            }

            public T Query(int index)
            {
                T ret = default(T);
                for (; index > 0; index -= index & -index)
                {
                    ret = this.func(ret, this.tree[index]);
                }

                return ret;
            }

            public int LowerBound(T value)
            {
                int l = 1;
                int r = this.size - 1;
                while (r - l > 4)
                {
                    int mid = l + (r - l) / 2;
                    T sum = this.Query(mid);
                    if (this.comp(sum, value) < 0)
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
                    T sum = this.Query(l);
                    if (this.comp(sum, value) == 0)
                    {
                        return l;
                    }
                }

                return -1;
            }
        }

        public static void Main(string[] args)
        {
#if CLown1331
            for (int testCase = 0; testCase < 1; testCase++)
            {
                Solve();
            }
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
#if CLown1331
            if (Debugger.IsAttached) Thread.Sleep(Timeout.Infinite);
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