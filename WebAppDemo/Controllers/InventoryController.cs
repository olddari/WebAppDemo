using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppDemo.Data;

namespace WebAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories()
        {
            var inventories = await _context.Inventories
                .Include(i => i.Product) // Include related Product
                .ToListAsync();

            return Ok(inventories);
        }

        // GET: api/Inventory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Product) // Include related Product
                .FirstOrDefaultAsync(i => i.InventoryID == id);

            if (inventory == null)
            {
                return NotFound("Inventory item not found.");
            }

            return Ok(inventory);
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<ActionResult<Inventory>> AddInventory(Inventory inventory)
        {
            if (inventory == null)
            {
                return BadRequest("Invalid inventory data.");
            }

 
            var productExists = await _context.Products.AnyAsync(p => p.ProductID == inventory.ProductID);
            if (!productExists)
            {
                return BadRequest("Associated product does not exist.");
            }

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventory), new { id = inventory.InventoryID }, inventory);
        }

        // PUT: api/Inventory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, Inventory inventory)
        {
            if (id != inventory.InventoryID)
            {
                return BadRequest("Inventory ID mismatch.");
            }

     
            var productExists = await _context.Products.AnyAsync(p => p.ProductID == inventory.ProductID);
            if (!productExists)
            {
                return BadRequest("Associated product does not exist.");
            }

            _context.Entry(inventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
                {
                    return NotFound("Inventory item not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound("Inventory item not found.");
            }

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            return Ok("Inventory item deleted successfully.");
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.InventoryID == id);
        }
    }
}
