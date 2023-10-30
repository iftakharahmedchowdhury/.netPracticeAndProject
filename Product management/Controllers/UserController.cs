using Product_management.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Product_management.Controllers
{
   
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        
        public ActionResult Index(User User)
        {
            var db = new ProductManagementDbEntities();
            var existingUser = db.Users.FirstOrDefault(c => c.UserUserName == User.UserUserName);
            

            if (existingUser != null && User.UserPassword == existingUser.UserPassword && existingUser.UserType== "Customer")
            {
                // Create cookie so that it can store user information
                HttpCookie UserCookie = new HttpCookie("UserInfo");
                UserCookie["UserUserName"] = existingUser.UserUserName;
                UserCookie["UserId"] = existingUser.UserId.ToString();
                UserCookie.Expires = DateTime.Now.AddMinutes(2); // Set the cookie to expire time
                Response.Cookies.Add(UserCookie);

                return RedirectToAction("firstPage", "User");
            }
            else if (existingUser != null && User.UserPassword == existingUser.UserPassword && existingUser.UserType == "Admin")
            {
                HttpCookie adminCookie = new HttpCookie("AdminInfo");
                adminCookie["UserUserName"] = existingUser.UserUserName;
                adminCookie["UserId"] = existingUser.UserId.ToString();
                adminCookie.Expires = DateTime.Now.AddMinutes(2); //set time
                Response.Cookies.Add(adminCookie);

                return RedirectToAction("Index", "Admin");

            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User c)
        {
            var db = new ProductManagementDbEntities();
            db.Users.Add(c);
            db.SaveChanges();
            return RedirectToAction("Index");

        }


        public ActionResult Logout()
        {
            
            if (Request.Cookies["UserInfo"] != null)
            {
                var UserCookie = new HttpCookie("UserInfo");
                UserCookie.Expires = DateTime.Now.AddYears(-1); // remove user info from cookie so that user can logout
                Response.Cookies.Add(UserCookie);
            }

          
            return RedirectToAction("Index");
        }
        public ActionResult LogoutAdmin()
        {
      
            if (Request.Cookies["AdminInfo"] != null)
            {
                var adminCookie = new HttpCookie("AdminInfo");
                adminCookie.Expires = DateTime.Now.AddYears(-1); // logout
                Response.Cookies.Add(adminCookie);
            }

        
            return RedirectToAction("Index"); 
        }

        public ActionResult firstPage() {
           
            int userId = 0;
            var db = new ProductManagementDbEntities();

            if (Request.Cookies["UserInfo"] != null && int.TryParse(Request.Cookies["UserInfo"]["UserId"], out userId))
            {
                var userOrders = db.OrderDetails.Where(o => o.CusId == userId).ToList();
                return View(userOrders);
            }

          
            return View("Error");


        }

        [HttpGet]
        public ActionResult Deshboard()
        {
            if (Request.Cookies["UserInfo"] != null)
            {
                string UserUserName = Request.Cookies["UserInfo"]["UserUserName"];
                
                    var db = new ProductManagementDbEntities();
                    var data = db.Products.ToList();
                    return View(data);
              
            }

       
            return RedirectToAction("Index"); 
        }


        public ActionResult AddToCart(int productId, string productName, int quantity, double productPrice, string status, int customerId, string orderDate)
        {
            if (Session["ProductBasket"] == null)
            {
                Session["ProductBasket"] = new List<CartItem>();
            }

            List<CartItem> productBasket = (List<CartItem>)Session["ProductBasket"];

            

           
            var existingProduct = productBasket.FirstOrDefault(item => item.ProductId == productId);
            // Checking is that same product already in the cart
            if (existingProduct != null)
            {
                // only increses the quantity
                existingProduct.ProductQuantity += quantity;
            }
            else
            {
                // add product in cart
                productBasket.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    ProductQuantity = quantity,
                    ProductPrice = productPrice,
                    Status = status,
                    CustomerId = customerId,
                    OrderDate = DateTime.Parse(orderDate) 
                });
            }

            Session["ProductBasket"] = productBasket;

            return RedirectToAction("Deshboard");
          
        }



        [HttpGet]
        public ActionResult ViewCart()
        {
            // Retrieve the shopping cart items from the session
            List<CartItem> cartItems = Session["ProductBasket"] as List<CartItem>;

            if (cartItems == null || cartItems.Count == 0)
            {
                ViewBag.Message = "Your cart is empty.";
            }
            else
            {
                // store in details in viewbag
                ViewBag.ProductIdsInCart = cartItems;
            }

            return View("ViewCart");
        }

        
      
        [HttpPost]
        public ActionResult ConfirmOrder()
        {
            // Retrieve the product details from the session
            var productIdsInCart = Session["ProductBasket"] as List<CartItem>;

            if (productIdsInCart != null)
            {
                // Get uer id from cookie
                int UserId;
                if (Request.Cookies["UserInfo"] != null && int.TryParse(Request.Cookies["UserInfo"]["UserId"], out UserId))
                {
                    using (var db = new ProductManagementDbEntities())
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                // Create a new order for each checkout
                                var orderD = new OrderDetail
                                {
                                    OrderStatus = "Ordered",
                                    CusId = UserId,
                                    OrderDate = DateTime.Now
                                };
                                db.OrderDetails.Add(orderD);
                                db.SaveChanges(); 

                                // Add products to the order
                                foreach (var product in productIdsInCart)
                                {
                                    var orderM = new OrderMapTable
                                    {
                                        Odid = orderD.OrderId, 
                                        Pid = product.ProductId,
                                        Quantity = product.ProductQuantity,
                                        orderPrice = product.ProductPrice * product.ProductQuantity
                                    };
                                    db.OrderMapTables.Add(orderM);
                                }

                                db.SaveChanges(); 

                                // after update both table delete the seassion
                                Session.Remove("ProductBasket");

                                transaction.Commit(); // all the operations within the transaction have been completed successfully.


                                return RedirectToAction("ThankYouForOrder");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback(); 
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Index"); 
        }





        public ActionResult ThankYouForOrder()
        {
            return View();
        }

        public ActionResult OrderHistory(int userId)
        {
            var db = new ProductManagementDbEntities();

            var userOrders = db.OrderDetails.Where(o => o.CusId == userId).ToList();

            return View(userOrders);
        }









    }
}