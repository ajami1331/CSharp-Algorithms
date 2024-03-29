﻿// Utils.cs
// Author: Araf Al Jami
// Last Updated: 10-09-2565 20:57

namespace CLown1331
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    public static class Utils
    {
        private const string Using = "using";
        private const string SemiColon = ";";
        private static readonly string RootNamespace = typeof(Program).Namespace + ".";
        private const string IfCLown1331 = "#if CLown1331";
        private const string Else = "#else";
        private const string EndIf = "#endif";

        public static string GetRootDirectoryPath()
        {
            string path = Directory.GetCurrentDirectory();
            return Directory.GetParent(path)?.Parent.Parent.FullName;
        }

        public static void CreateFileForSubmission(StreamWriter writer)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            string path = GetRootDirectoryPath();
            IEnumerable<string> content = ProcessUsing(path, writer);
            string submissionFile = Path.Combine(path, "Submission.cs");
            File.WriteAllLines(submissionFile, content);
            stopWatch.Stop();
            writer.WriteLine($"File created for submission: {stopWatch.ElapsedMilliseconds}ms");
        }

        private static IEnumerable<string> ProcessUsing(string path, TextWriter writer)
        {
            var content = new List<string>();
            var files = new HashSet<string>();
            var queue = new Queue<string>();
            string startingFile = Path.Combine(path, "Program.cs");
            queue.Enqueue(startingFile);
            files.Add(startingFile);
            while (queue.Count > 0)
            {
                string u = queue.Dequeue();
                writer.WriteLine(u);
                var skip = false;
                var clown1331Start = false;
                foreach (string line in File.ReadAllLines(u))
                {
                    if (line.Trim().StartsWith(IfCLown1331))
                    {
                        skip = true;
                        clown1331Start = true;
                    }

                    if (clown1331Start && (line.Trim().StartsWith(EndIf) || line.Trim().StartsWith(Else)))
                    {
                        skip = false;
                        clown1331Start = !line.Trim().StartsWith(EndIf);
                        continue;
                    }

                    if (skip)
                    {
                        continue;
                    }

                    content.Add(line);

                    if (!line.Contains(Using))
                    {
                        continue;
                    }

                    string import = line.Split(' ')
                        .SingleOrDefault(s => s.StartsWith(RootNamespace) && s.EndsWith(SemiColon))?
                        .Remove(0, RootNamespace.Length)
                        .Split(SemiColon[0])
                        .FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(import))
                    {
                        continue;
                    }

                    string dirPath = Path.Combine(path, Path.Combine(import.Split('.')));
                    foreach (FileInfo fileInfo in new DirectoryInfo(dirPath).GetFiles())
                    {
                        string filePath = fileInfo.FullName;
                        if (string.IsNullOrWhiteSpace(filePath) || files.Contains(filePath))
                        {
                            continue;
                        }

                        queue.Enqueue(filePath);
                        files.Add(filePath);
                    }
                }
            }

            return content;
        }
    }
}
