namespace WebServer.Server
{
    using Common;
    using HTTP;
    using HTTP.Contracts;
    using Handlers;
    using System;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using Routing.Contracts;
    using System.Text;

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IServerRouteConfig serverRouteConfig;

        public ConnectionHandler(Socket client, IServerRouteConfig serverRouteConfig)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            this.client = client;
            this.serverRouteConfig = serverRouteConfig;
        }

        public async Task ProcessRequestAsync()
        {
            string request = await this.ReadRequest();

            if (!string.IsNullOrEmpty(request))
            {
                IHttpContext httpContext = new HttpContext(request);

                IHttpResponse response = new HttpHandler(this.serverRouteConfig).Handle(httpContext);

                var data = Encoding.UTF8.GetBytes(response.Response);

                ArraySegment<byte> toBytes = new ArraySegment<byte>(data);

                await this.client.SendAsync(toBytes, SocketFlags.None);

                Console.WriteLine("-------------------Request:");
                Console.WriteLine(request.ToString());
                Console.WriteLine("-------------------Response:");
                Console.WriteLine(response.Response);
            }

            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<string> ReadRequest()
        {
            string request = string.Empty;

            ArraySegment<byte> data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int readBytes = await this.client.ReceiveAsync(data, SocketFlags.None);

                if (readBytes <= 0)
                {
                    break;
                }

                request += Encoding.UTF8.GetString(data.Array, 0, readBytes);

                if (readBytes < 1024)
                {
                    break;
                }
            }

            return request;
        }
    }
}
