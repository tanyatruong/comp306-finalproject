using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using API.Models;
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
		private readonly IDynamoDBContext _context;
		private readonly IAmazonDynamoDB _dbclient;
		private readonly IMapper _mapper;

		public EmployeeController(IAmazonDynamoDB dynamoDBClient, IDynamoDBContext context, IMapper mapper)
		{
			_dbclient = dynamoDBClient;
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllEmployees()
		{
			var allEmployees = await _context.ScanAsync<Employee>(default).GetRemainingAsync();
			var allEmployeesDTO = _mapper.Map<List<EmployeeDTO>>(allEmployees);
			return Ok(allEmployeesDTO);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetEmployeeById(int id)
		{
			var employee = await _context.LoadAsync<Employee>(id);
			if (employee == null)
			{
				return NotFound();
			}
			var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
			return Ok(employeeDTO);
		}


		[HttpPost]
		public async Task<IActionResult> CreateEmployee([FromBody] AddEmployeeDTO addEmployeeDTO)
		{
			var employee = _mapper.Map<Employee>(addEmployeeDTO);

			int count = 0;
			ScanRequest request = new ScanRequest
			{
				TableName = "Employees",
				Select = Select.COUNT
			};

			var response = await _dbclient.ScanAsync(request);

			if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
			{
				if (response.Count > 0)
				{
					Trace.WriteLine($"There are {response.Count} item(s) in the table");
					count = response.Count + 1;
				}
			}

			while (true)
			{
				try
				{
					// Check if the ID exists
					var existingItem = await _context.LoadAsync<Employee>(count);

					if (existingItem == null)
					{
						// ID does not exist, return the same ID
						break;
					}
					else
					{
						// ID exists, increment it by 1
						count += 1;
					}
				}
				catch (AmazonDynamoDBException ex)
				{
					// Handle exception
					throw new Exception("Error occurred while accessing DynamoDB", ex);
				}
			}

			employee.EmployeeId = count;
			await _context.SaveAsync(employee);
			var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
			return Ok(employee);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDTO updateEmployeeDTO)
		{
			var employee = await _context.LoadAsync<Employee>(id);
			if (employee == null)
			{
				return NotFound();
			}
			_mapper.Map(updateEmployeeDTO, employee);
			await _context.SaveAsync(employee);
			return Ok(employee);
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> PartiallyUpdateEmployee(int id, [FromBody] JsonPatchDocument<EmployeeDTO> patchDoc)
		{
			var employee = await _context.LoadAsync<Employee>(id);
			if (employee == null)
			{
				return NotFound();
			}
			var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
			patchDoc.ApplyTo(employeeDTO);
			Trace.WriteLine(employeeDTO.FirstName);
			_mapper.Map(employeeDTO, employee);

			await _context.SaveAsync(employee);
			return Ok(employee);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEmployee(int id)
		{
			var employee = await _context.LoadAsync<Employee>(id);
			if (employee == null)
			{
				return NotFound();
			}
			await _context.DeleteAsync(employee);
			return Ok(employee);
		}

	}
}
