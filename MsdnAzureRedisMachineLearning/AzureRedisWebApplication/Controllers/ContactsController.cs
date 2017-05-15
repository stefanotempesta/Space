using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AzureRedisWebApplication.Models;
using System.Threading.Tasks;
using AzureRedisWebApplication.Contracts;
using AzureRedisWebApplication.Managers;

namespace AzureRedisWebApplication.Controllers
{
    public class ContactsController : Controller
    {
        private IContactManager contactManager = new ContactManager();

        // GET: Contacts
        public ActionResult Index()
        {
            var contacts = contactManager.GetAll();

            return View(contacts);
        }

        // GET: Contacts/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            Contact contact = await contactManager.Get(id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Email,Country")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Id = Guid.NewGuid();
                await contactManager.Create(contact);

                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            Contact contact = await contactManager.Get(id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Email,Country")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                await contactManager.Update(contact);

                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            Contact contact = await contactManager.Get(id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Contact contact = await contactManager.Get(id);
            await contactManager.Delete(contact);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                contactManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
