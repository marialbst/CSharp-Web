namespace WebServer.Application.Controllers
{
    using Views;
    using System;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;

    public class HomeController
    {
        // GET /
        public IHttpResponse Index()
        {
            IHttpResponse response = new ViewResponse(HttpStatusCode.Ok, new HomeIndexView());

            response.Cookies.AddCookie("lang", "en");

            return response;
        }

        //GET /testsession
        public IHttpResponse SessionTest(IHttpRequest req)
        {
            IHttpSession session = req.Session;

            const string sessionDateKey = "saved_date";

            if (session.Get(sessionDateKey) == null)
            {
                session.Add(sessionDateKey, DateTime.UtcNow);
            }

            return new ViewResponse(HttpStatusCode.Ok, new SessionTestView((DateTime)session.Get(sessionDateKey)));
        }
    }
}
