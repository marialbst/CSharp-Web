namespace WebServer.Server.HTTP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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
            this.FormData = new Dictionary<string, string>();
            this.QueryParameters = new Dictionary<string, string>();
            this.UrlParameters = new Dictionary<string, string>();

            this.ParseRequest(requestString);
        }

        public Dictionary<string, string> FormData { get; private set; }

        public HttpHeaderCollection Headers { get; private set; }

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
            this.ParseParameters();

            if (this.Method == HttpRequestMethod.POST)
            {
                this.ParseQuery(requestLines[requestLines.Length - 1], this.FormData);
            }
        }
        
        private HttpRequestMethod ParseRequestMethod(string methodString)
        {
            try
            {
                return (HttpRequestMethod)Enum.Parse(typeof(HttpRequestMethod), methodString);
            }
            catch (Exception)
            {
                throw new BadRequestException("Invalid method");
            }
        }

        private void ParseHeaders(string[] requestLines)
        {
            int indexOfFirstEmptyLine = Array.IndexOf(requestLines, string.Empty);

            for (int i = 1; i < indexOfFirstEmptyLine; i++)
            {
                string currentLine = requestLines[0];

                string[] headerParameters = currentLine.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

                if (headerParameters.Length != 2)
                {
                    throw new BadRequestException("Invalid header");
                }

                string key = headerParameters[0];
                string value = headerParameters[1];

                HttpHeader header = new HttpHeader(key, value);
                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsKey("Host"))
            {
                throw new BadRequestException("Headers must contain Host");
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
    }
}
