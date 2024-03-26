using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using System.Diagnostics;

namespace API.Services
{
	public class JobRepository : IJobRepository
	{
		private readonly IDynamoDBContext _context;
		private readonly IAmazonDynamoDB _dbclient;
		private readonly IMapper _mapper;

		public JobRepository(IAmazonDynamoDB dynamoDBClient, IDynamoDBContext context, IMapper mapper)
		{
			_dbclient = dynamoDBClient;
			_context = context;
			_mapper = mapper;
		}

		public async Task<IEnumerable<Job>> GetAllJobsAsync()
		{
			var allJobs = await _context.ScanAsync<Job>(default).GetRemainingAsync();
			return allJobs;
		}

		public async Task<Job> GetJobAsync(int jobId)
		{
			var job = await _context.LoadAsync<Job>(jobId);
			return job;
		}

		public async Task<Job> AddJobAsync(AddJobDTO job)
		{
			var newJob = _mapper.Map<Job>(job);

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

			newJob.JobId = count;
			await _context.SaveAsync(newJob);
			return newJob;
		}

		public async Task<Job> UpdateJobAsync(int jobId, UpdateJobDTO updateJobDTO)
		{
			var job = await _context.LoadAsync<Job>(jobId);
			if (job == null)
			{
				return null;
			}

			job = _mapper.Map(updateJobDTO, job);
			await _context.SaveAsync(job);
			return job;
		}

		public async Task<Job> UpdateJobPatchAsync(int jobId, JsonPatchDocument<JobDTO> patchDoc)
		{
			var job = await _context.LoadAsync<Job>(jobId);
			if (job == null)
			{
				return null;
			}

			var jobDTO = _mapper.Map<JobDTO>(job);
			patchDoc.ApplyTo(jobDTO);
			job = _mapper
				.Map(jobDTO, job);
			await _context.SaveAsync(job);
			return job;
		}

		public async Task<Job> DeleteJobAsync(int jobId)
		{
			var job = await _context.LoadAsync<Job>(jobId);
			if (job == null)
			{
				return null;
			}

			await _context.DeleteAsync<Job>(jobId);
			return job;
		}
	}
}
