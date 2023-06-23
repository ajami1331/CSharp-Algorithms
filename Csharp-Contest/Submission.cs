// Program.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 18:04

namespace CLown1331
{
    using CLown1331.IO;
    using CLown1331.Solution;

    internal static class Program
    {
        public static void Main(string[] args)
        {
            var solution = new TaskSolution();
            Thread t = new Thread(solution.Solve, TaskSolution.StackSize);
            t.Start();
            t.Join();
            Output.OutputPrinter.Flush();
            Output.ErrorPrinter.Flush();
        }
    }
}
// Output.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 17:50

namespace CLown1331.IO
{
    using System;
    using System.IO;

    public static class Output
    {
        public static readonly Printer OutputPrinter = new Printer(Console.OpenStandardOutput());

        public static readonly Printer ErrorPrinter = new Printer(Console.OpenStandardError());
    }
}
// Printer.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 17:45

namespace CLown1331.IO
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public sealed class Printer : StreamWriter
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
// Reader.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 17:37

namespace CLown1331.IO
{
    using System.Collections.Generic;
    using System.IO;

    public static class Reader
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
}
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
// Solution.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 18:00

namespace CLown1331.Solution
{
    using CLown1331.IO;

    public class TaskSolution
    {
        public static int StackSize = 32 * (1 << 20);
        private const int Sz = (int)1e5 + 10;
        private const int Mod = (int)1e9 + 7;

        public void Solve()
        {
            int a, b;
            a = Input.NextInt();
            b = Input.NextInt();
            Output.OutputPrinter.WriteLine(a * b / 2);
        }
    }
}
// Helper.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 17:50

namespace CLown1331.Helper
{
    using System;
    using System.Collections.Generic;
    using CLown1331.IO;

    public static class Helper
    {
        private static readonly IEnumerator<uint> Xsi = Xsc();

        public static uint XorShift
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

    }
}
