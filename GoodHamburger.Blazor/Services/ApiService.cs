using GoodHamburger.Blazor.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoodHamburger.Blazor.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ItemDto>> GetItemsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ItemDto>>("api/items") ?? new List<ItemDto>();
        }

        public async Task<List<OrderResponseDto>> GetOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<OrderResponseDto>>("api/orders") ?? new List<OrderResponseDto>();
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<OrderResponseDto>($"api/orders/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<OrderResponseDto?> CreateOrderAsync(CreateOrderDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/orders", dto);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);
                throw new Exception(message ?? "Não foi possível criar o pedido.");
            }

            return await response.Content.ReadFromJsonAsync<OrderResponseDto>();
        }

        public async Task<OrderResponseDto?> UpdateOrderAsync(CreateOrderDto dto, int id)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/orders/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);
                throw new Exception(message ?? "Não foi possível atualizar o pedido.");
            }

            return await response.Content.ReadFromJsonAsync<OrderResponseDto>();
        }

        public async Task<OrderResponseDto?> DeleteOrderAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/orders/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<OrderResponseDto>();
        }

        private static async Task<string?> ReadErrorMessageAsync(HttpResponseMessage response)
        {
            try
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                var payload = await JsonSerializer.DeserializeAsync<ApiErrorResponse>(contentStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return payload?.Message;
            }
            catch
            {
                return null;
            }
        }

        private sealed class ApiErrorResponse
        {
            public string? Message { get; set; }
        }

    }
}
