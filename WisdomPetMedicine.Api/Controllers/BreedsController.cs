using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WisdomPetMedicine.Api.Data;

namespace WisdomPetMedicine.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class BreedsController(ManagementDbContext dbcontext, ILogger<BreedsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBreeds()
        {
            var breeds = await dbcontext.Breeds.ToListAsync();
            return Ok(breeds);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var breed = await dbcontext.Breeds
                       .FirstOrDefaultAsync(p => p.Id == id);

            if (breed == null)
                return NotFound();

            return Ok(breed);
        }
        [HttpPost]
        public async Task<IActionResult> Create(NewBreed newBreed)
        {
            var breed = newBreed.ToBreed();
            await dbcontext.Breeds.AddAsync(breed);
            await dbcontext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = breed.Id }, breed);
        }
        public record NewBreed(string name)
        {
            public Breed ToBreed() => new Breed(0, name);
        }

    }
}
