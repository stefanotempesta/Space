using SocialSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialSharing.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new DatabaseContext())
            {
                return View(db.Articles.OrderBy(a => a.Title).ToList());
            }
        }

        public ActionResult Open(int id)
        {
            using (var db = new DatabaseContext())
            {
                return View(db.Articles.Find(id));
            }
        }
    }
}