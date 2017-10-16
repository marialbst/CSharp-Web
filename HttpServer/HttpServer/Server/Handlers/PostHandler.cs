namespace HttpServer.Server.Handlers
{
    using System;
    using HTTP.Contracts;

    public class PostHandler : RequestHandler
    {
        public PostHandler(Func<IHttpContext, IHttpResponse> handlingFunc) 
            : base(handlingFunc)
        {
        }
    }
}
