namespace WebServer.Server.HTTP.Contracts
{
    using System.Collections.Generic;
    using Enums;

    public interface IHttpRequest
    {
        Dictionary<string, string> FormData { get; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        IHttpSession Session { get; set; }

        string Path { get; }

        Dictionary<string, string> QueryParameters { get; }

        HttpRequestMethod Method { get; }

        string Url { get; }

        Dictionary<string, string> UrlParameters { get; }

        void AddUrlParameter(string key, string value);
    }
}
