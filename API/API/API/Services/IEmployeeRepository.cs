using API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Services
{
	public interface IEmployeeRepository
	{
		Task<IEnumerable<Employee>> GetAllEmployeesAsync();

		Task<EmployeeDTO> GetEmployeeAsync(int employeeId);
		Task<Employee> AddEmployeeAsync(AddEmployeeDTO employee);
		Task<Employee> UpdateEmployeeAsync(int employeeId, UpdateEmployeeDTO updateEmployeeDTO);
		Task<Employee> UpdateEmployeePatchAsync(int employeeId, JsonPatchDocument<EmployeeDTO> patch);
		Task<Employee> DeleteEmployeeAsync(int employeeId);
	}
}
