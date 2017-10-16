namespace HttpServer
{
    using HttpServer.Application;
    using Server;
    using Server.Contracts;
    using Server.Routing;
    using Server.Routing.Contracts;

    class Launcher : IRunnable
    {
        private const int port = 8283;

        private WebServer webServer;
        
        static void Main()
        {
            new Launcher().Run();
        }

        public void Run()
        {
            IApplication app = new MainApplication();
            IAppRouteConfig appRouteConfig = new AppRouteConfig();
            app.Start(appRouteConfig);

            this.webServer = new WebServer(port, appRouteConfig);
            this.webServer.Run();
        }
    }
}
