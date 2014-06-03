using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using InviteOnly;

using Invite_Only.Models;

namespace Invite_Only.Controllers
{
    public class HomeController : Controller, IInviteContextProvider
    {
        ExampleContext _context = new ExampleContext();

        // Property required by IInviteContextProvider
        public IInviteContext InviteContext { get { return _context; } }

        // This action is only allowed if the request has a valid invite code in the querystring
        // Otherwise, this will redirect to the Denied action
        [InviteOnly(DenyAction = "Denied")]
        public ActionResult InviteOnlyAction()
        {
            // Pull the current invite for this request and mark it as fulfilled
            Invite invite = this.GetCurrentInvite();
            invite.Fulfilled = true;
            _context.SaveChanges();
            return View();
        }

        [InviteOnly(AsAuthenticated = true)]
        public ActionResult InviteOnlyActionAsAuthenticated()
        {
            // Pull the current invite for this request and mark it as fulfilled
            Invite invite = this.GetCurrentInvite();
            if(invite != null)
            {
                invite.Fulfilled = true;
                _context.SaveChanges();
            }

            ViewBag.IsAuthenticated = Request.IsAuthenticated;
            
            return View();
        }

        public ActionResult Denied() { return View(); }

        public ActionResult Index()
        {
            // Try to select the first unfulfilled invite in the system
            Invite firstInvite = _context.Invites.Where(i => i.Fulfilled == false).Take(1).SingleOrDefault();
            if (firstInvite == null)
            {
                // If we didn't find one, then make a new one
                firstInvite = Invite.Create(_context);
                _context.SaveChanges();
            }

            // Pass the invite into the view
            return View(firstInvite);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();

            base.Dispose(disposing);
        }
    }
}