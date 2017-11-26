namespace WebServer.Server.HTTP
{
    using Contracts;
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, ICollection<HttpHeader>> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, ICollection<HttpHeader>>();
        }

        public void Add(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));

            if (!this.headers.ContainsKey(header.Key))
            {
                this.headers[header.Key] = new List<HttpHeader>();
            }
            this.headers[header.Key].Add(header);
        }

        public void Add(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            if (!this.headers.ContainsKey(key))
            {
                this.headers[key] = new List<HttpHeader>();
            }
            this.headers[key].Add(new HttpHeader(key, value));
        }

        public bool ContainsKey(string key)
        {
            return this.headers.ContainsKey(key);
        }

        public ICollection<HttpHeader> GetHeader(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            if (!this.headers.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key with name {key} is not in the header collection!");
            }

            return this.headers[key];
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            foreach (var header in this.headers)
            {
                var headerKey = header.Key;

                foreach (var headerValue in header.Value)
                {
                    result.AppendLine($"{headerKey}: {headerValue.Value}");
                }
            }

            return result.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.headers.Values.GetEnumerator();
        }
        
        IEnumerator<ICollection<HttpHeader>> IEnumerable<ICollection<HttpHeader>>.GetEnumerator()
        {
            return this.headers.Values.GetEnumerator();
        }
    }
}
