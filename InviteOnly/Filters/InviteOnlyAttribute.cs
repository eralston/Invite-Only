using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InviteOnly
{
    /// <summary>
    /// An action filter attribute that only allows requests with an existing valid request to execute the action or controller marked
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class InviteOnlyAttribute : AuthorizeAttribute
    {
        #region Fields & Properties

        /// <summary>
        /// Gets or sets the name of the Controller to which we redirect on a deny 
        /// </summary>
        public string DenyController { get; set; }

        /// <summary>
        /// Gets or sets the action of the controller to which we redirect on deny
        /// </summary>
        public string DenyAction { get; set; }

        #endregion

        /// <summary>
        /// Called when an action is called
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If the invite has not valid, then redirect them to the deny action
            if (!filterContext.Controller.HasCurrentInvite())
            {
                Redirect(filterContext);
            }
        }

        /// <summary>
        /// Redirects the action to the action provided by the controller and action properties
        /// </summary>
        /// <param name="filterContext"></param>
        private void Redirect(AuthorizationContext filterContext)
        {
            string controller = this.DenyController ?? "Home";
            string action = this.DenyAction ?? "Index";

            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = controller, action = action }));
        }
    }
}
