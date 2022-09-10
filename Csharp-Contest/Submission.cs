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
    using CLown1331.Library.PriorityQueue;

    internal static class Program
    {
        private const int NumberOfTestCase = 3;
        private const int StackSize = 32 * (1 << 20);
        private const int Sz = (int)1e5 + 10;
        private const int Mod = (int)1e9 + 7;
        private static List<KeyValuePair<int, int>>[] G;
        private static int[] parent;
        private static long[] distance;

        private static void Solve()
        {
            int n = NextInt();
            int m = NextInt();
            G = Repeat(0, n + 1).Select(_ => new List<KeyValuePair<int, int>>()).ToArray();
            parent = Repeat(-1, n + 1).ToArray();
            distance = Repeat(long.MaxValue, n + 1).ToArray();
            for (var i = 0; i < m; i++)
            {
                int u = NextInt();
                int v = NextInt();
                int c = NextInt();
                G[u].Add(new KeyValuePair<int, int>(v, c));
                G[v].Add(new KeyValuePair<int, int>(u, c));
            }

            if (Dijkstra(n))
            {
                Print(n);
                OutputPrinter.WriteLine();
            }
            else
            {
                OutputPrinter.WriteLine(-1);
            }
        }

        private static bool Dijkstra(int n)
        {
            distance[1] = 0;
            var priorityQueue = new PriorityQueue<KeyValuePair<int, long>>((x, y) => x.Value.CompareTo(y.Value));
            priorityQueue.Enqueue(new KeyValuePair<int, long>(1, 0));
            while (!priorityQueue.Empty())
            {
                KeyValuePair<int, long> u = priorityQueue.Dequeue();
                foreach (KeyValuePair<int, int> v in G[u.Key])
                {
                    if (u.Value + v.Value < distance[v.Key])
                    {
                        distance[v.Key] = u.Value + v.Value;
                        priorityQueue.Enqueue(new KeyValuePair<int, long>(v.Key, distance[v.Key]));
                        parent[v.Key] = u.Key;
                    }
                }
            }

            return parent[n] != -1;
        }

        private static void Print(int x)
        {
            if (parent[x] != -1)
            {
                Print(parent[x]);
            }

            OutputPrinter.Write(x + " ");
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
// PriorityQueue.cs
// Author: Araf Al Jami
// Last Updated: 10-09-2565 20:57

namespace CLown1331.Library.PriorityQueue
{
    using System;
    using System.Collections.Generic;

    public class PriorityQueue<T>
    {
        private readonly List<T> data;
        private readonly Comparison<T> compare;

        public PriorityQueue(Comparison<T> compare)
        {
            this.compare = compare;
            this.data = new List<T> {default(T)};
        }

        public int Count
        {
            get { return this.data.Count - 1; }
        }

        public T Peek() => this.data[1];

        public void Clear()
        {
            this.data.Clear();
            this.data.Add(default(T));
        }

        public void Enqueue(T item)
        {
            this.data.Add(item);
            int curPlace = this.Count;
            while (curPlace > 1 && this.compare(item, this.data[curPlace / 2]) < 0)
            {
                this.data[curPlace] = this.data[curPlace / 2];
                this.data[curPlace / 2] = item;
                curPlace /= 2;
            }
        }

        public T Dequeue()
        {
            T ret = this.data[1];
            this.data[1] = this.data[this.Count];
            this.data.RemoveAt(this.Count);
            var curPlace = 1;
            while (true)
            {
                int max = curPlace;
                if (this.Count >= curPlace * 2 && this.compare(this.data[max], this.data[2 * curPlace]) > 0)
                {
                    max = 2 * curPlace;
                }

                if (this.Count >= curPlace * 2 + 1 && this.compare(this.data[max], this.data[2 * curPlace + 1]) > 0)
                {
                    max = 2 * curPlace + 1;
                }

                if (max == curPlace)
                {
                    break;
                }

                T item = this.data[max];
                this.data[max] = this.data[curPlace];
                this.data[curPlace] = item;
                curPlace = max;
            }

            return ret;
        }

        public bool Empty() => this.Count == 0;
    }
}
