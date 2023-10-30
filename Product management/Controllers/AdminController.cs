using Product_management.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product_management.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {

            if (Request.Cookies["AdminInfo"] != null)
            {
                var db = new ProductManagementDbEntities();
                var data = db.OrderDetails.ToList();
                return View(data);

            }
            return RedirectToAction("Index","User");

        }


        public ActionResult Process(int orderId)
        {
            using (var db = new ProductManagementDbEntities())
            {
                // Find all OrderDetail using order id
                var orderDetail = db.OrderDetails.FirstOrDefault(o => o.OrderId == orderId);

                if (orderDetail != null)
                {
                    orderDetail.OrderStatus = "Processing";

                    // Find all products from OrderMapTable accorading to order id
                    var orderProducts = db.OrderMapTables.Where(o => o.Odid == orderId).ToList();

                    foreach (var orderMap in orderProducts)
                    {
                        var product = db.Products.FirstOrDefault(p => p.Productid == orderMap.Pid);

                        if (product != null)
                        {
                            product.ProductCount -= orderMap.Quantity;
                        }
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View("Error");
        }

    
        public ActionResult Decline(int orderId)
        {
            using (var db = new ProductManagementDbEntities())
            {
                
                var order = db.OrderDetails.FirstOrDefault(o => o.OrderId == orderId);

                if (order != null)
                {
                   
                    order.OrderStatus = "Decline";
                    db.SaveChanges();

                   
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
            }
        }


        public ActionResult GetOrderDetails(int orderId)
        {
            using (var db = new ProductManagementDbEntities())
            {
                var order = db.OrderDetails.FirstOrDefault(o => o.OrderId == orderId);

                if (order != null)
                {
                    var customer = db.Users.FirstOrDefault(c => c.UserId == order.CusId);
                    var orderMap = db.OrderMapTables.Where(om => om.Odid == orderId).ToList();

                    var products = orderMap
                        .Select(om => new
                        {
                            ProductName = db.Products.FirstOrDefault(p => p.Productid == om.Pid).ProductName,
                            Quantity = om.Quantity  
                })
                        .ToList();

                    var orderDetails = new
                    {
                        CustomerName = customer.UserName,
                        CustomerEmail = customer.UserEmail,
                        OrderedProducts = products
                    };

                    return Json(orderDetails, JsonRequestBehavior.AllowGet);
                }

                return Json("Order not found", JsonRequestBehavior.DenyGet);
            }
        }






    }
}