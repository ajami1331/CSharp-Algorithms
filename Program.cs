﻿using System;
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
            string s = Console.ReadLine();
            int q = NextInt();
            int n = s.Length;
            SegmentTree tree = new SegmentTree(n, s);
            int ty, l, r, qr;
            while ((q--) > 0)
            {
                ty = NextInt();
                if (ty == 1)
                {
                    l = NextInt();
                    tree.Update(1, 0, n - 1, l - 1, 1 << (NextString()[0] - 'a'));
                }
                else
                {
                    l = NextInt();
                    r = NextInt();
                    qr = tree.Query(1, 0, n - 1, l - 1, r - 1);
                    Console.WriteLine(BitCount(qr));
                }
            }
        }

        static int BitCount(int x)
        {
            int ret = 0;
            while (x > 0)
            {
                ret += (x & 1);
                x /= 2;
            }
            return ret;
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

        class SegmentTree
        {
            int[] tree;
            int nodeCount;

            public SegmentTree(int nodeCount, string s)
            {
                tree = new int[nodeCount * 4];
                this.nodeCount = nodeCount;
                Build(1, 0, nodeCount - 1, s);
            }

            private void Build(int node, int b, int e, string s)
            {
                if (b == e)
                {
                    tree[node] = 1 << (s[b] - 'a');
                    return;
                }
                int left = node << 1;
                int right = left | 1;
                int mid = (b + e) >> 1;
                Build(left, b, mid, s);
                Build(right, mid + 1, e, s);
                tree[node] = tree[left] | tree[right];
            }

            public void Update(int node, int b, int e, int idx, int x)
            {
                if (b == e)
                {
                    tree[node] = x;
                    return;
                }
                int left = node << 1;
                int right = left | 1;
                int mid = (b + e) >> 1;
                if (idx <= mid) Update(left, b, mid, idx, x);
                else Update(right, mid + 1, e, idx, x);
                tree[node] = tree[left] | tree[right];
            }

            public int Query(int node, int b, int e, int l, int r)
            {
                if (r < b || e < l) return 0;
                if (b >= l && e <= r)
                {
                    return tree[node];
                }

                int left = node << 1;
                int right = left | 1;
                int mid = (b + e) >> 1;

                int p1 = Query(left, b, mid, l, r);
                int p2 = Query(right, mid + 1, e, l, r);

                return p1 | p2;
            }
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