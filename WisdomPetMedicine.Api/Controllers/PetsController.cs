using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WisdomPetMedicine.Api.Data;

namespace WisdomPetMedicine.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PetsController(ManagementDbContext dbcontext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPets()
        {
            var pets = await dbcontext.Pets.Include(b => b.Breed).ToListAsync();
            return Ok(pets);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pet = await dbcontext.Pets
                .Include(b => b.Breed)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
                return NotFound();

            return Ok(pet);
        }
        [HttpPost]
        public async Task<IActionResult> Create(NewPet newPet)
        {
            var pet = newPet.ToPet();
            await dbcontext.Pets.AddAsync(pet);
            await dbcontext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
        }
        public record NewPet(string name, int age, int breedId)
        {
            public Pet ToPet() => new Pet
            {
                Name = name,
                Age = age,
                BreedId = breedId
            };
        }

    }
}
