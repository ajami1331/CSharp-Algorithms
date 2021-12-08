// Utils.cs
// Authors: Araf Al-Jami
// Created: 23-08-2020 1:44 PM
// Updated: 08-07-2021 3:44 PM

using System.Diagnostics;

namespace CLown1331
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Utils
    {
        private const string Import = "#import_";
        private const string Using = "using";
        private const string SemiColon = ";";
        private const string NamespacePrefix = "Library.";
        private const string Library = "Library";

        public static void CreateFileForSubmission(StreamWriter writer)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            string path = Directory.GetCurrentDirectory();
            path = Directory.GetParent(path)?.Parent.Parent.FullName;
            List<string> content = ProcessUsing(path, writer);
            string submissionFile = $"{path}/Submission.cs";
            using (StreamWriter w = File.CreateText(submissionFile))
            {
                foreach (string line in content)
                {
                    w.WriteLine(line);
                }
            }

            stopWatch.Stop();
            writer.WriteLine($"File created for submission: {stopWatch.ElapsedMilliseconds}ms");
        }

        private static List<string> ProcessUsing(string path, StreamWriter writer)
        {
            string[] libraryFiles = Directory.GetFiles($"{path}/{Library}");
            List<string> content = new List<string>();
            HashSet<string> files = new HashSet<string>();
            Queue<string> queue = new Queue<string>();
            queue.Enqueue($"{path}/program.cs");
            files.Add($"{path}/program.cs");
            while (queue.Count > 0)
            {
                string u = queue.Dequeue();
                writer.WriteLine(u);
                foreach (string line in File.ReadAllLines(u))
                {
                    if (line.Contains(Using))
                    {
                        string import = line.Split(' ')
                            .SingleOrDefault(s => s.StartsWith(NamespacePrefix) && s.EndsWith(SemiColon))?
                            .Remove(0, NamespacePrefix.Length)
                            .Split(SemiColon)
                            .FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(import))
                        {
                            string searchString = $"\\{import}";
                            string filePath = libraryFiles.FirstOrDefault(s => s.Contains(searchString));
                            if (!string.IsNullOrWhiteSpace(filePath) && !files.Contains(filePath))
                            {
                                queue.Enqueue(filePath);
                                files.Add(filePath);
                            }
                        }
                    }

                    content.Add(line);
                }
            }

            return content;
        }

        private static List<string> ProcessImport(string path, StreamWriter writer)
        {
            string[] libraryFiles = Directory.GetFiles($"{path}/{Library}");
            List<string> content = new List<string>();
            HashSet<string> files = new HashSet<string>();
            Queue<string> queue = new Queue<string>();
            queue.Enqueue($"{path}/program.cs");
            files.Add($"{path}/program.cs");
            while (queue.Count > 0)
            {
                string u = queue.Dequeue();
                writer.WriteLine(u);
                foreach (string line in File.ReadAllLines(u))
                {
                    if (line.Contains(Import))
                    {
                        string import = line.Split(' ')
                                            .FirstOrDefault(s => s.StartsWith(Import))?
                                            .Remove(0, Import.Length);
                        if (!string.IsNullOrWhiteSpace(import))
                        {
                            string searchString = $"\\{import}";
                            string filePath = libraryFiles.FirstOrDefault(s => s.Contains(searchString));
                            if (!string.IsNullOrWhiteSpace(filePath) && !files.Contains(filePath))
                            {
                                queue.Enqueue(filePath);
                                files.Add(filePath);
                            }
                        }
                    }

                    content.Add(line);
                }
            }

            return content;
        }
    }
}
