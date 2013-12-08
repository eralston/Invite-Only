using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using System.Threading.Tasks;

namespace InviteOnly
{
    /// <summary>
    /// A static class that holds helpful extensions for InviteOnly
    /// </summary>
    public static class InviteOnlyExtensions
    {
        /// <summary>
        /// HTML Helper extension for easily generating an action link with an invite's value
        /// If you want to manually generate an invite link, just ensure to pass the invite.Value as the routevalue "invite"
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="invite"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLinkWithInvite(this HtmlHelper helper, Invite invite, string linkText, string actionName, string controllerName = null, object htmlAttributes = null)
        {
            return helper.ActionLink(linkText, actionName, controllerName, new { invite = invite.Value }, htmlAttributes);
        }
        
        /// <summary>
        /// Finds the invite with the given value using the given Invite context
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Invite FindInvite(this IQueryable<Invite> queryable, string value)
        {
            return queryable.Where(i => i.Value == value).Take(1).SingleOrDefault();
        }

        /// <summary>
        /// Gets the current invite for a controller implementing IInviteContextProvider
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string GetInviteValue(this ControllerBase controller)
        {
            return controller.ControllerContext.HttpContext.Request.QueryString["Invite"];
        }

        /// <summary>
        /// Gets the invite with the given value from the given provider
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="inviteValue"></param>
        /// <returns></returns>
        public static Invite GetInvite(this IInviteContextProvider provider, string inviteValue)
        {
            IInviteContext context = provider.InviteContext;

            if (context == null)
                return null;

            return context.Invites.FindInvite(inviteValue);
        }

        /// <summary>
        /// Gets the invite for the given controller, returning null if not found
        /// NOTE: The controller must implement IInviteContextProvider
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static Invite GetCurrentInvite(this ControllerBase controller)
        {
            string inviteValue = controller.GetInviteValue();

            if (inviteValue == null)
                return null;

            IInviteContextProvider provider = controller as IInviteContextProvider;

            if (provider == null)
                return null;

            return provider.GetInvite(inviteValue);
        }

        /// <summary>
        /// Returns true if the controller currently has an unfulfilled invite
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static bool HasCurrentInvite(this ControllerBase controller)
        {
            Invite invite = controller.GetCurrentInvite();

            if (invite == null)
                return false;

            return !invite.Fulfilled;
        }
    }
}
