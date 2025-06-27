//using _30333_Labs_Kravchenko.Domain.Entities;
//using System.Text.Json;
//using _30333_Labs_Kravchenko.Domain.Models;

//namespace _30333_Labs_Kravchenko.UI.Services
//{
//    public class ApiProductService : IProductService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly ILogger<ApiProductService> _logger;

//        public ApiProductService(HttpClient httpClient, ILogger<ApiProductService> logger)
//        {
//            _httpClient = httpClient;
//            _logger = logger;
//            _httpClient.BaseAddress = new Uri("https://localhost:7002/api/medications");
//        }

//        public async Task<ResponseData<ProductListModel<Medication>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
//        {
//            try
//            {
//                var queryData = new Dictionary<string, string>
//                {
//                    { "pageNo", pageNo.ToString() }
//                };

//                if (!string.IsNullOrEmpty(categoryNormalizedName))
//                {
//                    queryData.Add("category", categoryNormalizedName);
//                }

//                var query = QueryString.Create(queryData);
//                _logger.LogInformation("Requesting medications from {BaseAddress}{Query}", _httpClient.BaseAddress, query.Value);

//                var response = await _httpClient.GetAsync(query.Value);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = await response.Content.ReadFromJsonAsync<ResponseData<ProductListModel<Medication>>>();
//                    if (result == null)
//                    {
//                        _logger.LogWarning("No data returned from medications API");
//                        return new ResponseData<ProductListModel<Medication>> { Success = false, ErrorMessage = "No data returned" };
//                    }
//                    _logger.LogInformation("Successfully retrieved {Count} medications for category: {Category}, page: {Page}",
//                        result.Data?.Items.Count() ?? 0, categoryNormalizedName ?? "all", pageNo);
//                    return result;
//                }

//                _logger.LogError("Failed to retrieve medications: {StatusCode}", response.StatusCode);
//                return new ResponseData<ProductListModel<Medication>>
//                {
//                    Success = false,
//                    ErrorMessage = $"Ошибка чтения API: {response.StatusCode}"
//                };
//            }
//            catch (HttpRequestException ex)
//            {
//                _logger.LogError(ex, "HTTP error retrieving medications: {Message}", ex.Message);
//                return new ResponseData<ProductListModel<Medication>> { Success = false, ErrorMessage = $"HTTP error: {ex.Message}" };
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving medications: {Message}", ex.Message);
//                return new ResponseData<ProductListModel<Medication>> { Success = false, ErrorMessage = ex.Message };
//            }
//        }

//        public async Task<ResponseData<Medication>> CreateProductAsync(Medication product, IFormFile? formFile)
//        {
//            try
//            {
//                var serializerOptions = new JsonSerializerOptions
//                {
//                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//                };

//                _logger.LogInformation("Creating medication: {Name}", product.Name);
//                var responseData = new ResponseData<Medication>();

//                var response = await _httpClient.PostAsJsonAsync("", product, serializerOptions);
//                if (!response.IsSuccessStatusCode)
//                {
//                    _logger.LogError("Failed to create medication: {StatusCode}", response.StatusCode);
//                    responseData.Success = false;
//                    responseData.ErrorMessage = $"Не удалось создать объект: {response.StatusCode}";
//                    return responseData;
//                }

//                var medication = await response.Content.ReadFromJsonAsync<Medication>(serializerOptions);
//                if (formFile != null)
//                {
//                    _logger.LogInformation("Uploading image for medication: {Id}", medication?.Id);
//                    var request = new HttpRequestMessage
//                    {
//                        Method = HttpMethod.Post,
//                        RequestUri = new Uri($"{_httpClient.BaseAddress}/{medication?.Id}")
//                    };

//                    var content = new MultipartFormDataContent();
//                    var streamContent = new StreamContent(formFile.OpenReadStream());
//                    content.Add(streamContent, "image", formFile.FileName);
//                    request.Content = content;

//                    response = await _httpClient.SendAsync(request);
//                    if (!response.IsSuccessStatusCode)
//                    {
//                        _logger.LogError("Failed to upload image: {StatusCode}", response.StatusCode);
//                        responseData.Success = false;
//                        responseData.ErrorMessage = $"Не удалось сохранить изображение: {response.StatusCode}";
//                        return responseData;
//                    }
//                }

//                responseData.Data = medication;
//                _logger.LogInformation("Successfully created medication: {Id}", medication?.Id);
//                return responseData;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating medication: {Message}", ex.Message);
//                return new ResponseData<Medication> { Success = false, ErrorMessage = ex.Message };
//            }
//        }

//        public async Task DeleteProductAsync(int id)
//        {
//            try
//            {
//                _logger.LogInformation("Deleting medication: {Id}", id);
//                var response = await _httpClient.DeleteAsync($"{id}");
//                if (!response.IsSuccessStatusCode)
//                {
//                    _logger.LogError("Failed to delete medication: {StatusCode}", response.StatusCode);
//                    throw new Exception($"Ошибка при удалении объекта: {response.StatusCode}");
//                }
//                _logger.LogInformation("Successfully deleted medication: {Id}", id);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting medication: {Message}", ex.Message);
//                throw;
//            }
//        }

//        public async Task<ResponseData<Medication>> GetProductByIdAsync(int id)
//        {
//            try
//            {
//                _logger.LogInformation("Requesting medication by ID: {Id}", id);
//                var responseData = new ResponseData<Medication>();

//                var response = await _httpClient.GetAsync($"{id}");
//                if (!response.IsSuccessStatusCode)
//                {
//                    _logger.LogError("Failed to retrieve medication: {StatusCode}", response.StatusCode);
//                    responseData.Success = false;
//                    responseData.ErrorMessage = $"Ошибка при получении объекта: {response.StatusCode}";
//                    return responseData;
//                }

//                var medication = await response.Content.ReadFromJsonAsync<Medication>();
//                if (medication == null)
//                {
//                    _logger.LogWarning("Medication not found: {Id}", id);
//                    responseData.Success = false;
//                    responseData.ErrorMessage = "Объект не найден";
//                    return responseData;
//                }

//                responseData.Data = medication;
//                _logger.LogInformation("Successfully retrieved medication: {Id}", id);
//                return responseData;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving medication: {Message}", ex.Message);
//                return new ResponseData<Medication> { Success = false, ErrorMessage = ex.Message };
//            }
//        }

//        public async Task UpdateProductAsync(int id, Medication product, IFormFile? formFile)
//        {
//            try
//            {
//                _logger.LogInformation("Updating medication: {Id}", id);
//                var response = await _httpClient.PutAsJsonAsync($"{id}", product);
//                if (!response.IsSuccessStatusCode)
//                {
//                    _logger.LogError("Failed to update medication: {StatusCode}", response.StatusCode);
//                    throw new Exception($"Ошибка при обновлении объекта: {response.StatusCode}");
//                }

//                if (formFile != null)
//                {
//                    _logger.LogInformation("Uploading new image for medication: {Id}", id);
//                    var request = new HttpRequestMessage
//                    {
//                        Method = HttpMethod.Post,
//                        RequestUri = new Uri($"{_httpClient.BaseAddress}/{id}")
//                    };

//                    var content = new MultipartFormDataContent();
//                    var streamContent = new StreamContent(formFile.OpenReadStream());
//                    content.Add(streamContent, "image", formFile.FileName);
//                    request.Content = content;

//                    response = await _httpClient.SendAsync(request);
//                    if (!response.IsSuccessStatusCode)
//                    {
//                        _logger.LogError("Failed to upload image: {StatusCode}", response.StatusCode);
//                        throw new Exception($"Ошибка при загрузке изображения: {response.StatusCode}");
//                    }
//                }

//                _logger.LogInformation("Successfully updated medication: {Id}", id);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating medication: {Message}", ex.Message);
//                throw;
//            }
//        }
//    }
//}

using _30333_Labs_Kravchenko.Domain.Entities;
using System.Text.Json;
using _30333_Labs_Kravchenko.Domain.Models;

namespace _30333_Labs_Kravchenko.UI.Services
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiProductService> _logger;

        public ApiProductService(HttpClient httpClient, ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://localhost:7002/api/medications");
        }

        public async Task<ResponseData<ProductListModel<Medication>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            try
            {
                var queryData = new Dictionary<string, string>
                {
                    { "pageNo", pageNo.ToString() }
                };

                if (!string.IsNullOrEmpty(categoryNormalizedName))
                {
                    queryData.Add("category", categoryNormalizedName);
                }

                var query = QueryString.Create(queryData);
                _logger.LogInformation("Requesting medications from {BaseAddress}{Query}", _httpClient.BaseAddress, query.Value);

                var response = await _httpClient.GetAsync(query.Value);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseData<ProductListModel<Medication>>>();
                    if (result == null)
                    {
                        _logger.LogWarning("No data returned from medications API");
                        return new ResponseData<ProductListModel<Medication>> { Success = false, ErrorMessage = "No data returned" };
                    }
                    _logger.LogInformation("Successfully retrieved {Count} medications for category: {Category}, page: {Page}",
                        result.Data?.Items.Count() ?? 0, categoryNormalizedName ?? "all", pageNo);
                    return result;
                }

                _logger.LogError("Failed to retrieve medications: {StatusCode}", response.StatusCode);
                return new ResponseData<ProductListModel<Medication>>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка чтения API: {response.StatusCode}"
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error retrieving medications: {Message}", ex.Message);
                return new ResponseData<ProductListModel<Medication>> { Success = false, ErrorMessage = $"HTTP error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medications: {Message}", ex.Message);
                return new ResponseData<ProductListModel<Medication>> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ResponseData<Medication>> CreateProductAsync(Medication product, IFormFile? formFile)
        {
            try
            {
                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                _logger.LogInformation("Creating medication: {Name}", product.Name);
                var responseData = new ResponseData<Medication>();

                var response = await _httpClient.PostAsJsonAsync("", product, serializerOptions);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to create medication: {StatusCode}", response.StatusCode);
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Не удалось создать объект: {response.StatusCode}";
                    return responseData;
                }

                var medication = await response.Content.ReadFromJsonAsync<Medication>(serializerOptions);
                if (formFile != null)
                {
                    _logger.LogInformation("Uploading image for medication: {Id}", medication?.Id);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{_httpClient.BaseAddress}/{medication?.Id}/image")
                    };

                    var content = new MultipartFormDataContent();
                    var streamContent = new StreamContent(formFile.OpenReadStream());
                    content.Add(streamContent, "image", formFile.FileName);
                    request.Content = content;

                    response = await _httpClient.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to upload image: {StatusCode}", response.StatusCode);
                        responseData.Success = false;
                        responseData.ErrorMessage = $"Не удалось сохранить изображение: {response.StatusCode}";
                        return responseData;
                    }
                }

                responseData.Data = medication;
                _logger.LogInformation("Successfully created medication: {Id}", medication?.Id);
                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medication: {Message}", ex.Message);
                return new ResponseData<Medication> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting medication: {Id}", id);
                var response = await _httpClient.DeleteAsync($"{id}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to delete medication: {StatusCode}", response.StatusCode);
                    throw new Exception($"Ошибка при удалении объекта: {response.StatusCode}");
                }
                _logger.LogInformation("Successfully deleted medication: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting medication: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ResponseData<Medication>> GetProductByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Requesting medication by ID: {Id}", id);
                var responseData = new ResponseData<Medication>();

                var response = await _httpClient.GetAsync($"{id}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to retrieve medication: {StatusCode}", response.StatusCode);
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Ошибка при получении объекта: {response.StatusCode}";
                    return responseData;
                }

                var medication = await response.Content.ReadFromJsonAsync<Medication>();
                if (medication == null)
                {
                    _logger.LogWarning("Medication not found: {Id}", id);
                    responseData.Success = false;
                    responseData.ErrorMessage = "Объект не найден";
                    return responseData;
                }

                responseData.Data = medication;
                _logger.LogInformation("Successfully retrieved medication: {Id}", id);
                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medication: {Message}", ex.Message);
                return new ResponseData<Medication> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task UpdateProductAsync(int id, Medication product, IFormFile? formFile)
        {
            try
            {
                _logger.LogInformation("Updating medication: {Id}", id);
                var response = await _httpClient.PutAsJsonAsync($"{id}", product);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to update medication: {StatusCode}", response.StatusCode);
                    throw new Exception($"Ошибка при обновлении объекта: {response.StatusCode}");
                }

                if (formFile != null)
                {
                    _logger.LogInformation("Uploading new image for medication: {Id}", id);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{_httpClient.BaseAddress}/{id}/image")
                    };

                    var content = new MultipartFormDataContent();
                    var streamContent = new StreamContent(formFile.OpenReadStream());
                    content.Add(streamContent, "image", formFile.FileName);
                    request.Content = content;

                    response = await _httpClient.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to upload image: {StatusCode}", response.StatusCode);
                        throw new Exception($"Ошибка при загрузке изображения: {response.StatusCode}");
                    }
                }

                _logger.LogInformation("Successfully updated medication: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating medication: {Message}", ex.Message);
                throw;
            }
        }
    }
}