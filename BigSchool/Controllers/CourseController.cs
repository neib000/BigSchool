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
        [Authorize]
        public ActionResult Attending()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance item in listAttendances)
            {
                Course objCourse = item.Course;
                objCourse.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        [Authorize]
        public ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Courses.Where(c => c.LecturerId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach(Course i in courses)
            {
                i.Name = currentUser.Name;
            }
            return View(courses);
        }
        public ActionResult Delete(int id)
        {
            BigSchoolContext db = new BigSchoolContext();
            Course course = db.Courses.Find(id);
            if (course != null)
            {
                db.Courses.Remove(course);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return HttpNotFound();
        }
        public ActionResult Edit(int id)
        {
            BigSchoolContext db = new BigSchoolContext();
            Course course = db.Courses.Find(id);
            course.ListCategory = db.Categories.ToList();
            return View(course);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Course model)
        {
            BigSchoolContext db = new BigSchoolContext();
            Course course = db.Courses.Find(id);

          
            
            if (ModelState.IsValid && course != null)
            {
                course.Id = model.Id;
                course.LecturerId = model.LecturerId;
                course.Place = model.Place;
                course.DateTime = model.DateTime;
                course.CategoryId = model.CategoryId;                
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}