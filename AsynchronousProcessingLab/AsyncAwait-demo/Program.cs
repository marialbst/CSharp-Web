namespace AsyncAwait_demo
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            DoWork();

            //Task.Run(async() => await DoWork()).GetAwaiter().GetResult();
        }

        //public static async Task DoWork()
        public static async void DoWork()
        {
            var tasks = new List<Task>();
            var results = new List<bool>();

            for (int i = 0; i < 10; i++)
            {
                var task = Task.Run(async () =>
                {
                    var result = await SlowMethod();
                    results.Add(result);
                });
                tasks.Add(task);
            }

            //Task.WaitAll(tasks.ToArray());

            await Task.WhenAll(tasks.ToArray());

            Console.WriteLine("Finished!");
        }

        public static async Task<bool> SlowMethod()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Result!");
            return true;
        }
    }
}
//httpclient
//webclient
//main 