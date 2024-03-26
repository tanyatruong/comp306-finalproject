using Client.HttpClients;
using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        public EmployeeController(EmployeeService employeeHttpClient)
        {
            _employeeService = employeeHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            var allEmployees = await _employeeService.GetAllEmployees();
            return View(allEmployees);
        }

        public async Task<IActionResult> View(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            return View(employee);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            UpdateEmployeeDTO employee = await _employeeService.GetUpdateEmployeeById(id);
            ViewBag.EmployeeId = id;
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int EmployeeId, UpdateEmployeeDTO updateEmployeeDTO)
        {
            await _employeeService.UpdateEmployeeByID(EmployeeId, updateEmployeeDTO);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Manager newManager = new Manager();
            AddEmployeeDTO employee = new AddEmployeeDTO()
            {
                Manager = newManager
            };
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddEmployeeDTO employee)
        {
            
            await _employeeService.CreateEmployee(employee);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.Delete(id);
            return RedirectToAction("Index");
        } 
    }
}
