using _30333_Labs_Kravchenko.Domain.Entities;
using _30333_Labs_Kravchenko.Domain.Models;

namespace _30333_Labs_Kravchenko.UI.Services
{
    public interface IProductService
    {
        public Task<ResponseData<ProductListModel<Medication>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
        public Task<ResponseData<Medication>> GetProductByIdAsync(int id);
        public Task UpdateProductAsync(int id, Medication product, IFormFile? formFile);
        public Task DeleteProductAsync(int id);
        public Task<ResponseData<Medication>> CreateProductAsync(Medication product, IFormFile? formFile);
    }
}
