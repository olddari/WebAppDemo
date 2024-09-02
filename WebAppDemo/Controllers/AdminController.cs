using Microsoft.AspNetCore.Mvc;
using WebAppDemo.Data;
using WebAppDemo.Models;
using System.Linq;

namespace WebAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("isAdmin/{customerId}")]
        public IActionResult IsAdmin(int customerId)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.CustomerID == customerId && c.Role.RoleName == "Admin");

            if (customer != null)
            {
                return Ok(new { isAdmin = true });
            }
            else
            {
                return Ok(new { isAdmin = false });
            }
        }


        [HttpPost("assign-admin/{customerId}")]
        public IActionResult AssignAdminRole(int customerId)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer == null)
            {
                return NotFound("Müşteri bulunamadı.");
            }

            var adminRole = _context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
            if (adminRole == null)
            {
                return BadRequest("Admin rolü bulunamadı.");
            }

            customer.RoleID = adminRole.RoleID;

            _context.SaveChanges();

            return Ok(customer);
        }
    }
}
