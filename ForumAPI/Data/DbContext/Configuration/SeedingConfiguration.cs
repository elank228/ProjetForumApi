using ForumAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace ForumApi.DbContexts.Configuration
{
    public class SeedingConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        string JsonPath;

        public SeedingConfiguration(string jsonPath)
        {
            JsonPath = jsonPath;
        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            string json = File.ReadAllText(JsonPath);
            List<T> entities = JsonSerializer.Deserialize<List<T>>(json);

            foreach (var entity in entities)
            {
                entity.CreationDateTime = new DateTime(2022, 01, 01);
                entity.ModificationDateTime = new DateTime(2022, 01, 01);
                entity.UserCreated = "SYSTEM";
                entity.UserModified = "SYSTEM";
            }

            builder.HasData(entities);
        }
    }
}
