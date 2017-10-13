namespace _01.EvenNumbersThread
{
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            int firstNum = int.Parse(Console.ReadLine());
            int secondNum = int.Parse(Console.ReadLine());

            Thread thread = new Thread(() => PrintEvenNumbersInRange(firstNum, secondNum));
            thread.Start();
            thread.Join();

            Console.WriteLine("Thread finished!");
        }

        private static void PrintEvenNumbersInRange(int firstNum, int secondNum)
        {
            int max = Math.Max(firstNum, secondNum);
            int min = Math.Min(firstNum, secondNum);
            for (int i = min; i <= max; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}
