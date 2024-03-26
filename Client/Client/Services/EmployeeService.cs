using Client.Models;
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

        public async Task<EmployeeViewModel> GetEmployeeById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Employee/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EmployeeViewModel>();
        }

        public async Task<UpdateEmployeeDTO> GetUpdateEmployeeById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Employee/{id}");
            response.EnsureSuccessStatusCode();
            var employee = await response.Content.ReadFromJsonAsync<EmployeeViewModel>();

            // Map the response to UpdateEmployeeViewModel
            var updateEmployeeViewModel = new UpdateEmployeeDTO
            {
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                JobId = employee.Job.JobId,
                JobTitle = employee.Job.JobTitle,
                BaseSalary = employee.Job.BaseSalary,
                Department = employee.Job.Department,
                Manager = new Manager
                {
                    EmployeeId = employee.Job.Manager.EmployeeId,
                    FirstName = employee.Job.Manager.FirstName,
                    LastName = employee.Job.Manager.LastName,
                    Email = employee.Job.Manager.Email,
                    PhoneNumber = employee.Job.Manager.PhoneNumber,
                    Address = employee.Job.Manager.Address,
                    JobId = employee.Job.Manager.JobId
                },
                EndDate = employee.EndDate,
                Salary = employee.Salary,
                EmploymentType = employee.EmploymentType
            };

            return updateEmployeeViewModel;
        }

        public async Task UpdateEmployeeByID(int id, UpdateEmployeeDTO updateEmployeeDTO)
        {

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
    }
}
