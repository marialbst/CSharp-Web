namespace HttpServer.Server
{
    using System.Net;
    using System.Net.Sockets;
    using Contracts;
    using Routing;
    using Routing.Contracts;

    public class WebServer : IRunnable
    {
        private readonly int port;

        private readonly IServerRouteConfig serverRouteConfig;

        private readonly TcpListener listener;

        private bool isRunning;

        public WebServer(int port, IAppRouteConfig appRouteConfig)
        {
            this.port = port;
            this.serverRouteConfig = new ServerRouteConfig(appRouteConfig);

            this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"),this.port);
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }
    }
}
