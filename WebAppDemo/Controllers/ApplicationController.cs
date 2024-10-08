﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAppDemo.Data;
using WebAppDemo.Models;

namespace WebAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ApplicationController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/application/products
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products
                                         .Include(p => p.ProductCategory)
                                         .Include(p => p.ProductAttributes)
                                         .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }

        // GET: api/application/products/5
        [HttpGet("products/{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products
                                        .Include(p => p.ProductCategory)
                                        .Include(p => p.ProductAttributes)
                                        .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        // POST: api/application/products
        [HttpPost("products")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductID }, product);
        }

        [HttpPut("products/{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            if (product.CategoryID != 0 && existingProduct.CategoryID != product.CategoryID)
            {
                existingProduct.CategoryID = product.CategoryID;
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.Brand = product.Brand;
            existingProduct.Model = product.Model;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.Description = product.Description;
            existingProduct.ImageURL = product.ImageURL;

            _context.Entry(existingProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.ProductID == id))
                {
                    return NotFound("Product not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/application/products/5
        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product deleted successfully.");
        }

        // POST: api/application/products/{id}/brand
        [HttpPost("products/{id}/brand")]
        public async Task<IActionResult> AddBrand(int id, [FromBody] string brand)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            product.Brand = brand; 
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/application/products/{id}/brand
        [HttpDelete("products/{id}/brand")]
        public async Task<IActionResult> RemoveBrand(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            product.Brand = null; 
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/application/products/search
        [HttpGet("products/search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term is required.");
            }

            var products = await _context.Products
                .Where(p => p.ProductName.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductAttributes)
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No products found matching the search term.");
            }

            return Ok(products);
        }

        [HttpPost("products/{id}/upload-image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile image)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Directory.CreateDirectory(uploadsFolder);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var imageURL = $"/uploads/{uniqueFileName}";

            product.ImageURL = imageURL;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { imageURL });
        }
    }
}
