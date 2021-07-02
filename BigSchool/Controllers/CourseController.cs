using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CourseController : Controller
    {
       

        // GET: Course
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Create()
        {
            BigSchoolContext db = new BigSchoolContext();
            Course objCourse = new Course ();
            objCourse.ListCategory = db.Categories.ToList();
            return View(objCourse);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            BigSchoolContext db = new BigSchoolContext();

            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = db.Categories.ToList();
                return View("Create", objCourse);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            db.Courses.Add(objCourse);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}