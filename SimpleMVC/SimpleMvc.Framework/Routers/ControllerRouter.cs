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
        private IDictionary<string, string> getParameters;
        private IDictionary<string, string> postParameters;
        private string requestMethod;
        private string controllerName;
        private Controller controllerInstance;
        private string actionName;
        private object[] methodParameters;

        public IHttpResponse Handle(IHttpRequest request)
        {
            this.getParameters = new Dictionary<string, string>(request.UrlParameters);
            this.postParameters = new Dictionary<string, string>(request.FormData);
            this.requestMethod = request.Method.ToString().ToUpper();

            this.ExtractControllerAndActionName(request);

            MethodInfo methodInfo = this.GetActionForExecution();

            if(methodInfo == null)
            {
                return new NotFoundResponse();
            }

            this.ExtractMethodParameters(methodInfo);

            var actionResult = (IInvocable)methodInfo.Invoke(this.GetControllerInstance(), this.methodParameters);

            string content = actionResult.Invoke();

            return new ContentResponse(HttpStatusCode.Ok, content);
        }

        private void ExtractControllerAndActionName(IHttpRequest request)
        {
            var pathParts = request.Path
                .Split(new[] { '/', '?' }, StringSplitOptions.RemoveEmptyEntries);

            if(pathParts.Length < 2)
            {
                BadRequestException.ThrowFromInvalidRequest();
            }
            this.controllerName = $"{pathParts[0].Capitalize()}{MvcContext.Get.ControllersSuffix}";
            this.actionName = pathParts[1].Capitalize();
        }

        private void ExtractMethodParameters(MethodInfo methodInfo)
        {
            ParameterInfo[] parameters = methodInfo.GetParameters();

            this.methodParameters = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameter = parameters[i];

                if (parameter.ParameterType.IsPrimitive ||
                    parameter.ParameterType == typeof(string))
                {
                    string parameterValue = this.getParameters[parameter.Name];

                    var value = Convert.ChangeType(
                        parameterValue,
                        parameter.ParameterType
                    );

                    this.methodParameters[i] = value;
                }
                else
                {
                    //take type of model
                    Type modelType = parameter.ParameterType;
                    //create instance of it
                    Object modelInstance = Activator.CreateInstance(modelType);
                    //get propreties of the model
                    PropertyInfo[] modelProperties = modelType.GetProperties();

                    foreach (var property in modelProperties)
                    {
                        string propertyValue = this.postParameters[property.Name];

                        var value = Convert.ChangeType(
                            propertyValue,
                            property.PropertyType
                        );

                        property.SetValue(
                            modelInstance,
                            value);
                    }

                    this.methodParameters[i] = Convert.ChangeType(
                        modelInstance,
                        modelType
                    );
                }
            }
        }

        private MethodInfo GetActionForExecution()
        {
            foreach (var methodInfo in this.GetSuitableMethods())
            {
                IEnumerable<Attribute> methodAttributes = methodInfo
                    .GetCustomAttributes()
                    .Where(a => a is HttpMethodAttribute);

                if (!methodAttributes.Any() && this.requestMethod == "GET")
                {
                    return methodInfo;
                }

                foreach (HttpMethodAttribute attribute in methodAttributes)
                {
                    if (attribute.IsValid(this.requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods()
        {
            var controller = this.GetControllerInstance();

            if(controller == null)
            {
                return new MethodInfo[0];
            }

            return controller.GetType()
                .GetMethods()
                .Where(m => m.Name == this.actionName);
        }

        private Controller GetControllerInstance()
        {
            if(controllerInstance != null)
            {
                return controllerInstance;
            }

            var controllerFullQualifiedName = string.Format(
                "{0}.{1}.{2}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllersFolder,
                this.controllerName);

            Type type = Type.GetType(controllerFullQualifiedName);

            if(type == null)
            {
                return null;
            }

           this.controllerInstance = (Controller)Activator.CreateInstance(type);

            return this.controllerInstance;
        }
    }
}
