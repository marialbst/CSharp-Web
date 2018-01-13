namespace SimpleMvc.Framework.Routers
{
    using System.Linq;
    using WebServer.Contracts;
    using WebServer.Http.Contracts;
    using System;
    using WebServer.Http.Response;
    using WebServer.Enums;
    using System.IO;

    public class ResourceRouter : IHandleable
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            string fileFullName = request.Path.Split('/').Last();
            string fileExtension = request.Path.Split('.').Last();

            IHttpResponse fileResponse = null;

            try
            {
                byte[] fileContent = this.ReadFileContent(fileFullName, fileExtension);
                fileResponse = new FileResponse(HttpStatusCode.Found, fileContent);
            }
            catch (Exception)
            {
                return new NotFoundResponse();
            }

            return fileResponse;
        }

        private byte[] ReadFileContent(string fileFullName, string fileExtension)
        {
            return File.ReadAllBytes(string.Format(
                "{0}\\{1}\\{2}",
                MvcContext.Get.ResourcesFolder,
                fileExtension,
                fileFullName
            ));
        }
    }
}
