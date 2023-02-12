// Validator.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 19:02

namespace CLown1331
{
    using System;
    using System.IO;
    using System.Threading;
    using CLown1331.IO;
    using CLown1331.Solution;

    public static class Validator
    {
        public static void Process()
        {
            if (!TaskSolution.ValidateTestCases)
            {
                var outputStream = new MultiStream(
                    File.Create(Path.Combine(Utils.GetRootDirectoryPath(), "output.txt")),
                    Console.OpenStandardOutput());
                Output.OutputPrinter = new Printer(outputStream);
                for (var testCase = 1; !Reader.IsEndOfStream; testCase++)
                {
                    Output.ErrorPrinter.WriteLine("---- Case {0} ----", testCase);
                    new TaskSolution().Solve();
                    Output.ErrorPrinter.WriteLine("---- Case {0} ----", testCase);
                }
            }
            else
            {
                var testCasesCntFile = new StreamReader("./test_cases/cnt");
                int testCases = int.Parse(testCasesCntFile.ReadLine() ?? string.Empty);
                var stopWatch = new System.Diagnostics.Stopwatch();
                long totalTime = stopWatch.ElapsedMilliseconds;
                stopWatch.Start();
                var passedCount = 0;
                string outputFilePath = Path.Combine(Utils.GetRootDirectoryPath(), "output.txt");
                File.WriteAllText(outputFilePath, string.Empty);
                for (var testCase = 1; testCase <= testCases; testCase++)
                {
                    string inputFileNameForTestCase = "./test_cases/" + testCase + ".in";
                    string outputFileNameForTestCase = "./test_cases/" + testCase + ".out";
                    string validFileNameForTestCase = "./test_cases/" + testCase + ".val";
                    Reader.SetInputReader(new StreamReader(inputFileNameForTestCase));
                    FileStream outputFileForTestCase = File.Create(outputFileNameForTestCase);
                    var outputStream = new MultiStream(outputFileForTestCase, Console.OpenStandardOutput());
                    Output.OutputPrinter = new Printer(outputStream);
                    stopWatch.Restart();
                    new TaskSolution().Solve();
                    outputFileForTestCase.Close();
                    File.WriteAllText(outputFilePath, File.ReadAllText(outputFilePath) + File.ReadAllText(outputFileNameForTestCase));
                    bool success = File.ReadAllText(outputFileNameForTestCase) == File.ReadAllText(validFileNameForTestCase);
                    if (success)
                    {
                        passedCount++;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Output.ErrorPrinter.WriteLine("Test {0}: Success!", testCase);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Output.ErrorPrinter.WriteLine("Test {0}: Fail!", testCase);
                    }

                    Console.ForegroundColor = ConsoleColor.Black;
                    stopWatch.Stop();
                    totalTime += stopWatch.ElapsedMilliseconds;
                }

                Console.ForegroundColor = passedCount == testCases ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;

                Output.ErrorPrinter.WriteLine("{0}/{1} Tests passed!", passedCount, testCases);
                Console.ForegroundColor = ConsoleColor.Black;

                Output.ErrorPrinter.WriteLine($"Runtime: {totalTime}ms");
                stopWatch.Stop();
            }

            Utils.CreateFileForSubmission(Output.ErrorPrinter);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
