using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace PowerClone
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = @"";
            var target = @"";
            PowerClone(new DirectoryInfo(source), new DirectoryInfo(target));
            CheckClone(new DirectoryInfo(source));
        }
        /// <summary>
        /// Clone information contained by source to target. That means, create same dir-structure, move existed files, and clone filename of cloud files
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void PowerClone(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (var subDir in source.GetDirectories())
            {
                //var targetSubDir=target
                PowerClone(subDir, target.CreateSubdirectory(subDir.Name));
                Console.WriteLine($"Clone-FolderInfo: {subDir.FullName}");
            }

            foreach (var file in source.GetFiles())
            {
                if (file.GetRealSize() < file.Length)
                {
                    var tarPath = Path.Combine(target.FullName, file.Name);
                    if (File.Exists(tarPath)) continue;
                    File.Create(tarPath).Close();
                    Console.WriteLine($"Clone-FileInfo: {file.FullName}");
                }
                else
                {
                    try
                    {
                        file.MoveTo(Path.Combine(target.FullName, file.Name), true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        
                    }

                    Console.WriteLine($"Move-File/Clone-FileInfo: {file.FullName}");
                }
            }
        }
        /// <summary>
        /// Check if all files in source dir now are cloud ones.
        /// </summary>
        /// <param name="source"></param>
        public static void CheckClone(DirectoryInfo source)
        {
            foreach (var subDir in source.GetDirectories())
            {
                CheckClone(subDir);
            }

            foreach (var file in source.GetFiles())
            {
                if (file.GetRealSize() < file.Length)
                {

                    Console.WriteLine($"Size: {file.GetRealSize()} - FileInfo: {file.FullName}");
                }
                else
                {

                    Console.WriteLine($"FailedFile: {file.FullName}");
                }
            }
        }
    }
}
