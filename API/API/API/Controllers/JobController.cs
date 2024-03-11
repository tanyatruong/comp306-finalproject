using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using Amazon.DynamoDBv2.Model;
using System.Diagnostics;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobController : Controller
	{
		private readonly IDynamoDBContext _context;
		private readonly IAmazonDynamoDB _dbclient;
		private readonly IMapper _mapper;

		public JobController(IAmazonDynamoDB dynamoDBClient, IDynamoDBContext context, IMapper mapper)
		{
			_dbclient = dynamoDBClient;
			_context = context;
			_mapper = mapper;
		}


		[HttpGet]
		public async Task<IActionResult> GetAllJobs()
		{
			var allJobs = await _context.ScanAsync<Job>(default).GetRemainingAsync();
			return Ok(allJobs);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetJobById(int id)
		{
			var job = await _context.LoadAsync<Job>(id);
			if (job == null)
			{
				return NotFound();
			}
			return Ok(job);
		}

		[HttpPost]
		public async Task<IActionResult> CreateJob([FromBody] AddJobDTO addJobDTO)
		{
			var job = _mapper.Map<Job>(addJobDTO);

			int count = 0;
			ScanRequest request = new ScanRequest
			{
				TableName = "Jobs",
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
					var existingItem = await _context.LoadAsync<Job>(count);

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

			job.JobId = count;
			await _context.SaveAsync(job);
			var jobDTO = _mapper.Map<JobDTO>(job);
			return Ok(jobDTO);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobDTO updateJobDTO)
		{
			var job = await _context.LoadAsync<Job>(id);
			if (job == null)
			{
				return NotFound();
			}
			_mapper.Map(updateJobDTO, job);
			await _context.SaveAsync(job);
			return Ok(job);
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> PartiallyUpdateJob(int id, [FromBody] JsonPatchDocument<JobDTO> patchDoc)
		{
			var job = await _context.LoadAsync<Job>(id);
			if (job == null)
			{
				return NotFound();
			}
			var jobDTO = _mapper.Map<JobDTO>(job);
			patchDoc.ApplyTo(jobDTO);
			_mapper.Map(jobDTO, job);

			await _context.SaveAsync(job);
			return Ok(job);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteJob(int id)
		{
			await _context.DeleteAsync<Job>(id);
			return Ok();
		}
	}
}
