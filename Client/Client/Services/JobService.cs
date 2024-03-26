using Client.Models;

namespace Client.HttpClients
{
    public class JobService
    {
        private readonly HttpClient _httpClient;

        public JobService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<JobViewModel>> GetAllJobs()
        {
            var response = await _httpClient.GetAsync("api/Job");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<JobViewModel>>();
        }

        public async Task<JobViewModel> GetJobById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Job/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<JobViewModel>();
        }

        public async Task UpdateJob(JobViewModel updatedJob)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Job/{updatedJob.JobId}", updatedJob);
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateJob(AddJobDTO newJob)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Job", newJob);
            response.EnsureSuccessStatusCode();
        }
    }
}
