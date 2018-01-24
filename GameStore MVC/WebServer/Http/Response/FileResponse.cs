namespace WebServer.Http.Response
{
    using Enums;
    using Exceptions;

    public class FileResponse : HttpResponse
    {
        public FileResponse(HttpStatusCode statusCode, byte[] fileData)
        {
            this.ValidateStatusCode(statusCode);

            this.FileData = fileData;
            this.StatusCode = statusCode;

            this.Headers.Add(HttpHeader.ContentLength, fileData.Length.ToString());
            this.Headers.Add(HttpHeader.ContentDisposition, "attachment");
        }

        public byte[] FileData { get; private set; }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            int statusCodeNumber = (int)statusCode;

            if (299 > statusCodeNumber && statusCodeNumber < 400)
            {
                throw new InvalidResponseException("File responses need status code between 300 and 400 exclusive");
            }
        }
    }
}
