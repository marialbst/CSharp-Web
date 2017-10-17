namespace WebServer.Server.HTTP
{
    using Common;

    public class HttpHeader
    {
        public const string Cookie = "Cookie";
        public const string ContentType = "Content-Type";
        public const string SetCookie = "Set-Cookie";

        public HttpHeader(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.Key = key;
            this.Value = value;
        }

        public string Key { get; private set; }

        public string Value { get; private set; }

        public override string ToString()
        {
            return $"{this.Key}: {this.Value}";
        }
    }
}
