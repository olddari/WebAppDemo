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

           [HttpGet]
           public IActionResult Get(int pageIndex = 0, int pageSize = 10)
           {
               BaseResponseModel response = new BaseResponseModel();

               try
               {
                   var productCount = _context.Products.Count();
                   var productList = _context.Products.Include(x => x.ProductName).Skip(pageIndex * pageSize).Take(pageSize).ToList();

                   response.Status = true;
                   response.Message = "Success";
                   response.Data = new { Products = productList, Count = productCount };

                   return Ok(response);
               }
               catch (System.Exception ex)
               {
                   response.Status = false;
                   response.Message = "Not Succed";

                   return BadRequest(response);

                   throw;
               }
           }


           
        
    }
}
