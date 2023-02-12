// Output.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 17:50

namespace CLown1331.IO
{
    using System;
    using System.IO;

    public static class Output
    {
#if CLown1331
        public static Printer OutputPrinter = new Printer(
            new MultiStream(Console.OpenStandardOutput()));

        public static readonly Printer ErrorPrinter = new Printer(
            new MultiStream(
                File.Create(Path.Combine(Utils.GetRootDirectoryPath(), "error.txt")),
                Console.OpenStandardOutput()));
#else
        public static readonly Printer OutputPrinter = new Printer(Console.OpenStandardOutput());

        public static readonly Printer ErrorPrinter = new Printer(Console.OpenStandardError());
#endif
    }
}
