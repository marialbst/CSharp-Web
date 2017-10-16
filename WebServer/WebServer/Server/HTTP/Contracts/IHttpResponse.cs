namespace WebServer.Server.HTTP.Contracts
{
    using Enums;

    public interface IHttpResponse
    {
        IHttpHeaderCollection Headers { get; }

        HttpStatusCode StatusCode { get; }

        string StatusMessage { get; }

        string Response { get; }

        void AddHeader(string key, string value);
    }
}
