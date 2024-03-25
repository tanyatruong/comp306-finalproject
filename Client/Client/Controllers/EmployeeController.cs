using Client.HttpClients;
using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeHttpClient;

        public EmployeeController(EmployeeService employeeHttpClient)
        {
            _employeeHttpClient = employeeHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            var allEmployees = await _employeeHttpClient.GetAllEmployees();
            return View(allEmployees);
        }

        public async Task<IActionResult> View(int id)
        {
            var employee = await _employeeHttpClient.GetEmployeeById(id);
            return View(employee);
        }

        public async Task<IActionResult> Update(int id)
        {
            var employee = await _employeeHttpClient.GetEmployeeById(id);
            var updateEmployeeDTO = new UpdateEmployeeDTO
            {
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                JobId = employee.Job.JobId,
                JobTitle = employee.Job.JobTitle,
                BaseSalary = employee.Job.BaseSalary,
                Department = employee.Job.Department,
                Manager = employee.Job.Manager,
                EndDate = employee.EndDate,
                Salary = employee.Salary,
                EmploymentType = employee.EmploymentType 
            };
            await _employeeHttpClient.UpdateEmployeeByID(id, updateEmployeeDTO);
            return RedirectToAction("Index");
        }
    }
}
