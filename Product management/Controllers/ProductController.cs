using Product_management.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product_management.Controllers
{
    public class ProductController : Controller
    {
    
        public ActionResult Index()
        {
            var db = new ProductManagementDbEntities();
            var data = db.Products.ToList();
            return View(data);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Product c)
        {
            var db = new ProductManagementDbEntities();
            db.Products.Add(c);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Delete(int id)
        {
            var db = new ProductManagementDbEntities();
            var data = db.Products.Find(id);
            db.Products.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new ProductManagementDbEntities();
            var ex = (from c in db.Products
                     where c.Productid == id
                     select c).SingleOrDefault();

            return View(ex);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var db = new ProductManagementDbEntities();
            var data = db.Products.Find(product.Productid);
            data.ProductName= product.ProductName;
            data.ProductCount= product.ProductCount;
            data.ProductPrice= product.ProductPrice;
            db.SaveChanges();

            return RedirectToAction("index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var db = new ProductManagementDbEntities();
            var productInfo = db.Products.Find(id);
            return View(productInfo);

            
        }

    }
}