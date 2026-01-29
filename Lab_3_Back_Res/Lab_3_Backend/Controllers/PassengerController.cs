using Lab_3_Backend.Data;
using Lab_3_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassengersController : ControllerBase
    {
        private readonly CarDbContext _context;

        public PassengersController(CarDbContext context)
        {
            _context = context;
        }

        // GET: api/Passengers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Passenger>>> GetPassengers()
        {
            return await _context.Passengers.ToListAsync();
        }

        // GET: api/Passengers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Passenger>> GetPassenger(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);

            if (passenger == null)
            {
                return NotFound();
            }

            return passenger;
        }

        // PUT: api/Passengers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPassenger(int id, Passenger passenger)
        {
            if (id != passenger.Id)
            {
                return BadRequest();
            }

            _context.Entry(passenger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(id))
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

        // POST: api/Passengers
        [HttpPost]
        public async Task<ActionResult<Passenger>> PostPassenger(Passenger passenger)
        {
            _context.Passengers.Add(passenger);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPassenger", new { id = passenger.Id }, passenger);
        }

        // DELETE: api/Passengers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassenger(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }

            _context.Passengers.Remove(passenger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Passengers/brand/{brand}
        [HttpGet("brand/{brand}")]
        public async Task<ActionResult<IEnumerable<Passenger>>> GetPassengersByBrand(string brand)
        {
            return await _context.Passengers
                .Where(p => p.Brand == brand)
                .ToListAsync();
        }

        private bool PassengerExists(int id)
        {
            return _context.Passengers.Any(e => e.Id == id);
        }
    }
}