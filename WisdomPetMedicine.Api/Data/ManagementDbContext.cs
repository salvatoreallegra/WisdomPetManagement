using Microsoft.EntityFrameworkCore;

namespace WisdomPetMedicine.Api.Data
{
    public class ManagementDbContext(DbContextOptions<ManagementDbContext> options)
        : DbContext(options)
    {
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Breed> Breeds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Breed>().HasData(
                new Breed(1, "Golden Retriever"),
                new Breed(2, "Labrador"),
                new Breed(3, "German Shepherd")
            );
            modelBuilder.Entity<Pet>().HasData( [
                new Pet { Id = 1, Name = "Buddy", Age = 3, BreedId = 1 },
                new Pet { Id = 2, Name = "Max", Age = 5, BreedId = 2 },
                new Pet { Id = 3, Name = "Bella", Age = 2, BreedId = 3 }
                ]
            );

        }
    }

    public static class EnsureDbIsCreatedExtension
    {
        public static void EnsureDbIsCreated(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ManagementDbContext>();
            dbContext.Database.EnsureCreated();
        }
    }

    public class Pet
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public int BreedId { get; set; }
        public Breed? Breed { get; set; }
    }

    public record Breed(int Id,string Name);

}
