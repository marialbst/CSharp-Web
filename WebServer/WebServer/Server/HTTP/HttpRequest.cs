namespace WebServer.Server.HTTP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Enums;
    using Exceptions;
    using Common;
    using Contracts;
    using System.Net;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            this.FormData = new Dictionary<string, string>();
            this.QueryParameters = new Dictionary<string, string>();
            this.UrlParameters = new Dictionary<string, string>();
            
            this.ParseRequest(requestString);
        }

        public Dictionary<string, string> FormData { get; private set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; private set; }

        public IHttpSession Session { get; set; }

        public string Path { get; private set; }

        public Dictionary<string, string> QueryParameters { get; private set; }

        public HttpRequestMethod Method { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, string> UrlParameters { get; private set; }

        public void AddUrlParameter(string key, string value)
        {
            this.UrlParameters[key] = value;
        }

        private void ParseRequest(string requestString)
        {
            string[] requestLines = requestString
                .Split(Environment.NewLine);

            if (!requestLines.Any())
            {
                throw new BadRequestException("Invalid request!");
            }


            string[] requestLine = requestLines[0]
                .Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("Invalid request!");
            }

            this.Method = this.ParseRequestMethod(requestLine[0].ToUpper());
            this.Url = requestLine[1];
            this.Path = this.Url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];

            this.ParseHeaders(requestLines);
            this.ParseCookies();
            this.ParseParameters();
            this.ParseFormData(requestLines[requestLines.Length - 1]);
            this.SetSession();
            
        }

        private HttpRequestMethod ParseRequestMethod(string methodString)
        {
            try
            {
                return (HttpRequestMethod)Enum.Parse(typeof(HttpRequestMethod), methodString);
            }
            catch (Exception)
            {
                throw new BadRequestException("Invalid method in request line");
            }
        }

        private void ParseHeaders(string[] requestLines)
        {
            int indexOfFirstEmptyLine = Array.IndexOf(requestLines, string.Empty);

            for (int i = 1; i < indexOfFirstEmptyLine; i++)
            {
                string currentLine = requestLines[i];

                string[] headerParameters = currentLine.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

                if (headerParameters.Length != 2)
                {
                    throw new BadRequestException("Invalid header");
                }

                string key = headerParameters[0].Trim();
                string value = headerParameters[1].Trim();

                HttpHeader header = new HttpHeader(key, value);
                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsKey("Host"))
            {
                throw new BadRequestException("Headers must contain Host");
            }
        }

        private void ParseCookies()
        {
            if (this.Headers.ContainsKey(HttpHeader.Cookie))
            {
                var allCookies = this.Headers.GetHeader(HttpHeader.Cookie);

                foreach (var cookie in allCookies)
                {
                    if (!cookie.Value.Contains('='))
                    {
                        return;
                    }

                    string[] cookieParts = cookie
                        .Value
                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    if (!cookieParts.Any())
                    {
                        continue;
                    }

                    foreach (var cookiePart in cookieParts)
                    {
                        string[] cookieKvp = cookiePart.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                        if (cookieKvp.Length == 2)
                        {
                            string cookieKey = cookieKvp[0].Trim();
                            string cookieValue = cookieKvp[1].Trim();
                            HttpCookie parsedCookie = new HttpCookie(cookieKey, cookieValue, false);
                            this.Cookies.AddCookie(parsedCookie);
                        }
                    }
                }
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            string query = this.Url.Split(new[] {'?'}, StringSplitOptions.RemoveEmptyEntries)[1];

            this.ParseQuery(query, this.QueryParameters);
         }

        private void ParseQuery(string query, Dictionary<string, string> dict)
        {
            if (!query.Contains('='))
            {
                return;
            }

            string[] queryPairs = query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var kvp in queryPairs)
            {
                string[] queryPair = kvp.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (queryPair.Length != 2)
                {
                    return;
                }

                string queryKey = WebUtility.UrlDecode(queryPair[0]);
                string queryValue = WebUtility.UrlDecode(queryPair[1]);

                dict.Add(queryKey, queryValue);
            }
        }

        private void ParseFormData(string requestLine)
        {
            if (this.Method == HttpRequestMethod.POST)
            {
                this.ParseQuery(requestLine, this.FormData);
            }
        }

        private void SetSession()
        {
            string sessionCookieKey = SessionStore.SessionCookieKey;

            if (this.Cookies.ContainsKey(sessionCookieKey))
            {
                HttpCookie cookie = this.Cookies.GetCookie(sessionCookieKey);
                string sessionId = cookie.Value;

                this.Session = SessionStore.Get(sessionId);
            }
        }
    }
}
