namespace HttpServer.Application.Views
{
    using Server;
    using Server.Contracts;

    public class UserDetailsView : IView
    {
        private readonly Model model;

        public UserDetailsView(Model model)
        {
            this.model = model;
        }

        public string View()
        {
            return $"<body><h1>Hello, {model["name"]}!<h1></body>";
        }
    }
}
