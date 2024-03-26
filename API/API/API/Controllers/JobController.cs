using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using Amazon.DynamoDBv2.Model;
using System.Diagnostics;
using Microsoft.AspNetCore.JsonPatch;
using API.Services;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobController : Controller
	{
		private readonly IJobRepository _jobRepository;

		public JobController(IJobRepository jobRepository)
		{
			_jobRepository = jobRepository;
		}


		[HttpGet]
		public async Task<IActionResult> GetAllJobs()
		{
			var allJobs = await _jobRepository.GetAllJobsAsync();
			return Ok(allJobs);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetJobById(int id)
		{
			var job = await _jobRepository.GetJobAsync(id);
			if (job == null)
			{
				return NotFound();
			}
			return Ok(job);
		}

		[HttpPost]
		public async Task<IActionResult> CreateJob([FromBody] AddJobDTO addJobDTO)
		{
			var job = await _jobRepository.AddJobAsync(addJobDTO);
			return Ok(job);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobDTO updateJobDTO)
		{
			var job = await _jobRepository.UpdateJobAsync(id, updateJobDTO);
			if (job == null)
			{
				return NotFound();
			}
			return Ok(job);
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> PartiallyUpdateJob(int id, [FromBody] JsonPatchDocument<JobDTO> patchDoc)
		{
			var job = await _jobRepository.UpdateJobPatchAsync(id, patchDoc);
			if (job == null)
			{
				return NotFound();
			}
			return Ok(job);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteJob(int id)
		{
			var job = await _jobRepository.DeleteJobAsync(id);
			if (job == null)
			{
				return NotFound();
			}
			return Ok(job);
		}
	}
}
