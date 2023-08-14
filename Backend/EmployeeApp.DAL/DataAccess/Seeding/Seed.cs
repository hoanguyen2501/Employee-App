using EmployeeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeApp.DAL.DataAccess.Seeding
{
    public class Seed
    {
        public static async Task SeedCompanies(EmployeeAppDbContext context)
        {
            if (await context.Companies.AnyAsync())
                return;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            var companiesData = await File.ReadAllTextAsync($"{basePath}/DataAccess/generated-company-data.json");

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            var companies = JsonSerializer.Deserialize<List<Company>>(companiesData, options);

            foreach (var company in companies)
            {
                context.Companies.Add(company);
            }

            await context.SaveChangesAsync();
        }
    }
}