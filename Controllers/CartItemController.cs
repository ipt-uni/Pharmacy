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
    /// REST API controller for CartItem CRUD operations.
    /// Routes: api/CartItem
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all cart items with their medicine names.
        /// GET: api/CartItem
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItem()
        {
            return await _context.CartItems
                .Include(ci => ci.Medicine)
                .Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    MedicineId = ci.MedicineId,
                    MedicineName = ci.Medicine.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single cart item by id.
        /// Returns 404 if not found.
        /// GET: api/CartItem/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItemDto>> GetCartItem(int id)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Medicine)
                .Where(ci => ci.Id == id)
                .Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    MedicineId = ci.MedicineId,
                    MedicineName = ci.Medicine.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                })
                .FirstOrDefaultAsync();

            if (cartItem == null)
                return NotFound();

            return cartItem;
        }

        /// <summary>
        /// Updates an existing cart item.
        /// Returns 400 if the route id doesn't match the body id,
        /// 404 if not found, 204 on success.
        /// PUT: api/CartItem/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemExists(id))
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
        /// Creates a new cart item and returns the created resource with its new id.
        /// POST: api/CartItem
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItem(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartItem", new { id = cartItem.Id }, cartItem);
        }

        /// <summary>
        /// Deletes a cart item by id. Returns 404 if not found, 204 on success.
        /// DELETE: api/CartItem/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Returns true if a cart item with the given id exists.
        /// </summary>
        private bool CartItemExists(int id)
        {
            return _context.CartItems.Any(e => e.Id == id);
        }
    }
}
