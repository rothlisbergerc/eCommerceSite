﻿using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public CartController(ProductContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }
        /// <summary>
        /// Adds a product to the shopping cart
        /// </summary>
        /// <param name="id">The id of the product to add</param>
        /// <returns></returns>
        public async Task<IActionResult> Add(int id)
        {
            // Get product from the database
            Product p = await ProductDB.GetProductAsync(_context, id);

            const string CartCookie = "CartCookie";

            //Get existing cart items
            string existingItems =
                _httpContext.HttpContext.Request.Cookies[CartCookie];

            List<Product> cartProducts = new List<Product>();
            if(existingItems != null) // if there is no cookie
            {
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(existingItems);
            }

            // Add current product to existing cart
            cartProducts.Add(p);

            // Add product list to cart cookie
            string data = JsonConvert.SerializeObject(cartProducts); // converts the list of products into a string

            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddYears(1),
                Secure = true,
                IsEssential = true
            };

            // Create the cookie
            _httpContext.HttpContext.Response.Cookies.Append(CartCookie, data, options);

            // Redirect back to previous page
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Summary()
        {
            // Display all products in shopping cart cookie
            string cookieData = _httpContext.HttpContext.Request.Cookies["CartCookie"];

            List<Product> cartProducts = JsonConvert.DeserializeObject<List<Product>>(cookieData);

            return View(cartProducts);
        }
    }
}
