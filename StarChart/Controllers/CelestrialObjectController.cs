using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestrialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestrialObjectController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        // GET
       
    }
}
