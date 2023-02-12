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
#if CLown1331
        private static StreamReader InputReader = new StreamReader("input.txt");
#else
        private static readonly StreamReader InputReader = new StreamReader(Console.OpenStandardInput());
#endif
        public static bool IsEndOfStream
        {
            get { return InputReader.EndOfStream; }
        }

#if CLown1331
        public static void SetInputReader(StreamReader streamReader)
        {
            InputReader = streamReader;
            Param.Clear();
        }
#endif

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
