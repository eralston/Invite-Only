using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Invite_Only.Models;

using InviteOnly;

namespace Invite_Only.Controllers
{
    public class InviteController : Controller, IInviteContextProvider
    {
        private ExampleContext _context = new ExampleContext();

        /// <summary>
        /// A context that implements persisting the Invite model
        /// </summary>
        public IInviteContext InviteContext { get { return _context; } }

        // GET: /Invite/
        public ActionResult Index()
        {
            return View(_context.Invites.ToList());
        }

        // GET: /Invite/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = _context.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            return View(invite);
        }

        // GET: /Invite/Create
        public ActionResult Create()
        {
            Invite.Create(_context);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Invite/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = _context.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            return View(invite);
        }

        // POST: /Invite/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Type,Value,CreatedDate")] Invite invite)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(invite).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invite);
        }

        // GET: /Invite/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = _context.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            return View(invite);
        }

        // POST: /Invite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invite invite = _context.Invites.Find(id);
            _context.Invites.Remove(invite);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
