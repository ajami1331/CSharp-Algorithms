// Utils.cs
// Author: Araf Al Jami
// Last Updated: 21-08-2565 01:43

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
        private const string NamespacePrefix = "Library.";
        private const string CommentPrefix = "//";

        public static void CreateFileForSubmission(StreamWriter writer)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            string path = Directory.GetCurrentDirectory();
            path = Directory.GetParent(path)?.Parent.Parent.FullName;
            List<string> content = ProcessUsing(path, writer);
            string submissionFile = Path.Combine(path, "Submission.cs");
            File.WriteAllLines(submissionFile, content);
            stopWatch.Stop();
            writer.WriteLine($"File created for submission: {stopWatch.ElapsedMilliseconds}ms");
        }

        private static List<string> ProcessUsing(string path, StreamWriter writer)
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
                foreach (string line in File.ReadAllLines(u))
                {
                    content.Add(line);

                    if (!line.Contains(Using))
                    {
                        continue;
                    }

                    string import = line.Split(' ')
                        .SingleOrDefault(s => s.StartsWith(NamespacePrefix) && s.EndsWith(SemiColon))?
                        .Split(SemiColon)
                        .FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(import))
                    {
                        continue;
                    }

                    string filePath = Path.Combine(path, Path.Combine(import.Split("."))) + ".cs";
                    if (string.IsNullOrWhiteSpace(filePath) || files.Contains(filePath))
                    {
                        continue;
                    }

                    queue.Enqueue(filePath);
                    files.Add(filePath);
                }
            }

            return content;
        }
    }
}
