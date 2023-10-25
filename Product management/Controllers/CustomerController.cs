using Product_management.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product_management.Controllers
{
   
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        /* public ActionResult Index(Customer customer)
         {
             var db = new ProductManagementEntities1();
             var existingCustomer = db.Customers.FirstOrDefault(c => c.CustomerUserName == customer.CustomerUserName);

             if (existingCustomer != null && customer.CustomerPassword == existingCustomer.CustomerPassword)
             {

                 return RedirectToAction("Index", "Product");
             }
             else
             {

                 ModelState.AddModelError("", "Invalid username or password");
                 return View();
             }


            // return View("index");
         }*/
        public ActionResult Index(Customer customer)
        {
            var db = new ProductManagementEntities1();
            var existingCustomer = db.Customers.FirstOrDefault(c => c.CustomerUserName == customer.CustomerUserName);

            if (existingCustomer != null && customer.CustomerPassword == existingCustomer.CustomerPassword)
            {
                // Create a cookie to store the customer's information
                HttpCookie customerCookie = new HttpCookie("CustomerInfo");
                customerCookie["CustomerUserName"] = existingCustomer.CustomerUserName;
                customerCookie["CustomerId"] = existingCustomer.CustomerId.ToString();
                customerCookie.Expires = DateTime.Now.AddMinutes(2); // Set the cookie to expire in 5 minutes

                // Add the cookie to the response
                Response.Cookies.Add(customerCookie);

                return RedirectToAction("Deshboard", "Customer");
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
        public ActionResult SignUp(Customer c)
        {
            var db = new ProductManagementEntities1();
            db.Customers.Add(c);
            db.SaveChanges();
            return RedirectToAction("Index");

        }


        public ActionResult Logout()
        {
            // Clear the "CustomerInfo" cookie
            if (Request.Cookies["CustomerInfo"] != null)
            {
                var customerCookie = new HttpCookie("CustomerInfo");
                customerCookie.Expires = DateTime.Now.AddYears(-1); // Expire the cookie by setting its expiration to the past
                Response.Cookies.Add(customerCookie);
            }

            // Redirect to the login page or another appropriate page
            return RedirectToAction("Index"); // Replace "Login" with your login page action name
        }

        [HttpGet]
        public ActionResult Deshboard()
        {
            if (Request.Cookies["CustomerInfo"] != null)
            {
                string customerUserName = Request.Cookies["CustomerInfo"]["CustomerUserName"];
                // Get the customer's username from the cookie

                // Compare the username from the cookie with the current user's username
               /* if (User.Identity.IsAuthenticated && User.Identity.Name == customerUserName)
                {*/
                    // If the user is authenticated and the usernames match, proceed to the dashboard
                    var db = new ProductManagementEntities1();
                    var data = db.Products.ToList();
                    return View(data);
               // }
            }

            // If the conditions are not met, redirect to the login page or take appropriate action
            return RedirectToAction("Index"); // Replace "Login" with your login page action name
        }

        /*    public ActionResult AddToCart(int id,string productName)
            {
                if (Session["ProductBasket"] == null)
                {
                    Session["ProductBasket"] = new List<CartItem>();
                }

                List<CartItem> productBasket = (List<CartItem>)Session["ProductBasket"];

                // Perform validation here to ensure id is a valid product ID

                if (!productBasket.Any(item => item.Id == id))
                {
                    productBasket.Add(new CartItem { Id = id, Name = productName });
                }

                Session["ProductBasket"] = productBasket;

                return RedirectToAction("Deshboard");
            }*/
        //also add quantity

        public ActionResult AddToCart(int id, string productName, int quantity)
        {

            if (Session["ProductBasket"] == null)
            {
                Session["ProductBasket"] = new List<CartItem>();
            }

            List<CartItem> productBasket = (List<CartItem>)Session["ProductBasket"];

            // Perform validation here to ensure id is a valid product ID

            // Check if the product is already in the cart
            var existingProduct = productBasket.FirstOrDefault(item => item.Id == id);
            if (existingProduct != null)
            {
                // If the product is already in the cart, update its quantity
                existingProduct.Quantity += quantity;
            }
            else
            {
                // If the product is not in the cart, add it with the given quantity
                productBasket.Add(new CartItem { Id = id, Name = productName, Quantity = quantity });
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
                // If the cart is empty, you can handle it as needed.
                ViewBag.Message = "Your cart is empty.";
            }
            else
            {
                // Store the list of product IDs in the ViewBag to display in the view
                ViewBag.ProductIdsInCart = cartItems;
            }

            return View("ViewCart");
        }

        /*public ActionResult ConfirmOrder()
        {
            // Retrieve the product details from the session
            var productIdsInCart = Session["ProductBasket"] as List<Product>; // Assuming 'Product' is your model

            if (productIdsInCart != null)
            {
                // Retrieve the customer ID from a cookie
                int customerId;
                if (Request.Cookies["CustomerId"] != null && int.TryParse(Request.Cookies["CustomerId"].Value, out customerId))
                {
                    // Assuming you have a database context, you can save the order details
                    // to the database here, for example, using Entity Framework.
                    using (var context = new ProductManagementEntities1())
                    {
                        foreach (var product in productIdsInCart)
                        {
                            var order = new OrderMapTable
                            {
                                Cusid = customerId,
                                Pid = product.Productid
                                // Set other properties as needed
                            };

                            context.OrderMapTables.Add(order);
                        }

                        context.SaveChanges();
                    }

                    // Remove the session data
                    Session.Remove("ProductBasket");

                    // Redirect to a thank you page or another appropriate action
                    return RedirectToAction("ThankYou");
                }
            }

            return RedirectToAction("Index"); // Handle the case when the cart is empty or customer ID is not found in the cookie
        }*/
      
        [HttpPost]
        public ActionResult ConfirmOrder()
        {
            // Retrieve the product details from the session
            var productIdsInCart = Session["ProductBasket"] as List<CartItem>;

            if (productIdsInCart != null)
            {
                // Retrieve the customer ID from a cookie
                int customerId;
                if (Request.Cookies["CustomerInfo"] != null && int.TryParse(Request.Cookies["CustomerInfo"]["CustomerId"], out customerId))
                {
                    using (var context = new ProductManagementEntities1())
                    {
                        try
                        {
                            foreach (var product in productIdsInCart)
                            {
                                // Validate the product ID before creating an order
                              
                                    var order = new OrderMapTable
                                    {
                                        Pid = product.Id,
                                        Cusid = customerId,
                                        Quantity = product.Quantity



                                    };

                                    context.OrderMapTables.Add(order);
                              
                            }

                            context.SaveChanges(); // Attempt to save changes to the database

                            // Remove the session data
                            Session.Remove("ProductBasket");

                            // Redirect to a thank you page or another appropriate action
                            return RedirectToAction("ThankYouForOrder");
                        }
                        catch (Exception ex)
                        {
                            // Log or debug the exception
                            // This will help you identify any database-related issues
                            // You can use: System.Diagnostics.Debug.WriteLine(ex.Message);
                            // Or your preferred logging method
                        }
                    }
                }
            }

            return RedirectToAction("Index"); // Handle the case when the cart is empty or customer ID is not found in the cookie
        }

       public ActionResult ThankYouForOrder()
        {
            return View();
        }
     








    }
}