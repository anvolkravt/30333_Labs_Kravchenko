using _30333_Labs_Kravchenko.Domain.Entities;
using _30333_Labs_Kravchenko.Domain.Models;

namespace _30333_Labs_Kravchenko.UI.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>>
       GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id=1, Name="Витамины", NormalizedName="vitamins"},
                new Category {Id=2, Name="Давление", NormalizedName="bloodpreasure"},
                new Category {Id=3, Name="Болеутоляющие", NormalizedName="painkillers"},
                new Category {Id=4, Name="Антидепрессанты", NormalizedName="antidepressants"},
                new Category {Id=5, Name="Гипогликемические", NormalizedName="hypoglycemic"},
                new Category {Id=6, Name="Противопростудные", NormalizedName="anticold"}
            };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }

}
