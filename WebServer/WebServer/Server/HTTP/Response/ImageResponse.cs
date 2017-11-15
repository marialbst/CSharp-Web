namespace WebServer.Server.HTTP.Response
{
    using Server.HTTP.Contracts;
    using Server.Enums;
    using System.Text;
    using System.IO;

    public class ImageResponse : IHttpResponse
    {
        private string imagePath;

        public ImageResponse(string imagepath)
        {
            this.imagePath = imagepath;
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
        }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public string StatusMessage => this.StatusCode.ToString();

        public string Response
        {
            get
            {
                StringBuilder response = new StringBuilder();

                string filepath = $@".\ByTheCake\Resources\Images\{this.imagePath}";

                if (File.Exists(filepath))
                {
                    this.StatusCode = HttpStatusCode.Ok;

                    response.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {StatusMessage}");
                    if (!this.Headers.ContainsKey("Content-Type"))
                    {
                        this.AddHeader("Content-Type", "image/png,image/jpg,image");
                    }

                    byte[] imageData = File.ReadAllBytes(filepath);
                    if (!this.Headers.ContainsKey("Content-Length"))
                    {
                        this.AddHeader("Content-Length", $"{imageData.Length}");
                    }
                    this.Data = imageData;

                    response.Append(this.Headers.ToString());
                    response.AppendLine();
                }
                else
                {
                    this.StatusCode = HttpStatusCode.NotFound;

                    response.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {StatusMessage}");

                    response.Append(this.Headers.ToString());
                    response.AppendLine();
                }

                return response.ToString();
            }
        }

        public byte[] Data { get; private set; }

        public void AddHeader(string key, string value)
        {
            this.Headers.Add(new HttpHeader(key, value));
        }
    }
}
