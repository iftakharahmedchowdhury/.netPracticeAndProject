using RegistrationModule.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistrationModule.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            var db = new DemoF23_CEntities();
            var data = db.Students.ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult Create() {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student s) {
            var db = new DemoF23_CEntities();
            db.Students.Add(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       /* [HttpGet]
        public ActionResult Create()
        {
            var db = new DemoF23_CEntities();
            ViewBag.Departments = db.Departments.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student student)
        {
            var db = new DemoF23_CEntities();
            db.Students.Add(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new DemoF23_CEntities();
            var ex = (from ed in db.Students
                      where ed.Id == id
                      select ed).SingleOrDefault();

            return View(ex);

        }
        [HttpPost]
        public ActionResult Edit(Student student)
        {
            var db = new DemoF23_CEntities();
            var exdata = db.Students.Find(student.Id);
            exdata.Name = student.Name;
            exdata.Cgpa = student.Cgpa;
            exdata.DeptId = student.DeptId;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var db = new DemoF23_CEntities();
            var stuInfo = db.Students.Find(id);

            /*var stuInfo = db.Students.ToList();*/


            return View(stuInfo);

        }
        public ActionResult Delete(int id)
        {
            var db = new DemoF23_CEntities();
            var exdata = db.Students.Find(id);
            db.Students.Remove(exdata);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}