﻿namespace WebServer.ByTheCake.Views
{
    using Server.Contracts;

    public class NotFoundView : IView
    {
        public string View()
        {
            return "<body><center><h1>404 Not Found :(</h1></br><a href=\"/\">Home</a></center></body>";
        }
    }
}
