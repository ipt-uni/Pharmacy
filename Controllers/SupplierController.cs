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
    /// REST API controller for Supplier CRUD operations.
    /// Routes: api/Supplier
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all suppliers mapped to DTOs.
        /// GET: api/Supplier
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSupplier()
        {
            return await _context.Suppliers
                .Select(s => new SupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single supplier by id.
        /// Returns 404 if not found.
        /// GET: api/Supplier/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers
                .Where(s => s.Id == id)
                .Select(s => new SupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                })
                .FirstOrDefaultAsync();

            if (supplier == null)
                return NotFound();

            return supplier;
        }

        /// <summary>
        /// Updates an existing supplier.
        /// Returns 400 if the route id doesn't match the body id,
        /// 404 if not found, 204 on success.
        /// PUT: api/Supplier/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return BadRequest();
            }

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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
        /// Creates a new supplier and returns the created resource with its new id.
        /// POST: api/Supplier
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSupplier", new { id = supplier.Id }, supplier);
        }

        /// <summary>
        /// Deletes a supplier by id. Returns 404 if not found, 204 on success.
        /// DELETE: api/Supplier/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Returns true if a supplier with the given id exists.
        /// </summary>
        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
