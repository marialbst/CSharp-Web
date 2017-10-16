namespace HttpServer.Server.Handlers
{
    using System;
    using HTTP.Contracts;

    public class GetHandler : RequestHandler
    {
        public GetHandler(Func<IHttpContext, IHttpResponse> handlingFunc) 
            : base(handlingFunc)
        {
        }
    }
}
