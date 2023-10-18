using RegistrationModule.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistrationModule.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Info()
        {
            /*var db = new DemoF23_CEntities();
            ViewBag.Courses = db.Courses.ToList();
            return View();*/
            var db = new DemoF23_CEntities();

            // Get a list of courses, excluding those with an enrollment count of 40 or more.
            var availableCourses = db.Courses.Where(course => course.CourseStudents.Count < 40).ToList();

            ViewBag.Courses = availableCourses;
            return View();
        }
        [HttpPost]
        /* public ActionResult Register(int SId, int[] Courses) {
             var db = new DemoF23_CEntities();
             foreach (var c in Courses) {
                 var cs = new CourseStudent() {

                     StId = SId,
                     CId = c,
                     EnrollTime = DateTime.Now
                 };
                 db.CourseStudents.Add(cs);
             }
             db.SaveChanges();
             return RedirectToAction("Info");
         }*/
        public ActionResult Register(int SId, int[] Courses)
        {
            var db = new DemoF23_CEntities();

            foreach (var c in Courses)
            {
                // Check if the course has fewer than 40 students enrolled before allowing registration.
                var course = db.Courses.FirstOrDefault(x => x.Id == c);
                if (course != null && course.CourseStudents.Count < 40)
                {
                    var cs = new CourseStudent()
                    {
                        StId = SId,
                        CId = c,
                        EnrollTime = DateTime.Now
                    };
                    db.CourseStudents.Add(cs);
                }
                else
                {
                    // Handle the case where the course is full (more than or equal to 40 students).
                    // You can return an error message or perform any other appropriate action.
                    // For example, you can set a ViewBag message.
                    ViewBag.ErrorMessage = "Course is full, and registration is not allowed.";

                }
            }

            db.SaveChanges();
            return RedirectToAction("Info");
        }

    }
}