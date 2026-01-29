using Lab_3_Backend.Data;
using Lab_3_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrucksController : ControllerBase
    {
        private readonly CarDbContext _context;

        public TrucksController(CarDbContext context)
        {
            _context = context;
        }

        // GET: api/Trucks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Truck>>> GetTrucks()
        {
            return await _context.Trucks.ToListAsync();
        }

        // GET: api/Trucks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Truck>> GetTruck(int id)
        {
            var truck = await _context.Trucks.FindAsync(id);

            if (truck == null)
            {
                return NotFound();
            }

            return truck;
        }

        // PUT: api/Trucks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTruck(int id, Truck truck)
        {
            if (id != truck.Id)
            {
                return BadRequest();
            }

            _context.Entry(truck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TruckExists(id))
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

        // POST: api/Trucks
        [HttpPost]
        public async Task<ActionResult<Truck>> PostTruck(Truck truck)
        {
            _context.Trucks.Add(truck);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTruck", new { id = truck.Id }, truck);
        }

        // DELETE: api/Trucks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruck(int id)
        {
            var truck = await _context.Trucks.FindAsync(id);
            if (truck == null)
            {
                return NotFound();
            }

            _context.Trucks.Remove(truck);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Trucks/brand/{brand}
        [HttpGet("brand/{brand}")]
        public async Task<ActionResult<IEnumerable<Truck>>> GetTrucksByBrand(string brand)
        {
            return await _context.Trucks
                .Where(t => t.Brand == brand)
                .ToListAsync();
        }

        private bool TruckExists(int id)
        {
            return _context.Trucks.Any(e => e.Id == id);
        }
    }
}