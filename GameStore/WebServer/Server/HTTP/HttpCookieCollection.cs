namespace WebServer.Server.HTTP
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Common;
    using System.Collections;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly Dictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void AddCookie(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));

            this.cookies.Add(cookie.Key, cookie);
        }

        public void AddCookie(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.AddCookie(new HttpCookie(key,value));
        }

        public bool ContainsKey(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key,nameof(key));

            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            if (!this.cookies.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key with name {key} is not in the cookies collection!");
            }

            return this.cookies[key];
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            return this.cookies.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.cookies.Values.GetEnumerator();
        }
    }
}
