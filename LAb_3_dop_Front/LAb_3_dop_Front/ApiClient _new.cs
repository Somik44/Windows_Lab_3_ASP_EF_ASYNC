using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;


namespace LAb_3_dop_Front
{
    public class ApiClient_new 
    {
        private readonly HttpClient _httpClient;
        private readonly List<ServerConfig> _servers;
        private int _currentServerIndex = 0;
        private readonly System.Timers.Timer _healthCheckTimer;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly object _lockObject = new object();

        public event EventHandler<ServerSwitchedEventArgs> ServerSwitched;

        public ApiClient_new()
        {
            _httpClient = new HttpClient();

            _servers = new List<ServerConfig>
            {
                new ServerConfig {
                    Url = "http://localhost:7926",
                    IsActive = false,
                    Priority = 1,
                    Name = "Основной",
                    LastCheck = DateTime.UtcNow
                },
                new ServerConfig {
                    Url = "http://localhost:7927",
                    IsActive = false,
                    Priority = 2,
                    Name = "Резервный",
                    LastCheck = DateTime.UtcNow
                }
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };



            _healthCheckTimer = new System.Timers.Timer(10000);
            _healthCheckTimer.Elapsed += HealthCheckCallback;
            _healthCheckTimer.AutoReset = true;
            _healthCheckTimer.Enabled = true;

        }

        public class ServerConfig
        {
            public string Url { get; set; }
            public bool IsActive { get; set; }
            public int Priority { get; set; }
            public string Name { get; set; }
            public DateTime LastCheck { get; set; }
        }


        private async Task<T> ExecuteWithFailover<T>(Func<string, Task<T>> action)
        {
            var originalIndex = _currentServerIndex;
            var attemptedServers = new HashSet<int>();

            while (attemptedServers.Count < _servers.Count)
            {
                var server = _servers[_currentServerIndex];

                try
                {
                    return await action(server.Url);
                }
                catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
                {
                    attemptedServers.Add(_currentServerIndex);

                    var oldServer = server.Name;

                    _currentServerIndex = (_currentServerIndex + 1) % _servers.Count;

                    if (attemptedServers.Count > 1)
                    {
                        ServerSwitched?.Invoke(this, new ServerSwitchedEventArgs
                        {
                            FromServer = oldServer,
                            ToServer = _servers[_currentServerIndex].Name,
                            Reason = "Сбой соединения"
                        });
                    }
                }
            }

            throw new Exception("Не удалось выполнить запрос на всех серверах");
        }

        private void SwitchToNextServer()
        {
            lock (_lockObject)
            {
                var originalIndex = _currentServerIndex;

                do
                {
                    _currentServerIndex = (_currentServerIndex + 1) % _servers.Count;

                    if (_currentServerIndex == originalIndex)
                    {
                        throw new Exception("Нет доступных серверов");
                    }

                } while (!_servers[_currentServerIndex].IsActive);

                Console.WriteLine($"Переключились на сервер: {_servers[_currentServerIndex].Url}");
            }
        }

        private async void HealthCheckCallback(object sender, ElapsedEventArgs e)
        {
            foreach (var server in _servers)
            {
                try
                {
                    string healthEndpoint = server.Name == "Основной" ? "/health-primary" : "/health-secondary";
                    var response = await _httpClient.GetAsync($"{server.Url}{healthEndpoint}");
                    server.IsActive = response.IsSuccessStatusCode;
                    server.LastCheck = DateTime.UtcNow;

                }
                catch (Exception ex)
                {
                    server.IsActive = false;
                    server.LastCheck = DateTime.UtcNow;
                }
            }

            if (!_servers[_currentServerIndex].IsActive)
            {
                SwitchToNextServer();
            }
        }



        // Passenger
        public async Task<List<Passenger>> GetPassengersAsync()
        {
            return await ExecuteWithFailover(async (baseUrl) =>
            {
                var response = await _httpClient.GetAsync($"{baseUrl}/api/Passengers");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Passenger>>(content, _jsonOptions);
            });
        }

        public async Task<Passenger> CreatePassengerAsync(Passenger passenger)
        {
            return await ExecuteWithFailover(async (baseUrl) =>
            {
                var json = JsonSerializer.Serialize(passenger, _jsonOptions);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{baseUrl}/api/Passengers", data);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Passenger>(content, _jsonOptions);
            });
        }

        public async Task UpdatePassengerAsync(int id, Passenger passenger)
        {
            await ExecuteWithFailover(async (baseUrl) =>
            {
                var json = JsonSerializer.Serialize(passenger, _jsonOptions);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{baseUrl}/api/Passengers/{id}", data);
                response.EnsureSuccessStatusCode();
                return true; // Для совместимости с generic методом
            });
        }

        public async Task DeletePassengerAsync(int id)
        {
            await ExecuteWithFailover(async (baseUrl) =>
            {
                var response = await _httpClient.DeleteAsync($"{baseUrl}/api/Passengers/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            });
        }

        public async Task<List<Passenger>> GetPassengersByBrandAsync(string brand)
        {
            return await ExecuteWithFailover(async (baseUrl) =>
            {
                var response = await _httpClient.GetAsync($"{baseUrl}/api/Passengers/brand/{Uri.EscapeDataString(brand)}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Passenger>>(content, _jsonOptions);
            });
        }

        // Truck
        public async Task<List<Truck>> GetTrucksAsync()
        {
            return await ExecuteWithFailover(async (baseUrl) =>
            {
                var response = await _httpClient.GetAsync($"{baseUrl}/api/Trucks");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Truck>>(content, _jsonOptions);
            });
        }

        public async Task<Truck> CreateTruckAsync(Truck truck)
        {
            return await ExecuteWithFailover(async (baseUrl) =>
            {
                var json = JsonSerializer.Serialize(truck, _jsonOptions);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{baseUrl}/api/Trucks", data);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Truck>(content, _jsonOptions);
            });
        }

        public async Task UpdateTruckAsync(int id, Truck truck)
        {
            await ExecuteWithFailover(async (baseUrl) =>
            {
                var json = JsonSerializer.Serialize(truck, _jsonOptions);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{baseUrl}/api/Trucks/{id}", data);

                response.EnsureSuccessStatusCode();
                return true;
            });
        }

        public async Task DeleteTruckAsync(int id)
        {
            await ExecuteWithFailover(async (baseUrl) =>
            {
                var response = await _httpClient.DeleteAsync($"{baseUrl}/api/Trucks/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            });
        }

        public async Task<List<Truck>> GetTrucksByBrandAsync(string brand)
        {
            return await ExecuteWithFailover(async (baseUrl) =>
            {
                var response = await _httpClient.GetAsync($"{baseUrl}/api/Trucks/brand/{Uri.EscapeDataString(brand)}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Truck>>(content, _jsonOptions);
            });
        }


        //get
        public string CurrentServer => _servers[_currentServerIndex].Url;
        public string CurrentServerName => _servers[_currentServerIndex].Name;
        public List<ServerConfig> GetServerStatus() => _servers.ToList();
    }

    public class ServerSwitchedEventArgs : EventArgs
    {
        public string FromServer { get; set; }
        public string ToServer { get; set; }
        public string Reason { get; set; }
        public DateTime SwitchTime { get; set; } = DateTime.Now;
    }
}