using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using API.Models;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : Controller
	{
		//private readonly IDynamoDBContext _context;
		//private readonly IAmazonDynamoDB _dbclient;
		//private readonly IMapper _mapper;

		private readonly IEmployeeRepository _employeeRepository;
		public EmployeeController( IEmployeeRepository employeeRepository)
		{
			//_dbclient = dynamoDBClient;
			//_context = context;
			//_mapper = mapper;
			_employeeRepository = employeeRepository;
		}


		[HttpGet]
		public async Task<IActionResult> GetAllEmployees()
		{
			var allEmployees = await _employeeRepository.GetAllEmployeesAsync();

			return Ok(allEmployees);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetEmployeeById(int id)
		{
			var employee = await _employeeRepository.GetEmployeeAsync(id);
			if (employee == null)
			{
				return NotFound();
			}
			
			return Ok(employee);
		}


		[HttpPost]
		public async Task<IActionResult> CreateEmployee([FromBody] AddEmployeeDTO addEmployeeDTO)
		{
			var employee = await _employeeRepository.AddEmployeeAsync(addEmployeeDTO);
			return Ok(employee);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDTO updateEmployeeDTO)
		{
			var employee = await _employeeRepository.UpdateEmployeeAsync(id, updateEmployeeDTO);
			if (employee == null)
			{
				return NotFound();
			}
			return Ok(employee);
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> PartiallyUpdateEmployee(int id, [FromBody] JsonPatchDocument<EmployeeDTO> patchDoc)
		{
			var employee = await _employeeRepository.UpdateEmployeePatchAsync(id, patchDoc);
			if (employee == null)
			{
				return NotFound();
			}
			return Ok(employee);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEmployee(int id)
		{
			var employee = await _employeeRepository.DeleteEmployeeAsync(id);
			if (employee == null)
			{
				return NotFound();
			}
			return Ok(employee);
		}

	}
}
