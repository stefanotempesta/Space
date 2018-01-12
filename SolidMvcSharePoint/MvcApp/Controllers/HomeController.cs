using SolidMvcSharePoint.Interfaces;
using SolidMvcSharePoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SolidMvcSharePoint.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IRepositoryContext context)
        {
            this.context = context;
        }

        //private SharePointContext context;
        private readonly IRepositoryContext context;

        //public IRepositoryContext Context { get; set; }

        public ActionResult Index()
        {
            var courses = this.context.Get<InstructorLedCourse>()
                .OrderBy(t => t.Title)
                .ToList();

            return View(courses);
        }

        public ActionResult Course(int id)
        {
            var course = this.context.Find<InstructorLedCourse>(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            course.ContentItems = this.context.Get<Content>()
                .Where(c => c.CourseId == course.Id)
                .OrderBy(c => c.Name)
                .ToList();

            return View(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}