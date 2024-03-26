using Client.HttpClients;
using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class JobController : Controller
    {
        private readonly JobService _jobService;

        public JobController(JobService jobHttpClient)
        {
            _jobService = jobHttpClient;
        }
        public async Task<IActionResult> Index()
        {
            var allEmployees = await _jobService.GetAllJobs();
            return View(allEmployees);
        }

        public async Task<IActionResult> View(int id)
        {
            var employee = await _jobService.GetJobById(id);
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            UpdateJobDTO job = await _jobService.GetUpdateJobById(id);
            ViewBag.JobId = id;
            return View(job);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int JobId, UpdateJobDTO updateJobDTO)
        {
            await _jobService.UpdateJobById(JobId, updateJobDTO);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Manager newManager = new Manager();
            AddJobDTO job = new AddJobDTO()
            {
                Manager = newManager
            };
            return View(job);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddJobDTO job)
        {

            await _jobService.CreateJob(job);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _jobService.DeleteJob(id);
            return RedirectToAction("Index");
        }
    }
}
