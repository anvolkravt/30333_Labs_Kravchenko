using _30333_Labs_Kravchenko.Domain.Entities;
using _30333_Labs_Kravchenko.Domain.Models;

namespace _30333_Labs_Kravchenko.UI.Services
{
    public class MemoryProductService : IProductService
    {
        List<Medication> _medications;
        List<Category> _categories;
        public MemoryProductService(ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync()
           .Result
           .Data;
            SetupData();
        }
        private void SetupData()
        {
            _medications = new List<Medication>
            {
                new Medication {
                    Id = 1,
                    Name="D3",
                    Description="Витамин Д3 2000 МЕ капсулы 700мг №30",
                    Manufacturer ="ООО Полярис",
                    Image="Images/D3.jpg",
                    CategoryId= _categories.Find(c=>c.NormalizedName.Equals("vitamins")).Id},
                new Medication {
                    Id = 2,
                    Name="Алотендин",
                    Description="Алотендин 10 мг+10 мг таблетки 30 шт",
                    Manufacturer ="ЭГИС ЗАО",
                    Image="Images/Алотендин.jpg",
                    CategoryId= _categories.Find(c=>c.NormalizedName.Equals("bloodpreasure")).Id},
                new Medication {
                    Id = 3,
                    Name="Аспирин",
                    Description="Аспирин Кардио 100 мг таблетки кишечнорастворимые 28 шт",
                    Manufacturer ="Байер АГ",
                    Image="Images/Аспирин.jpg",
                    CategoryId= _categories.Find(c=>c.NormalizedName.Equals("painkillers")).Id},
                new Medication {
                    Id = 4,
                    Name="Бринтелликс",
                    Description="Бринтелликс 20 мг таблетки покрытые пленочной оболочкой 28 шт",
                    Manufacturer ="Х. Лундбек А/О",
                    Image="Images/Бринтелликс.jpg",
                    CategoryId= _categories.Find(c=>c.NormalizedName.Equals("antidepressants")).Id},
                new Medication {
                    Id = 5,
                    Name="Вилдаглиптин",
                    Description="Вилдаглиптин-АМ 50 мг таблетки 30 шт",
                    Manufacturer ="АмантисМед ООО",
                    Image="Images/Вилдаглиптин.jpg",
                    CategoryId= _categories.Find(c=>c.NormalizedName.Equals("hypoglycemic")).Id},
                new Medication {
                    Id = 6,
                    Name="Лирика",
                    Description="Лирика 75 мг капсулы 14 шт",
                    Manufacturer ="Пфайзер ГмбХ",
                    Image="Images/Лирика.jpg",
                    CategoryId= _categories.Find(c=>c.NormalizedName.Equals("antidepressants")).Id},
                new Medication {
                    Id = 7,
                    Name="Лозартан",
                    Description="Лозартан-ЛФ 50 мг таблетки покрытые пленочной оболочкой 30 шт",
                    Manufacturer ="Лекфарм СООО",
                    Image="Images/Лозартан.jpg",
                    CategoryId= _categories.Find(c=>c.NormalizedName.Equals("bloodpreasure")).Id},
                new Medication {
                    Id = 8,
                    Name="Омега-3",
                    Description="Omega 3 от NOW (200 капс)",
                    Manufacturer ="Now Foods",
                    Image="Images/Омега.jpg",
                    CategoryId= _categories.Find(c=>c.NormalizedName.Equals("vitamins")).Id},
                new Medication {
                    Id = 9,
                    Name="Тамифлю",
                    Description="Тамифлю 75 мг капсулы 10 шт",
                    Manufacturer ="F.Hoffmann-La Roche Ltd",
                    Image="Images/Тамифлю.jpg",
                    CategoryId= _categories.Find(c=>c.NormalizedName.Equals("anticold")).Id}
            };
        }

        public Task<ResponseData<ProductListModel<Medication>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var data = _medications
                .Where(m => categoryNormalizedName == null ||
                            _categories.Any(c => c.NormalizedName.Equals(categoryNormalizedName, StringComparison.OrdinalIgnoreCase) && c.Id == m.CategoryId))
                .ToList();

            var model = new ProductListModel<Medication> { Items = data };
            var result = new ResponseData<ProductListModel<Medication>> { Data = model };

            if (!data.Any() && categoryNormalizedName != null)
            {
                result.Success = false;
                result.ErrorMessage = $"Нет лекарств в категории '{categoryNormalizedName}'";
            }

            return Task.FromResult(result);
        }

        public Task<ResponseData<Medication>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Medication product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Medication>> CreateProductAsync(Medication product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
