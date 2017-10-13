namespace _02.SliceFiles
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            //Console.Write("Enter directory: ");
            //string path = Console.ReadLine();
            string directory = Directory.GetCurrentDirectory();
            string path = directory + "\\file.jpg";
            //Console.Write("Enter destination: ");
            //string destination = Console.ReadLine();
            string destination = "result";
            Console.Write("Enter pieces: ");
            int pieces = int.Parse(Console.ReadLine());

            

            Task.Run(() =>
            {
                Slice(path, destination, pieces);
            });

            Console.WriteLine("Anything else?");

            while (true)
            {
                var line = Console.ReadLine();

                if (line.ToLower() == "no")
                {
                    break;
                }
            }
        }

        private static void Slice(string path, string destination, int pieces)
        {
            if (Directory.Exists(destination))
            {
                Directory.Delete(destination, true);
            }

            Directory.CreateDirectory(destination);

            using (FileStream source = new FileStream(path, FileMode.Open))
            {
                FileInfo fileInfo = new FileInfo(path);

                if (pieces == 0)
                {
                    pieces = 1;
                }

                long pieceLength = source.Length / pieces + 1;
                long currentByte = 0;
                Console.WriteLine("Processing file...");

                for (int i = 1; i <= pieces; i++)
                {
                    string finishedFilePath = $"{destination}/Part{i}-{fileInfo.Name}";

                    using (FileStream destinationStr = new FileStream(finishedFilePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[pieceLength];

                        while (currentByte < pieceLength * i)
                        {
                            int readBytes = source.Read(buffer, 0 , buffer.Length);

                            if (readBytes == 0)
                            {
                                break;
                            }

                            destinationStr.Write(buffer, 0, readBytes);
                            currentByte += readBytes;
                        }
                    }

                    Console.WriteLine($"Part {i}/{pieces} ready!");
                }

            }

            Console.WriteLine("All done!");
        }
    }
}
