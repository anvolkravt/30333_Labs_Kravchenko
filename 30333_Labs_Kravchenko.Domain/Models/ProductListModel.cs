namespace _30333_Labs_Kravchenko.Domain.Models
{
    public class ProductListModel<T>
    {
        public List<T> Items { get; set; } = [];
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
    }
}