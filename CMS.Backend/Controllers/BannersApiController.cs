using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BannersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveBanners()
        {
            var banners = await _context.Banners
                .Where(b => b.IsActive)
                .OrderBy(b => b.DisplayOrder)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.ImageUrl,
                    b.Link
                })
                .ToListAsync();

            return Ok(banners);
        }
    }
}
