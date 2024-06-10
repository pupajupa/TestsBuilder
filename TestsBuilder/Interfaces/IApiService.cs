using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Responses;

namespace TestsBuilder.Interfaces
{
    public interface IApiService
    {
        Task<List<TestResponse>> GetAllTestsInfo(string token);
        string AuthorizationHeader { get; set; }
        Task<TResponse> GetAsync<TResponse>(string endpoint);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest content);
        Task<TResponse> PostWithHostIdAsync<TRequest, TResponse>(string hostId, string endpoint, TRequest content);

        Task<AuthResponse> Login(string email, string password);

        Task<AuthResponse> Register(string first, string last, string login, string password);

    }
}
