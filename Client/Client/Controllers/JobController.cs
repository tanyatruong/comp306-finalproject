using Client.HttpClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class JobController : Controller
    {
        private readonly JobService _jobHttpClient;

        public JobController(JobService jobHttpClient)
        {
            _jobHttpClient = jobHttpClient;
        }
        public async Task<IActionResult> Index()
        {
            var allEmployees = await _jobHttpClient.GetAllJobs();
            return View(allEmployees);
        }

        public async Task<IActionResult> View(int id)
        {
            var employee = await _jobHttpClient.GetJobById(id);
            return View(employee);
        }
    }
}
