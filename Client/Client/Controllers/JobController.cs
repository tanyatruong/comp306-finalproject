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
    }
}
