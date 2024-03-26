using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using AutoMapper;
using API.Models;
using Amazon.DynamoDBv2.Model;
using System.Diagnostics;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Services
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly IDynamoDBContext _context;
		private readonly IAmazonDynamoDB _dbclient;
		private readonly IMapper _mapper;

		public EmployeeRepository(IAmazonDynamoDB dynamoDBClient, IDynamoDBContext context, IMapper mapper)
		{
			_dbclient = dynamoDBClient;
			_context = context;
			_mapper = mapper;
		}

		public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
		{
			var allEmployees = await _context.ScanAsync<Employee>(default).GetRemainingAsync();
			
			return allEmployees;
		}

		public async Task<EmployeeDTO> GetEmployeeAsync(int employeeId)
		{
			var employee = await _context.LoadAsync<Employee>(employeeId);
			if (employee == null)
			{
				return null;
			}

			var employeeDTO = _mapper.Map<EmployeeDTO>(employee);

			return employeeDTO;
		}

		public async Task<Employee> AddEmployeeAsync(AddEmployeeDTO addEmployeeDTO)
		{
			var newEmployee = _mapper.Map<EmployeeDTO>(addEmployeeDTO);

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
			var employee = _mapper.Map<Employee>(newEmployee);
			employee.EmployeeId = count;
			await _context.SaveAsync(employee);
			return employee;
		}

		public async Task<Employee> UpdateEmployeeAsync(int id, UpdateEmployeeDTO updateEmployeeDTO)
		{
			var employee = await _context.LoadAsync<Employee>(id);
			if (employee == null)
			{
				return null;
			}
			_mapper.Map(updateEmployeeDTO, employee);
			await _context.SaveAsync(employee);
			return employee;
		}

		public async Task<Employee> UpdateEmployeePatchAsync(int id, JsonPatchDocument<EmployeeDTO> patchDoc)
		{

			var employee = await _context.LoadAsync<Employee>(id);
			if (employee == null)
			{
				return null;
			}
			var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
			patchDoc.ApplyTo(employeeDTO);
			_mapper.Map(employeeDTO, employee);

			await _context.SaveAsync(employee);
			return employee;
		}

		public async Task<Employee> DeleteEmployeeAsync(int id)
		{
			var employee = await _context.LoadAsync<Employee>(id);
			if (employee == null)
			{
				return null;
			}
			await _context.DeleteAsync(employee);
			return employee;
		}
	}
}
