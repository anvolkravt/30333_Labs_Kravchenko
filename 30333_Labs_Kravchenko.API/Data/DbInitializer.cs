using Microsoft.EntityFrameworkCore;
using _30333_Labs_Kravchenko.Domain.Entities;

namespace _30333_Labs_Kravchenko.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            var uri = "https://localhost:7002/";
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();

            if (!context.Categories.Any() && !context.Medications.Any())
            {
                var categories = new Category[]
                {
                    new() { Name = "Витамины", NormalizedName = "vitamins" },
                    new() { Name = "Антигипертензивные", NormalizedName = "bloodpreasure" },
                    new() { Name = "Обезболивающие", NormalizedName = "painkillers" },
                    new() { Name = "Антидепрессанты", NormalizedName = "antidepressants" },
                    new() { Name = "Гипогликемические", NormalizedName = "hypoglycemic" },
                    new() { Name = "Противопростудные", NormalizedName = "anticold" }
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();

                var medications = new List<Medication>
                {
                    new()
                    {
                        Name = "D3",
                        Description = "Витамин Д3 2000 МЕ капсулы 700мг №30",
                        Manufacturer = "ООО Полярис",
                        Image = uri + "Images/D3.jpg",
                        Category = categories.FirstOrDefault(c => c.NormalizedName == "vitamins")
                    },
                    new()
                    {
                        Name = "Алотендин",
                        Description = "Алотендин 10 мг+10 мг таблетки 30 шт",
                        Manufacturer = "ЭГИС ЗАО",
                        Image = uri + "Images/Алотендин.jpg",
                        Category = categories.FirstOrDefault(c => c.NormalizedName == "bloodpreasure")
                    },
                    new()
                    {
                        Name = "Аспирин",
                        Description = "Аспирин Кардио 100 мг таблетки кишечнорастворимые 28 шт",
                        Manufacturer = "Байер АГ",
                        Image = uri + "Images/Аспирин.jpg",
                        Category = categories.FirstOrDefault(c => c.NormalizedName == "painkillers")
                    },
                    new()
                    {
                        Name = "Бринтелликс",
                        Description = "Бринтелликс 20 мг таблетки покрытые пленочной оболочкой 28 шт",
                        Manufacturer = "Х. Лундбек А/О",
                        Image = uri + "Images/Бринтелликс.jpg",
                        Category = categories.FirstOrDefault(c => c.NormalizedName == "antidepressants")
                    },
                    new()
                    {
                        Name = "Вилдаглиптин",
                        Description = "Вилдаглиптин-АМ 50 мг таблетки 30 шт",
                        Manufacturer = "АмантисМед ООО",
                        Image = uri + "Images/Вилдаглиптин.jpg",
                        Category = categories.FirstOrDefault(c => c.NormalizedName == "hypoglycemic")
                    },
                    new()
                    {
                        Name = "Лирика",
                        Description = "Лирика 75 мг капсулы 14 шт",
                        Manufacturer = "Пфайзер ГмбХ",
                        Image = uri + "Images/Лирика.jpg",
                        Category = categories.FirstOrDefault(c => c.NormalizedName == "antidepressants")
                    },
                    new()
                    {
                        Name = "Лозартан",
                        Description = "Лозартан-ЛФ 50 мг таблетки покрытые пленочной оболочкой 30 шт",
                        Manufacturer = "Лекфарм СООО",
                        Image = uri + "Images/Лозартан.jpg",
                        Category = categories.FirstOrDefault(c => c.NormalizedName == "bloodpreasure")
                    },
                    new()
                    {
                        Name = "Омега-3",
                        Description = "Omega 3 от NOW (200 капс)",
                        Manufacturer = "Now Foods",
                        Image = uri + "Images/Омега.jpg",
                        Category = categories.FirstOrDefault(c => c.NormalizedName == "vitamins")
                    },
                    new()
                    {
                        Name = "Тамифлю",
                        Description = "Тамифлю 75 мг капсулы 10 шт",
                        Manufacturer = "F.Hoffmann-La Roche Ltd",
                        Image = uri + "Images/Тамифлю.jpg",
                        Category = categories.FirstOrDefault(c => c.NormalizedName == "anticold")
                    }
                };
                await context.Medications.AddRangeAsync(medications);
                await context.SaveChangesAsync();
            }
        }
    }
}