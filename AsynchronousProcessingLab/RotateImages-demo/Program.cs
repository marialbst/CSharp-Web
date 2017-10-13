namespace RotateImages_demo
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            string directory = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(directory + "/images");

            FileInfo[] files = directoryInfo.GetFiles();

            const string resultDir = "result";

            if (Directory.Exists(resultDir))
            {
                Directory.Delete(resultDir, true);
            }
            Directory.CreateDirectory(resultDir);

            var tasks = new List<Task>();

            foreach (var file in files)
            {
                Task task = Task.Run(()=> 
                {
                    Image image = Image.FromFile(file.FullName);
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    image.Save($"{resultDir}\\flipped-{file.Name}");

                    Console.WriteLine($"File {file.Name} processed...");
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("Finished!");
        }
    }
}
