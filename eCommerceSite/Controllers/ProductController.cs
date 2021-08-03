﻿using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class ProductController : Controller
    {
        // Construction injection
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a view that lists all products
        /// </summary>
        public IActionResult Index()
        {
            // Get all products from the database
            List<Product> products = _context.Products.ToList();

            // Send list of products to view to be displayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product p)
        {
            if (ModelState.IsValid)
            {
                // Add to DB
                _context.Products.Add(p);
                _context.SaveChanges();

                TempData["Message"] = $"{p.ProductId}:{p.Title} was added successfully";

                // redirect back to catalog page
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
