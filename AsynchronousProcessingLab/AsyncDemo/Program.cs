namespace AsyncDemo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        private static string result;

        static void Main(string[] args)
        {
            Console.WriteLine("Calculationg...");

            Task.Run(() =>  CalculateSlowly());

            Console.WriteLine("Enter command: ");

            while (true)
            {
                var line = Console.ReadLine();

                if (line == "exit")
                {
                    break;
                }

                if (line == "show")
                {
                    if (result == null)
                    {
                        Console.WriteLine("Still calculationg... Please wait");
                    }
                    else
                    {
                        Console.WriteLine($"Result is: {result}");
                    }
                }
            }
        }

        private static void CalculateSlowly()
        {
            Thread.Sleep(5000);
            result = "42";
        }
    }
}
