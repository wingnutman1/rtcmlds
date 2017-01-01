using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_Web.Models;
using CMS_Web.DAL;

namespace CMS_Web.Controllers
{
    public class JournalEntryTypesController : Controller
    {
        private CMS_WebContext db = new CMS_WebContext();

        //
        // GET: /JournalEntryTypes/

        public ActionResult Index()
        {
            return View(db.JournalEntryTypes.ToList());
        }

        //
        // GET: /JournalEntryTypes/Details/5

        public ActionResult Details(int id = 0)
        {
            JournalEntryType journalType = db.JournalEntryTypes.Find(id);
            if (journalType == null)
            {
                return HttpNotFound();
            }
            return View(journalType);
        }

        //
        // GET: /JournalEntryTypes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /JournalEntryTypes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JournalEntryType newType)
        {
            if (ModelState.IsValid)
            {
                db.JournalEntryTypes.Add(newType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newType);
        }

        //
        // GET: /JournalEntryTypes/Edit/5

        public ActionResult Edit(int id = 0)
        {
            JournalEntryType journalType = db.JournalEntryTypes.Find(id);
            if (journalType == null)
            {
                return HttpNotFound();
            }
            return View(journalType);
        }

        //
        // POST: /JournalEntryTypes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JournalEntryType newType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newType);
        }

        //
        // GET: /JournalEntryTypes/Delete/5

        public ActionResult Delete(int id = 0)
        {
            JournalEntryType journalType = db.JournalEntryTypes.Find(id);
            if (journalType == null)
            {
                return HttpNotFound();
            }
            return View(journalType);
        }

        //
        // POST: /JournalEntryTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JournalEntryType journalType = db.JournalEntryTypes.Find(id);
            db.JournalEntryTypes.Remove(journalType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}