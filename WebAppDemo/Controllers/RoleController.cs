using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppDemo.Data;
using WebAppDemo.Models;
using System.Linq;

namespace WebAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _context.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost]
        public IActionResult CreateRole([FromBody] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Add(role);
                _context.SaveChanges();
                return Ok(role);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRole(int id, [FromBody] Role role)
        {
            var existingRole = _context.Roles.Find(id);
            if (existingRole == null)
            {
                return NotFound();
            }

            existingRole.RoleName = role.RoleName;
            existingRole.Description = role.Description;

            _context.Roles.Update(existingRole);
            _context.SaveChanges();
            return Ok(existingRole);
        }

        [HttpPost("assign-role/{customerId}/{roleId}")]
        public IActionResult AssignRole(int customerId, int roleId)
        {
            var customer = _context.Customers.Find(customerId);
            var role = _context.Roles.Find(roleId);

            if (customer == null)
            {
                return NotFound($"Customer with ID {customerId} not found.");
            }

            if (role == null)
            {
                return NotFound($"Role with ID {roleId} not found.");
            }

            customer.RoleID = roleId;
            _context.SaveChanges();

            return Ok(customer);
        }
    }
}
