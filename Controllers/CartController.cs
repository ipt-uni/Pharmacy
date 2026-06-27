using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;
using pharmacy.Data.Models.ViewModels;

namespace pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetCart()
        {
            var carts = await _context.Carts
                .Include(c => c.Customer)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Medicine)
                .Include(c => c.Payment)
                .ToListAsync();

            var dtos = carts.Select(c => new CartDto
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                CustomerName = $"{c.Customer.FirstName} {c.Customer.LastName}",
                TotalCost = c.CartItems.Sum(ci => ci.Quantity * ci.UnitPrice),
                TotalQuantity = c.CartItems.Sum(ci => ci.Quantity),
                IsPaid = c.Payment != null,
                Items = c.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    MedicineId = ci.MedicineId,
                    MedicineName = ci.Medicine.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                }).ToList(),
            }).ToList();

            return dtos;
        }

        // GET: api/Cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartDto>> GetCart(int id)
        {
            var cart = await _context.Carts
                .Include(c => c.Customer)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Medicine)
                .Include(c => c.Payment)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
                return NotFound();

            var dto = new CartDto
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                CustomerName = $"{cart.Customer.FirstName} {cart.Customer.LastName}",
                TotalCost = cart.CartItems.Sum(ci => ci.Quantity * ci.UnitPrice),
                TotalQuantity = cart.CartItems.Sum(ci => ci.Quantity),
                IsPaid = cart.Payment != null,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    MedicineId = ci.MedicineId,
                    MedicineName = ci.Medicine.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                }).ToList(),
            };

            return dto;
        }

        // PUT: api/Cart/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
        }

        // DELETE: api/Cart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
