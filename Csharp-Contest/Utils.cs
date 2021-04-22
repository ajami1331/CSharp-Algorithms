namespace CLown1331
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class Utils
    {
        private const string Import = "#import_";
        private const string Library = "Library";

        public static void CreateFileForSubmission()
        {
            string path = Directory.GetCurrentDirectory();
            path = Directory.GetParent(path)?.Parent.Parent.FullName;
            List<string> content = Process(path);
            string submissionFile = $"{path}/Submission.txt";
            using (StreamWriter w = File.CreateText(submissionFile))
            {
                foreach (string line in content)
                {
                    w.WriteLine(line);
                }
            }

            Console.WriteLine("Complete");
        }

        private static List<string> Process(string path)
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
                Console.WriteLine(u);
                foreach (string line in File.ReadAllLines(u))
                {
                    if (line.Contains(Import))
                    {
                        string import = line.Split(' ')
                                            .FirstOrDefault(s => s.StartsWith(Import))?
                                            .Remove(0, Import.Length);
                        string filePath = libraryFiles.FirstOrDefault(s => s.Contains($"\\{import}"));
                        if (!files.Contains(filePath))
                        {
                            queue.Enqueue(filePath);
                            files.Add(filePath);
                        }
                    }

                    content.Add(line);
                }
            }

            return content;
        }
    }
}
