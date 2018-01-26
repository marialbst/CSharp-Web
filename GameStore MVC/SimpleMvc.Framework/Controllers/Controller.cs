namespace SimpleMvc.Framework.Controllers
{
    using Contracts;
    using System.Runtime.CompilerServices;
    using Helpers;
    using Models;
    using Views;
    using ActionResults;
    using System.Reflection;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using Attributes.Property;
    using Security;
    using WebServer.Http.Contracts;
    using WebServer.Http;

    public abstract class Controller
    {

        protected Controller()
        {
            this.ViewModel = new ViewModel();
            this.User = new Authentication();
        }

        public ViewModel ViewModel { get; set; }

        protected internal Authentication User { get; private set; }

        protected internal IHttpRequest Request { get; set; }

        protected IViewable View([CallerMemberName]string caller = "")
        {
            this.InitializeViewData();

            string controllerName = ControllerHelpers.GetControllerName(this);

            string viewFullQualifiedName = string.Format(
                "{0}\\{1}\\{2}",
                MvcContext.Get.ViewsFolder,
                controllerName,
                caller
            );

            IRenderable view = new View(viewFullQualifiedName, this.ViewModel.Data);

            return new ViewResult(view);
        }

        private void InitializeViewData()
        {
            this.ViewModel["displayType"] = this.User.IsAuthenticated ? "block" : "none";
        }

        protected IRedirectable RedirectToAction(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }

        protected bool IsValidModel(object model)
        {
            PropertyInfo[] properties = model.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                IEnumerable<Attribute> attributes = property
                    .GetCustomAttributes()
                    .Where(a => a is PropertyAttribute);

                if (!attributes.Any())
                {
                    continue;
                }

                foreach (PropertyAttribute attribute in attributes)
                {
                    if (!attribute.IsValid(property.GetValue(model)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected internal virtual void InitializeController()
        {
            var userName = this.Request.Session
                .Get<string>(SessionStore.CurrentUserKey);

            if (userName != null)
            {
                this.User = new Authentication(userName);
            }
        }

        protected void SignIn(string name)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, name);
        }

        protected void SignOut()
        {
            this.Request.Session.Remove(SessionStore.CurrentUserKey);
        }
    }
}
