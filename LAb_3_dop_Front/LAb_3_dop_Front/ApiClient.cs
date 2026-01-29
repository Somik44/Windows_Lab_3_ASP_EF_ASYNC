using LAb_3_dop_Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LAb_3_dop_Front
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7296";
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        // Passenger
        public async Task<List<Passenger>> GetPassengersAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Passengers");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Passenger>>(content, _jsonOptions);
        }

        public async Task<Passenger> CreatePassengerAsync(Passenger passenger)
        {
            var json = JsonSerializer.Serialize(passenger, _jsonOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/Passengers", data);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Passenger>(content, _jsonOptions);
        }

        public async Task UpdatePassengerAsync(int id, Passenger passenger)
        {
            var json = JsonSerializer.Serialize(passenger, _jsonOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/api/Passengers/{id}", data);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePassengerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Passengers/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Passenger>> GetPassengersByBrandAsync(string brand)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Passengers/brand/{Uri.EscapeDataString(brand)}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Passenger>>(content, _jsonOptions);
        }




        // Truck
        public async Task<List<Truck>> GetTrucksAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Trucks");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Truck>>(content, _jsonOptions);
        }

        public async Task<Truck> CreateTruckAsync(Truck truck)
        {
            var json = JsonSerializer.Serialize(truck, _jsonOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/Trucks", data);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Truck>(content, _jsonOptions);
        }

        public async Task UpdateTruckAsync(int id, Truck truck)
        {
            var json = JsonSerializer.Serialize(truck, _jsonOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/api/Trucks/{id}", data);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTruckAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Trucks/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Truck>> GetTrucksByBrandAsync(string brand)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Trucks/brand/{Uri.EscapeDataString(brand)}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Truck>>(content, _jsonOptions);
        }
    }
}