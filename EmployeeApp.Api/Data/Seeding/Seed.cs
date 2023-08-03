using System.Text.Json;
using EmployeeApp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Api.Data.Seeding
{
    public class Seed
    {
        public static async Task SeedCompanies(EmployeeAppDbContext context)
        {
            if (await context.Companies.AnyAsync())
                return;

            var companiesData = await File.ReadAllTextAsync("Data/generated.json");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var companies = JsonSerializer.Deserialize<List<Company>>(companiesData);

            foreach (var company in companies)
            {
                context.Companies.Add(company);
            }

            await context.SaveChangesAsync();
        }
    }
}