using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    /// <summary>
    /// Returns the current list of cart products. If cart is empty
    /// an empty list will be returned.
    /// </summary>
    /// <returns>An empty list if cart is empty</returns>
    public static class CookieHelper
    {
        const string CartCookie = "CartCookie"; // to access the key

        // Get the cart products
        public static List<Product> GetCartProducts(IHttpContextAccessor http)        {
            

            //Get existing cart items (get cookie data)
            string existingItems = http.HttpContext.Request.Cookies[CartCookie];

            // Create a list
            List<Product> cartProducts = new List<Product>();
            if (existingItems != null) // if there is no cookie
            {
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(existingItems);
            }

            return cartProducts;
        }

        public static void AddProductToCart(IHttpContextAccessor http, Product p)
        {
            List<Product> cartProducts = GetCartProducts(http);
            cartProducts.Add(p);

            string data = JsonConvert.SerializeObject(cartProducts);

            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddYears(1),
                Secure = true,
                IsEssential = true
            };

            http.HttpContext.Response.Cookies.Append(CartCookie, data, options);

        }

        public static int GetTotalCartProducts(IHttpContextAccessor http)
        {
            throw new NotImplementedException();
        }
    }
}
