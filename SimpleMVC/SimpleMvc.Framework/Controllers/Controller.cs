namespace SimpleMvc.Framework.Controllers
{
    using Contracts;
    using Contracts.Generic;
    using System;
    using System.Runtime.CompilerServices;
    using ViewEngine;
    using ViewEngine.Generic;
    using Helpers;

    public abstract class Controller
    {
        protected IActionResult View([CallerMemberName]string caller = "")
        {
            string controllerName = ControllerHelpers.GetControllerName(this);

            string viewFullQualifiedName = ControllerHelpers.GetViewFullQualifiedName(controllerName, caller);

            return new ActionResult(viewFullQualifiedName);
        }

        protected IActionResult View(string controller, string action)
        {
            string viewFullQualifiedName = ControllerHelpers
                .GetViewFullQualifiedName(controller, action);

            return new ActionResult(viewFullQualifiedName);
        }

        protected IActionResult<T> View<T>(T model, [CallerMemberName]string caller = "")
        {
            string controllerName = ControllerHelpers.GetControllerName(this);

            string viewFullQualifiedName = ControllerHelpers
                .GetViewFullQualifiedName(controllerName, caller);

            return new ActionResult<T>(viewFullQualifiedName, model);
        }

        protected IActionResult<T> View<T>(T model, string controller, string action)
        {
            string viewFullQualifiedName = ControllerHelpers
                .GetViewFullQualifiedName(controller, action);

            return new ActionResult<T>(viewFullQualifiedName, model);
        }
    }
}
