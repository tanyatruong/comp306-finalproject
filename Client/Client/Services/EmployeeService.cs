using Client.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployees()
        {
            var response = await _httpClient.GetAsync("/api/Employee");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeViewModel>>();
        }

        public async Task<EmployeeViewModel> GetEmployeeById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Employee/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EmployeeViewModel>();
        }

        public async Task UpdateEmployeeByID(int id, UpdateEmployeeDTO updatedEmployee)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Employee/{id}", updatedEmployee);
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateEmployee(EmployeeViewModel newEmployee)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Employee", newEmployee);
            response.EnsureSuccessStatusCode();
        }
    }
}
