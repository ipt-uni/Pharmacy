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
    /// REST API controller for Company CRUD operations.
    /// Routes: api/Company
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all companies mapped to DTOs.
        /// GET: api/Company
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany()
        {
            return await _context.Companies
                .Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single company by id.
        /// Returns 404 if not found.
        /// GET: api/Company/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            var company = await _context.Companies
                .Where(c => c.Id == id)
                .Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .FirstOrDefaultAsync();

            if (company == null)
                return NotFound();

            return company;
        }

        /// <summary>
        /// Updates an existing company.
        /// Returns 400 if the route id doesn't match the body id,
        /// 404 if not found, 204 on success.
        /// PUT: api/Company/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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
        /// Creates a new company and returns the created resource with its new id.
        /// POST: api/Company
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        /// <summary>
        /// Deletes a company by id. Returns 404 if not found, 204 on success.
        /// DELETE: api/Company/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Returns true if a company with the given id exists.
        /// </summary>
        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
