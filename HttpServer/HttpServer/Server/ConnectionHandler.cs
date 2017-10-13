namespace HttpServer.Server
{
    using System.Net.Sockets;
    using Routing.Contracts;
    using Common;
    using System.Threading.Tasks;
    using System;
    using HTTP;
    using HTTP.Contracts;
    using Handlers;
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
            string requestStr = await this.ReadRequest();

            IHttpRequest request = new HttpRequest(requestStr);

            IHttpContext httpContext = new HttpContext(request);

            IHttpResponse httpResponse = new HttpHandler(this.serverRouteConfig).Handle(httpContext);

            byte[] responseBytes = Encoding.UTF8.GetBytes(httpResponse.ToString());

            ArraySegment<byte> byteSegments = new ArraySegment<byte>(responseBytes);

            await this.client.SendAsync(byteSegments, SocketFlags.None);

            Console.WriteLine("###Request:");
            Console.WriteLine(request);
            Console.WriteLine("###Response: ");
            Console.WriteLine(httpResponse);

            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<string> ReadRequest()
        {
            StringBuilder result = new StringBuilder();

            byte[] buffer = new byte[1024];

            ArraySegment<byte> data = new ArraySegment<byte>(buffer);

            while (true)
            {
                int readBytes = await this.client.ReceiveAsync(data, SocketFlags.None);

                if (readBytes <= 0)
                {
                    break;
                }

                string bytesAsStr = Encoding.UTF8.GetString(data.Array, 0, readBytes);

                result.AppendLine(bytesAsStr);

                if (readBytes < 1024)
                {
                    break;
                }
            }

            return result.ToString();
        }
    }
}
