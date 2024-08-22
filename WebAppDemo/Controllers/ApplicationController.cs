using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public ApplicationController(ApplicationDbContext context)
        {
            _context = context;
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

        // PUT: api/application/products/5
        [HttpPut("products/{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest("Product ID mismatch.");
            }

            _context.Entry(product).State = EntityState.Modified;

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
    }
}
