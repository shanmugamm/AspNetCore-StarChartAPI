using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Clauses;
using StarChart.Data;
using StarChart.Models;

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
            var celestialObjects = _context.CelestialObjects.Where(e=>e.Name == name).ToList();
            if (!celestialObjects.Any())
                return NotFound();
            foreach (var celestial in celestialObjects)
            {
                celestial.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestial.Id).ToList();
            }
            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var celestial in celestialObjects)
            {
                celestial.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestial.Id).ToList();
            }
            return Ok(celestialObjects);

        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new {id = celestialObject.Id}, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var exisitingObject = _context.CelestialObjects.Find(id);
            if (exisitingObject == null)
                return NotFound();
            exisitingObject.Name = celestialObject.Name;
            exisitingObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            exisitingObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            _context.CelestialObjects.Update(exisitingObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var exisitingObject = _context.CelestialObjects.Find(id);
            if (exisitingObject == null)
                return NotFound();
            exisitingObject.Name = name;
            _context.CelestialObjects.Update(exisitingObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id);
            if (!celestialObjects.Any())
                return NotFound();
            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
