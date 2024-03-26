using API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Services
{
	public interface IJobRepository
	{
		Task<IEnumerable<Job>> GetAllJobsAsync();
		Task<Job> GetJobAsync(int jobId);
		Task<Job> AddJobAsync(AddJobDTO job);
		Task<Job> UpdateJobAsync(int jobId, UpdateJobDTO updateJobDTO);
		Task<Job> UpdateJobPatchAsync(int jobId, JsonPatchDocument<JobDTO> patchDoc);
		Task<Job> DeleteJobAsync(int jobId);
	}
}
