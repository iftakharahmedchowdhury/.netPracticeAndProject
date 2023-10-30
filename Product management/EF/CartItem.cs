using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Product_management.EF
{
    /*  public class CartItem
      {
          public int ProductId { get; set; } // Product ID
          public string Name { get; set; } // Product Name
          public int Quantity { get; set; } // Quantity of the product in the cart
      }*/

    public class CartItem
    {
        public int ProductId { get; set; } // Product ID
        public string ProductName { get; set; } // Product Name
        public int ProductQuantity { get; set; } // Quantity of the product in the cart
        public double ProductPrice { get; set; } // Price of the product
        public string Status { get; set; } // Status of the product in the cart
        public int CustomerId { get; set; } // Customer ID
        public DateTime OrderDate { get; set; } // Date of the order
    }

}