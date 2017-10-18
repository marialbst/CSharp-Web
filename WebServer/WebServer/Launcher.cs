﻿namespace WebServer
{
    //using Application;
    using ByTheCakeApp;
    using Server;
    using Server.Contracts;
    using Server.Routing;
    using Server.Routing.Contracts;

    class Launcher : IRunnable
    {
        private WebServer webServer;

        static void Main()
        {
            new Launcher().Run();
        }

        public void Run()
        {
            //IApplication app = new MainApplication();
            IApplication app = new ByTheCakeMainApp();
            IAppRouteConfig appRouteConfig = new AppRouteConfig();
            app.Start(appRouteConfig);

            this.webServer = new WebServer(1333, appRouteConfig);
            this.webServer.Run();
        }
    }
}
