using WebAPI.IServices;
using static WebApi.Data.MsSQLDataAccess;
using System.Net.Http;
using System.Net.Http.Json;


namespace WebAPI.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }


        public async Task<IEnumerable<CCUSTOMER>> GetCustomers()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/DatabaseApi/GetCustomers");

            request.Headers.Add("X-Api-Key", "KevinChuang");
            request.Headers.Add("accept", "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<CCUSTOMER>>();
        }

        public async Task<IEnumerable<CCUSTOMER>> GetCustomers_Alive()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/DatabaseApi/GetCustomersAlive");

            request.Headers.Add("X-Api-Key", "KevinChuang");
            request.Headers.Add("accept", "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<CCUSTOMER>>();
        }

        public async Task<CCUSTOMER> GetCustomerBySN(Int64 customerSN)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/DatabaseApi/GetCustomersBySN/{customerSN}");

            request.Headers.Add("X-Api-Key", "KevinChuang");
            request.Headers.Add("accept", "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var x = await response.Content.ReadFromJsonAsync<CCUSTOMER>();

            return x;
        }

        public async Task<Int64> CreateCustomer(CCUSTOMER customer)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/DatabaseApi/CreateCustomer");

            request.Headers.Add("X-Api-Key", "KevinChuang");
            request.Headers.Add("accept", "application/json");

            request.Content = JsonContent.Create(customer);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Int64>();
        }

        public async Task<Int64> UpdateCustomer(CCUSTOMER customer)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/DatabaseApi/UpdateCustomer");

            request.Headers.Add("X-Api-Key", "KevinChuang");
            request.Headers.Add("accept", "application/json");

            request.Content = JsonContent.Create(customer);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Int64>();
        }


        public async Task<bool> DeleteCustomer(Int64 customerSN)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/DatabaseApi/DeleteCustomer/{customerSN}");

            request.Headers.Add("X-Api-Key", "KevinChuang");
            request.Headers.Add("accept", "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<bool>();
        }
    }
}
