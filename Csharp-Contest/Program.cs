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
#if CLown1331
            Validator.Process();
#else
            var solution = new TaskSolution();
            Thread t = new Thread(solution.Solve, TaskSolution.StackSize);
            t.Start();
            t.Join();
            Output.OutputPrinter.Flush();
            Output.ErrorPrinter.Flush();
#endif
        }
    }
}
