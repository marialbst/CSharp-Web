namespace _03.SimpleWebServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            int port = 1333;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ip, port);

            tcpListener.Start();

            Console.WriteLine($"Server started.");
            Console.WriteLine($"Listening to TCP clients at {ip}:{port}");

            Task.Run(async () => { await Connect(tcpListener);})
                .GetAwaiter()
                .GetResult();
        }

        private static async Task Connect(TcpListener tcpListener)
        {
            while (true)
            {
                Console.WriteLine("Waiting for client...");
                using (TcpClient client = await tcpListener.AcceptTcpClientAsync())
                {
                    Console.WriteLine("Client connected");
                    byte[] buffer = new byte[1024];
                    await client.GetStream().ReadAsync(buffer, 0, buffer.Length);

                    string message = Encoding.UTF8.GetString(buffer);

                    Console.WriteLine(message.Trim('\0'));

                    //string responce = "Hello from server!";
                    string responce = "HTTP/1.1 200 OK\nContent-Type: text/plain\n\nHello from server!";
                    byte[] responceBuff = Encoding.UTF8.GetBytes(responce);
                    await client.GetStream().WriteAsync(responceBuff, 0, responceBuff.Length);
                }
                Console.WriteLine("Closing connection");
            }
        }
    }
}
