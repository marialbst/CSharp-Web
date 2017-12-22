namespace SimpleMvc.Framework.Routers
{
    using WebServer.Contracts;
    using WebServer.Http.Contracts;

    public class ControllerRouter : IHandleable
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            return null;
        }
    }
}
