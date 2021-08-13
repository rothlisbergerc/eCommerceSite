using eCommerceSite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Data
{
    public static class ProductDB
    {
        /// <summary>
        /// Returns the total count of products
        /// </summary>
        /// <param name="_context">Database context to use</param>
        public static async Task<int> GetTotalProductsAsync(ProductContext _context)
        {
            return await (from p in _context.Products
                        select p).CountAsync(); 
        }

        /// <summary>
        /// Get a page worth of products
        /// </summary>
        /// <param name="_context">Database context to use</param>
        /// <param name="pageSize">Number of products per page</param>
        /// <param name="pageNum">Page of products to return</param>
        public static async Task<List<Product>> GetProductAsync(ProductContext _context, int pageSize, int pageNum)
        {
            return await (from p in _context.Products
                       orderby p.Title ascending
                       select p)
                    .Skip(pageSize * (pageNum - 1)) // Skip() must be before Take()
                    .Take(pageSize)
                    .ToListAsync();
        }

        public static async Task<Product> AddProductAsync(ProductContext _context, Product p)
        {
            // Add to DB
            _context.Products.Add(p);
            await _context.SaveChangesAsync();
            return p;
        }
    }
}
