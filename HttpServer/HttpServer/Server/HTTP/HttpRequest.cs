namespace HttpServer.Server.HTTP
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Enums;
    using Common;
    using Exceptions;
    using System.Linq;
    using System.Net;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, string>();
            this.Headers = new HttpHeaderCollection();
            this.QueryParameters = new Dictionary<string, string>();
            this.UrlParameters = new Dictionary<string, string>();

            this.ParseRequest(requestString);
            
        }

        public IDictionary<string, string> FormData { get; private set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public string Path { get; private set; }

        public IDictionary<string, string> QueryParameters { get; private set; }

        public HttpRequestMethod Method { get; private set; }

        public string Url { get; private set; }

        public IDictionary<string, string> UrlParameters { get; private set; }

        public void AddUrlParameter(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.UrlParameters[key] = value;
        }

        private void ParseRequest(string requestString)
        {
            string[] requestLines = requestString.Split(Environment.NewLine);

            if (!requestLines.Any())
            {
                throw new BadRequestException("Invalid request!");
            }

            string[] requestLine = requestLines[0].Split(
                new[] { ' ' }, 
                StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("Invalid request line!");
            }

            this.Method = this.ParseMethod(requestLine[0]);
            this.Url = requestLine[1];
            this.Path = this.ParsePath(this.Url);

            this.ParseHeaders(requestLines);
            this.ParseParameters();
            this.ParseFormData(requestLines.Last());
        }

        private HttpRequestMethod ParseMethod(string method)
        {
            try
            {
                return Enum.Parse<HttpRequestMethod>(method, true);
            }
            catch (Exception)
            {

                throw new BadRequestException("Invalid method!");
            }
        }

        private string ParsePath(string url)
        {
            return url.Split(new [] { '?','#'},StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private void ParseHeaders(string[] requestLines)
        {
            int emptyLineAfterHeaders = Array.IndexOf(requestLines, string.Empty);

            for (int i = 1; i < emptyLineAfterHeaders; i++)
            {
                string currentLine = requestLines[i];
                string[] headerParts = currentLine.Split(new[] { ": "}, StringSplitOptions.RemoveEmptyEntries);

                if (headerParts.Length != 2)
                {
                    throw new BadRequestException("Invalid header");
                }

                string headerKey = headerParts[0];
                string headerValue = headerParts[1].Trim();

                this.Headers.Add(new HttpHeader(headerKey, headerValue));
            }

            if (!this.Headers.ContainsKey("Host"))
            {
                throw new BadRequestException("Invalid header. Must contain header Host");
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            string query = this.Url
                .Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Last();

            this.ParseQuery(query, this.UrlParameters);
        }

        private void ParseFormData(string formDataLine)
        {
            if (this.Method == HttpRequestMethod.Get)
            {
                return;
            }
            //thtia.QueryParameters
            this.ParseQuery(formDataLine, this.FormData);
        }

        private void ParseQuery(string query, IDictionary<string,string> dict)
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
