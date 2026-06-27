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
    /// REST API controller for Payment CRUD operations.
    /// Routes: api/Payment
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all payments mapped to DTOs.
        /// GET: api/Payment
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayment()
        {
            return await _context.Payments
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    CartId = p.CartId,
                    Amount = p.Amount,
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single payment by id.
        /// Returns 404 if not found.
        /// GET: api/Payment/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPayment(int id)
        {
            var payment = await _context.Payments
                .Where(p => p.Id == id)
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    CartId = p.CartId,
                    Amount = p.Amount,
                })
                .FirstOrDefaultAsync();

            if (payment == null)
                return NotFound();

            return payment;
        }

        /// <summary>
        /// Updates an existing payment.
        /// Returns 400 if the route id doesn't match the body id,
        /// 404 if not found, 204 on success.
        /// PUT: api/Payment/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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
        /// Creates a new payment and returns the created resource with its new id.
        /// POST: api/Payment
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
        }

        /// <summary>
        /// Deletes a payment by id. Returns 404 if not found, 204 on success.
        /// DELETE: api/Payment/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Returns true if a payment with the given id exists.
        /// </summary>
        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
