namespace WebServer.ByTheCake.Controllers
{
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Server.HTTP;
    using ByTheCake.ViewModels;

    public class UserController : Controller
    {
        public IHttpResponse Login()
        {
            this.ViewData["showError"] = "none";
            this.ViewData["authDisplay"] = "none";

            return this.FileViewResponse(@"User\login");
        }

        public IHttpResponse Login(IHttpRequest req)
        {
            const string nameField = "username";
            const string passwordField = "password";

            if (req.FormData.ContainsKey(nameField)
                && req.FormData.ContainsKey(passwordField)
                && !string.IsNullOrEmpty(req.FormData[nameField].Trim())
                && !string.IsNullOrEmpty(req.FormData[passwordField].Trim()))
            {

                string name = req.FormData[nameField].Trim();

                req.Session.Add(SessionStore.CurrentUserKey, name);
                req.Session.Add(Cart.SessionKey, new Cart());
            }
            else
            {
                this.ViewData["error"] = "You have empty fields";
                this.ViewData["showError"] = "block";

                return this.FileViewResponse(@"User\login");
            }

            return new RedirectResponse("/");
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            return new RedirectResponse("/login");
        }
    }
}
