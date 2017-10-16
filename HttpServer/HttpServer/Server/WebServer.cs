namespace HttpServer.Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using Contracts;
    using Routing;
    using Routing.Contracts;
    using System.Threading.Tasks;

    public class WebServer : IRunnable
    {
        private const string localHost = "127.0.0.1";

        private readonly int port;

        private readonly IServerRouteConfig serverRouteConfig;

        private readonly TcpListener listener;

        private bool isRunning;

        public WebServer(int port, IAppRouteConfig appRouteConfig)
        {
            this.port = port;
            this.serverRouteConfig = new ServerRouteConfig(appRouteConfig);

            this.listener = new TcpListener(IPAddress.Parse(localHost),this.port);
        }

        public void Run()
        {
            this.listener.Start();

            this.isRunning = true;

            Console.WriteLine($"Server started. Listening to TCP clients at {localHost}:{port}");

            Task.Run(this.ListenLoop).Wait();
        }

        private async Task ListenLoop()
        {
            while (this.isRunning)
            {
                Socket client = await this.listener.AcceptSocketAsync();

                ConnectionHandler connectionHandler = new ConnectionHandler(client, this.serverRouteConfig);

                Task connection = connectionHandler.ProcessRequestAsync();
                connection.Wait();
            }
        }
    }
}
