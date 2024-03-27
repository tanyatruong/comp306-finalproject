using Client.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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

        public async Task<EmployeeDTO> GetEmployeeById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Employee/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
        }

        public async Task<UpdateEmployeeDTO> GetUpdateEmployeeById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Employee/{id}");
            response.EnsureSuccessStatusCode();
            var employee = await response.Content.ReadFromJsonAsync<UpdateEmployeeDTO>();

            return employee;
        }

        public async Task UpdateEmployeeByID(int id, UpdateEmployeeDTO updateEmployeeDTO)
        {
            if (updateEmployeeDTO.EndDate == null)
                updateEmployeeDTO.EndDate = "";
            var response = await _httpClient.PutAsJsonAsync($"/api/Employee/{id}", updateEmployeeDTO);
            response.EnsureSuccessStatusCode();
        }


        public async Task CreateEmployee(AddEmployeeDTO newEmployee)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Employee", newEmployee);
            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Employee/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateEmployeeEndDate(int id)
        {
            var patchDoc = new JsonPatchDocument<UpdateEmployeeDTO>();
            patchDoc.Replace(e => e.EndDate, DateTime.Now.ToString("yyyy-MM-dd"));

            var serializedDoc = JsonConvert.SerializeObject(patchDoc);
            var requestContent = new StringContent(serializedDoc, Encoding.UTF8, "application/json-patch+json");
            var response = await _httpClient.PatchAsync($"api/Employee/{id}", requestContent);
            response.EnsureSuccessStatusCode();
        }
    }
}
