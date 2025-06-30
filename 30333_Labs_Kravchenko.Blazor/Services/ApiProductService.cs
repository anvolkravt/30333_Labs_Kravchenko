using _30333_Labs_Kravchenko.Domain.Entities;
using _30333_Labs_Kravchenko.Domain.Models;
using System.Diagnostics;

namespace _30333_Labs_Kravchenko.Blazor.Services
{
    public class ApiProductService(HttpClient Http) : IProductService<Medication>
    {
        private List<Medication> _medications = new();
        private int _currentPage = 1;
        private int _totalPages = 1;

        public IEnumerable<Medication> Products => _medications;
        public int CurrentPage => _currentPage;
        public int TotalPages => _totalPages;
        public event Action ListChanged;

        public async Task GetProducts(int pageNo = 1, int pageSize = 3)
        {
            try
            {
                var uri = Http.BaseAddress?.AbsoluteUri.TrimEnd('/') + "/";
                var queryData = new Dictionary<string, string>
                {
                    { "pageNo", pageNo.ToString() },
                    { "pageSize", pageSize.ToString() }
                };
                var query = QueryString.Create(queryData);
                var fullUri = uri + query.Value.TrimStart('/');
                Debug.WriteLine($"Requesting: {fullUri}");

                var result = await Http.GetAsync(fullUri);
                Debug.WriteLine($"Response: {result.StatusCode}");

                if (result.IsSuccessStatusCode)
                {
                    var responseData = await result.Content.ReadFromJsonAsync<ResponseData<ProductListModel<Medication>>>();
                    if (responseData?.Success == true && responseData.Data != null)
                    {
                        _medications = responseData.Data.Items?.ToList() ?? new List<Medication>();
                        _currentPage = responseData.Data.CurrentPage;
                        _totalPages = responseData.Data.TotalPages;
                        Debug.WriteLine($"Received {_medications.Count} items, page {_currentPage}/{_totalPages}");
                        foreach (var med in _medications)
                        {
                            Debug.WriteLine($"Medication: {med.Name}, ID: {med.Id}");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Response data is null or invalid");
                        _medications = new List<Medication>();
                    }
                }
                else
                {
                    var error = await result.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error: {error}");
                    _medications = new List<Medication>();
                    _currentPage = 1;
                    _totalPages = 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in GetProducts: {ex.Message}");
                _medications = new List<Medication>();
                _currentPage = 1;
                _totalPages = 1;
            }
            ListChanged?.Invoke();
        }
    }
}