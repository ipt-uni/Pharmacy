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
    /// <summary>
    /// REST API controller for Cart CRUD operations.
    /// Routes: api/Cart
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all carts with nested items and payment status.
        /// GET: api/Cart
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetCart()
        {
            // 1. Query all carts with their related customer, items, medicine details, and payment
            var carts = await _context.Carts
                .Include(c => c.Customer)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Medicine)
                .Include(c => c.Payment)
                .ToListAsync();

            // 2. Build response DTOs with computed totals and nested item DTOs
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

        /// <summary>
        /// Retrieves a single cart by id with nested items and payment status.
        /// Returns 404 if not found.
        /// GET: api/Cart/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CartDto>> GetCart(int id)
        {
            // 1. Query the specific cart with related data
            var cart = await _context.Carts
                .Include(c => c.Customer)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Medicine)
                .Include(c => c.Payment)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
                return NotFound();

            // 2. Build response DTO with computed totals and nested item DTOs
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

        /// <summary>
        /// Updates an existing cart.
        /// Returns 400 if the route id doesn't match the body id,
        /// 404 if not found, 204 on success.
        /// PUT: api/Cart/5
        /// </summary>
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

        /// <summary>
        /// Creates a new cart and returns the created resource with its new id.
        /// POST: api/Cart
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
        }

        /// <summary>
        /// Deletes a cart by id. Returns 404 if not found, 204 on success.
        /// DELETE: api/Cart/5
        /// </summary>
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

        /// <summary>
        /// Returns true if a cart with the given id exists.
        /// </summary>
        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
