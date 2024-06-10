using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;
using TestsBuilder.Requests;
using TestsBuilder.Responses;

namespace TestsBuilder.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        public string _accessToken;

        private const string _url = "https://c2dc-185-128-201-41.ngrok-free.app";
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string AuthorizationHeader
        {
            get => _accessToken;
            set
            {
                _accessToken = value;
                if (!string.IsNullOrEmpty(_accessToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                }
            }
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error occurred during GET request", ex);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest content)
        {
            try
            {
                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpoint, jsonContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error occurred during POST request", ex);
            }
        }

        public async Task<TResponse> PostWithHostIdAsync<TRequest, TResponse>(string hostId, string endpoint, TRequest content)
        {
            try
            {
                var fullEndpoint = $"hosts/{hostId}/{endpoint}";
                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(fullEndpoint, jsonContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error occurred during POST request", ex);
            }
        }

        public async Task<AuthResponse> Login(string email, string password)
        {
            LoginRequest authData = new(email, password);

            string jsonContent = JsonConvert.SerializeObject(authData);

            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/auth/login", requestContent);

            string jsonInfo = await response.Content.ReadAsStringAsync();

            AuthResponse res = JsonConvert.DeserializeObject<AuthResponse>(jsonInfo);

            return res;
        }

        public async Task<AuthResponse> Register(string first, string last, string login, string password)
        {
            RegistrationRequest authData = new(first, last, login, password);

            string jsonContent = JsonConvert.SerializeObject(authData);

            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/auth/register", requestContent);

            string jsonInfo = await response.Content.ReadAsStringAsync();

            AuthResponse res = JsonConvert.DeserializeObject<AuthResponse>(jsonInfo);

            return res;
        }

        public async Task<List<TestResponse>> GetAllTestsInfo(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/tests");

            string jsonInfo = await response.Content.ReadAsStringAsync();

            List<TestResponse> testResponse = JsonConvert.DeserializeObject<List<TestResponse>>(jsonInfo);

            return testResponse;
        }
    }
}
