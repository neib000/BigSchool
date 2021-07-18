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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BigSchoolContext db = new BigSchoolContext();
            var upcommingCourse = db.Courses.Where(p => p.DateTime>DateTime.Now).OrderBy(p => p.DateTime).ToList();
            //Lấy user login hiện tại
            var userId = User.Identity.GetUserId();

            foreach(Course i in upcommingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                i.Name = user.Name;
                if (userId != null)
                {
                    i.isLogin = true;
                    Attendance find = db.Attendances.FirstOrDefault(p => p.CourseId == i.Id && p.Attendee == userId);
                    if(find == null)
                    {
                        i.isShowGoing = true;
                    }
                    Following findFollow = db.Followings.FirstOrDefault(p => p.FollowerId == userId && p.FolloweeId == i.LecturerId);
                    if (findFollow == null)
                    {
                        i.isShowFollow = true;
                    }
                }
            }

            return View(upcommingCourse);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}