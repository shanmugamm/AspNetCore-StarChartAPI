using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Clauses;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        // GET
        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
           var celestialObject =  _context.CelestialObjects.Find(id);
            if(celestialObject == null)
                return NotFound();
            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObject = _context.CelestialObjects.Where(e=>e.Name == name).ToList();
            if (!celestialObject.Any())
                return NotFound();
            foreach (var celestial in celestialObject)
            {
                celestial.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestial.Id).ToList();
            }
            return Ok(celestialObject);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObject = _context.CelestialObjects.ToList();
            foreach (var celestial in celestialObject)
            {
                celestial.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestial.Id).ToList();
            }
            return Ok(celestialObject);

        }

    }
}
