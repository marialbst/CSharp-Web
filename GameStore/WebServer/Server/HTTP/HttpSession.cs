namespace WebServer.Server.HTTP
{
    using Common;
    using Contracts;
    using System;
    using System.Collections.Generic;

    public class HttpSession : IHttpSession
    {
        private IDictionary<string, object> values;

        public HttpSession(string id)
        {
            CoreValidator.ThrowIfNullOrEmpty(id, nameof(id));

            this.values = new Dictionary<string, object>();
            this.Id = id;
        }

        public string Id { get; private set; }

        public object Get(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            if (!this.values.ContainsKey(key))
            {
                return null;
            }

            return this.values[key];
        }

        public void Add(string key, object value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNull(value, nameof(value));
            
            this.values[key] = value;
        }

        public void Clear()
        {
            this.values.Clear();
        }

        public void Clear(string key)
        {
            this.values.Remove(key);
        }

        public bool IsAuthenticated()
        {
            return this.values.ContainsKey(SessionStore.CurrentUserKey);
        }

        public bool Contains(string key)
        {
            return this.values.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            return (T)this.Get(key);
        }
    }
}
