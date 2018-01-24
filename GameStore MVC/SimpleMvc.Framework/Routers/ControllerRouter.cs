namespace SimpleMvc.Framework.Routers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Framework.Helpers;
    using Framework.Controllers;
    using WebServer.Contracts;
    using WebServer.Exceptions;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;
    using Attributes.Methods;
    using WebServer.Enums;
    using SimpleMvc.Framework.Contracts;

    public class ControllerRouter : IHandleable
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var getParameters = new Dictionary<string, string>(request.UrlParameters);
            var postParameters = new Dictionary<string, string>(request.FormData);
            string requestMethod = request.Method.ToString().ToUpper();
            string controllerName = string.Empty;
            string actionName = string.Empty;

            var pathParts = request.Path
                .Split(new[] { '/', '?' }, StringSplitOptions.RemoveEmptyEntries);

            if (request.Path == "/")
            {
                controllerName = $"Home{MvcContext.Get.ControllersSuffix}";
                actionName = "Index";
            }
            else if (pathParts.Length < 2)
            {
                BadRequestException.ThrowFromInvalidRequest();
            }
            else
            {
                controllerName = $"{pathParts[0].Capitalize()}{MvcContext.Get.ControllersSuffix}";

                actionName = pathParts[1].Capitalize();
            }

            Controller controller = this.GetController(controllerName);

            if (controller != null)
            {
                controller.Request = request;
                controller.InitializeController();
            }

            MethodInfo methodInfo = this.GetMethod(controller, requestMethod, actionName);

            if (methodInfo == null)
            {
                return new NotFoundResponse();
            }

            IEnumerable<ParameterInfo> parameters = methodInfo.GetParameters();

            object[] methodParameters = this.AddParameters(parameters, getParameters, postParameters);

            try
            {
                IHttpResponse response = this.GetResponse(methodInfo,controller,methodParameters);
                return response;
            }
            catch (Exception e)
            {

                return new InternalServerErrorResponse(e);
            }
        }

        private IHttpResponse GetResponse(MethodInfo methodInfo, Controller controller, object[] methodParameters)
        {
            object actionResult = methodInfo
                .Invoke(controller, methodParameters);

            IHttpResponse response = null;

            if (actionResult is IViewable)
            {
                string content = ((IViewable)actionResult).Invoke();

                response = new ContentResponse(HttpStatusCode.Ok,content);
            }
            else if (actionResult is IRedirectable)
            {
                string redirectUrl = ((IRedirectable)actionResult).Invoke();

                response = new RedirectResponse(redirectUrl);
            }

            return response;
        }

        private object[] AddParameters(IEnumerable<ParameterInfo> parameters, Dictionary<string, string> getParameters, Dictionary<string, string> postParameters)
        {
            object[] methodParameters = new object[parameters.Count()];

            int index = 0;

            foreach (ParameterInfo parameter in parameters)
            {
                if (parameter.ParameterType.IsPrimitive ||
                    parameter.ParameterType == typeof(string))
                {
                    methodParameters[index] = this.ProcessPrimitiveParameter(parameter, getParameters);
                    index++;
                }
                else
                {
                    methodParameters[index] = this.ProcessComplexParameter(parameter, postParameters);
                    index++;
                }
            }

            return methodParameters;
        }

        private object ProcessComplexParameter(ParameterInfo parameter, Dictionary<string, string> postParameters)
        {
            //take type of model
            Type modelType = parameter.ParameterType;
            //create instance of it
            Object modelInstance = Activator.CreateInstance(modelType);
            //get propreties of the model
            PropertyInfo[] modelProperties = modelType.GetProperties();

            foreach (var property in modelProperties)
            {
                string propertyValue = postParameters[property.Name];

                var value = Convert.ChangeType(
                    propertyValue,
                    property.PropertyType
                );

                property.SetValue(
                    modelInstance,
                    value);
            }

            return Convert.ChangeType(
                modelInstance,
                modelType);
        }

        private object ProcessPrimitiveParameter(ParameterInfo parameter, Dictionary<string, string> getParameters)
        {
            string parameterValue = getParameters[parameter.Name];

            return Convert.ChangeType(
                parameterValue,
                parameter.ParameterType
            );
        }

        private MethodInfo GetMethod(Controller controller, string requestMethod, string actionName)
        {
            IEnumerable<MethodInfo> methods = this.GetSuitableMethods(controller, actionName);
            foreach (var methodInfo in methods)
            {
                IEnumerable<Attribute> methodAttributes = methodInfo
                    .GetCustomAttributes()
                    .Where(a => a is HttpMethodAttribute);

                if (!methodAttributes.Any() && requestMethod == "GET")
                {
                    return methodInfo;
                }

                foreach (HttpMethodAttribute attribute in methodAttributes)
                {
                    if (attribute.IsValid(requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if(controller == null)
            {
                return new MethodInfo[0];
            }

            return controller.GetType()
                .GetMethods()
                .Where(m => m.Name == actionName);
        }

        private Controller GetController(string controllerName)
        {
            var controllerFullQualifiedName = string.Format(
                "{0}.{1}.{2}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllersFolder,
                controllerName);

            Type type = Type.GetType(controllerFullQualifiedName);

            if(type == null)
            {
                return null;
            }

           Controller controller = (Controller)Activator.CreateInstance(type);

            return controller;
        }
    }
}
