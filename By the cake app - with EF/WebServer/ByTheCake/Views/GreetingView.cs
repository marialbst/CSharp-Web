namespace WebServer.ByTheCake.Views
{
    using Server.Contracts;
    using Server.HTTP.Contracts;

    public class GreetingView : IView
    {
        private IHttpSession session;

        public GreetingView(IHttpSession session)
        {
            this.session = session;
        }

        public string View()
        {
            string result = string.Empty;

            if (this.session.Get("firstName") == null)
            {
                result = "<h2>Please fulfill the required items</h2>"
                    +"<form method=\"post\">"
                    +"<label>First name: <input type=\"text\" name=\"firstName\"/></label>"
                    + "<input type=\"submit\" value=\"Next>>\"/>"
                    + "</form>";
            }
            else if (this.session.Get("lastName") == null)
            {
                result = "<h2>Please fulfill the required items</h2>"
                    + "<form method=\"post\">"
                    + "<label>Last name: <input type=\"text\" name=\"lastName\"/></label>"
                    + "<input type=\"submit\" value=\"Next>>\"/>"
                    + "</form>";
            }
            else if (this.session.Get("age") == null)
            {
                result = "<h2>Please fulfill the required items</h2>"
                    + "<form method=\"post\">"
                    + "<label>Age: <input type=\"text\" name=\"age\"/></label>"
                    + "<input type=\"submit\" value=\"Greet Me\"/>"
                    + "</form>";
            }
            else
            {
                result = $"<h2>Hello {this.session.Get("firstName")} {this.session.Get("lastName")} at age {this.session.Get("age")}!</h2>";
            }
            return result;
        }
    }
}
