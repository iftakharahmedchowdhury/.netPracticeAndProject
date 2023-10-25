using Product_management.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product_management.Controllers
{
    public class CategoryController : Controller
    {
        
        public ActionResult Index()
        {
            var db = new ProductManagementEntities1();
            var data = db.Categories.ToList();
            return View(data);
        }

        [HttpGet]
        public ActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category c)
        {
            var db = new ProductManagementEntities1();
            db.Categories.Add(c);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
      
        public ActionResult Delete(int p)
        {
            var db = new ProductManagementEntities1();
            var data = db.Categories.Find(p);
            db.Categories.Remove(data);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new ProductManagementEntities1();
            var ex = (from ed in db.Categories
                      where ed.CatagoryId == id
                      select ed).SingleOrDefault();

            return View(ex);

        }
        [HttpPost]
        public ActionResult Edit(Category catagory)
        {
            var db = new ProductManagementEntities1();
            var exdata = db.Categories.Find(catagory.CatagoryId);
            exdata.CategoryName = catagory.CategoryName;

            exdata.CatagoryId = catagory.CatagoryId;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var db = new ProductManagementEntities1();
            var categoryInfo = db.Categories.Find(id);
            return View(categoryInfo);

        }

    }
}