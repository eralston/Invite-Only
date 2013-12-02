using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using InviteOnly.Models;

using Invite_Only.Models;

namespace Invite_Only.Controllers
{
    public class HomeController : Controller, IInviteContextProvider
    {

        InviteContext _context = new InviteContext();
        public IInviteContext InviteContext
        {
            get { return _context; }
        }

        public ActionResult Index()
        {
            Invite firstInvite = _context.Invites.Where(i => i.Fulfilled == false).Take(1).SingleOrDefault();
            if (firstInvite == null)
                firstInvite = Invite.Create(_context);
            ViewBag.FirstInvite = firstInvite;
            return View();
        }

        public ActionResult Denied()
        {
            return View();
        }

        [InviteOnly.Filters.InviteOnly(DenyAction="Denied")]
        public ActionResult InviteOnlyAction()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();

            base.Dispose(disposing);
        }

        
    }
}