namespace RaceCondition_demo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = Enumerable.Range(0, 10000).ToList();

            for (int i = 0; i < 4; i++)
            {

                var thread = new Thread(() =>
                {
                    while (numbers.Count > 0)
                    {
                        lock (numbers)
                        {
                            if (numbers.Count == 0)
                            {
                                break;
                            }

                            numbers.RemoveAt(numbers.Count - 1);
                        }
                    }
                });

                thread.Start();
            }
        }
    }
}
