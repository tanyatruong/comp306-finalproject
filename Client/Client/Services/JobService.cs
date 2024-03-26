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

        public async Task<UpdateJobDTO> GetUpdateJobById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Job/{id}");
            response.EnsureSuccessStatusCode();
            var job = await response.Content.ReadFromJsonAsync<JobViewModel>();

            // Map the response to UpdateEmployeeViewModel
            var updateJobViewModel = new UpdateJobDTO
            {
                Manager = new Manager
                {
                    EmployeeId = job.Manager.EmployeeId,
                    FirstName = job.Manager.FirstName,
                    LastName = job.Manager.LastName,
                    Email = job.Manager.Email,
                    PhoneNumber = job.Manager.PhoneNumber,
                    Address = job.Manager.Address,
                    JobId = job.Manager.JobId
                }
            };

            return updateJobViewModel;
        }

        public async Task UpdateJobById(int id, UpdateJobDTO updateJobDTO)
        {

            var response = await _httpClient.PutAsJsonAsync($"/api/Job/{id}", updateJobDTO);
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateJob(AddJobDTO newJob)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Job", newJob);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteJob(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Job/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
