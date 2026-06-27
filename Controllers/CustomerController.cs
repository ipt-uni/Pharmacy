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
    /// REST API controller for Customer CRUD operations.
    /// Routes: api/Customer
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all customers mapped to DTOs.
        /// GET: api/Customer
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomer()
        {
            return await _context.Customers
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Age = c.Age,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single customer by id.
        /// Returns 404 if not found.
        /// GET: api/Customer/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .Where(c => c.Id == id)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Age = c.Age,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                })
                .FirstOrDefaultAsync();

            if (customer == null)
                return NotFound();

            return customer;
        }

        /// <summary>
        /// Updates an existing customer.
        /// Returns 400 if the route id doesn't match the body id,
        /// 404 if not found, 204 on success.
        /// PUT: api/Customer/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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
        /// Creates a new customer and returns the created resource with its new id.
        /// POST: api/Customer
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        /// <summary>
        /// Deletes a customer by id. Returns 404 if not found, 204 on success.
        /// DELETE: api/Customer/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Returns true if a customer with the given id exists.
        /// </summary>
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
