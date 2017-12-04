namespace WebServer.Server.HTTP
{
    using System.Collections.Concurrent;

    public static class SessionStore
    {
        public const string SessionCookieKey = "SessionId";
        public const string CurrentUserKey = "CurrentUser";
        public const string ShoppingCartKey = "ShoppingCart";

        private static readonly ConcurrentDictionary<string, HttpSession> sessions = new ConcurrentDictionary<string, HttpSession>();

        public static HttpSession Get(string id)
        {
            return sessions.GetOrAdd(id, f => new HttpSession(id));
        }
    }
}
