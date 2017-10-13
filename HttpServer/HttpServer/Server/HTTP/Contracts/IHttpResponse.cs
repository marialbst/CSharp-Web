namespace HttpServer.Server.HTTP.Contracts
{
    using Enums;

    public interface IHttpResponse
    {
        HttpHeaderCollection Headers { get; }

        HttpResponseStatusCode StatusCode { get; }
    }
}
