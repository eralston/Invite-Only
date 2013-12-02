using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.Web.Mvc;

using InviteOnly.Models;
using System.Web.Routing;

namespace InviteOnly.Filters
{
    /// <summary>
    /// An action filter attribute that only allows requests with an existing valid request in
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InviteOnlyAttribute : AuthorizeAttribute
    {
        #region Fields & Properties

        private bool _typeSet = false;
        private int _type = 0;

        /// <summary>
        /// Gets or sets the type of invite this must be
        /// NOTE: If not set, this will search only by value
        /// </summary>
        public int Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                _typeSet = true;
            }
        }

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
            // Try handle the invite
            bool valid = CheckForValidInvite(filterContext);

            // If the invite has not valid, then redirect them to the deny action
            if (!valid)
            {
                Redirect(filterContext);
            }
        }

        /// <summary>
        /// Examines the context and tries to find the analogous 
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private bool CheckForValidInvite(AuthorizationContext filterContext)
        {
            string inviteValue = filterContext.RequestContext.HttpContext.Request.QueryString["Invite"];

            // Ensure valid invite value
            if (inviteValue == null)
                return false;

            IInviteContextProvider provider = filterContext.Controller as IInviteContextProvider;

            // Ensure valid context provider
            if (provider == null)
                return false;

            IInviteContext context = provider.InviteContext;

            // Ensure valid context
            if (context == null)
                return false;

            Invite invite = FindInvite(inviteValue, context);

            // Ensure valid invite
            if (invite == null)
                return false;

            // If we have a valid invite, then we are good
            return true;
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

        /// <summary>
        /// Finds the invite with the given value, conditionally using type only if the attribute was declared with it
        /// </summary>
        /// <param name="inviteValue"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private Invite FindInvite(string inviteValue, IInviteContext context)
        {
            if (_typeSet)
                return context.Invites.FindInvite(inviteValue);
            else
                return context.Invites.FindInvite(inviteValue, Type);
        }
    }
}
