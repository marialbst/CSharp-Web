namespace WebServer.Application.Views
{
    using System;
    using Server.Contracts;

    public class SessionTestView : IView
    {
        private readonly DateTime date;

        public SessionTestView(DateTime date)
        {
            this.date = date;
        }

        public string View()
        {
            return $"<h1>Saved date is: {date}</h1>";
        }
    }
}
