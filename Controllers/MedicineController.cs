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
    /// REST API controller for Medicine CRUD operations.
    /// Routes: api/Medicine
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicineController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all medicines with their company and supplier details.
        /// GET: api/Medicine
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineDto>>> GetMedicine()
        {
            // Query all medicines including related Company and Suppliers,
            // then project to flattened MedicineDto
            return await _context.Medicines
                .Include(m => m.Company)
                .Include(m => m.Suppliers)
                .Select(m => new MedicineDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    RetailPrice = m.RetailPrice,
                    ImageSrc = m.imageSrc,
                    CompanyName = m.Company.Name,
                    Suppliers = m.Suppliers.Select(s => s.Name).ToList()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single medicine by id with company and supplier details.
        /// Returns 404 if not found.
        /// GET: api/Medicine/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineDto>> GetMedicine(int id)
        {
            var medicine = await _context.Medicines
                .Include(m => m.Company)
                .Include(m => m.Suppliers)
                .Where(m => m.Id == id)
                .Select(m => new MedicineDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    RetailPrice = m.RetailPrice,
                    ImageSrc = m.imageSrc,
                    CompanyName = m.Company.Name,
                    Suppliers = m.Suppliers.Select(s => s.Name).ToList()
                })
                .FirstOrDefaultAsync();

            if (medicine == null)
                return NotFound();

            return medicine;
        }

        /// <summary>
        /// Updates an existing medicine.
        /// Returns 400 if the route id doesn't match the body id,
        /// 404 if not found, 204 on success.
        /// PUT: api/Medicine/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicine(int id, Medicine medicine)
        {
            if (id != medicine.Id)
            {
                return BadRequest();
            }

            _context.Entry(medicine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(id))
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
        /// Creates a new medicine and returns the created resource with its new id.
        /// POST: api/Medicine
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Medicine>> PostMedicine(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicine", new { id = medicine.Id }, medicine);
        }

        /// <summary>
        /// Deletes a medicine by id. Returns 404 if not found, 204 on success.
        /// DELETE: api/Medicine/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Returns true if a medicine with the given id exists.
        /// </summary>
        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}
