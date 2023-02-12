// Solution.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 18:00

namespace CLown1331.Solution
{
    using CLown1331.IO;

    public class TaskSolution
    {
#if CLown1331
        public static bool ValidateTestCases = true;
#endif
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
