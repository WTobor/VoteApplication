using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VoteApplication.Models;
using VoteApplication.Services;

namespace VoteApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CandidateService _candidateService;

        public HomeController(ILogger<HomeController> logger, CandidateService candidateService)
        {
            _logger = logger;
            _candidateService = candidateService;
        }

        public IActionResult Vote()
        {
            var candidates = _candidateService.GetAllCandidates();
            return View();
        }

        public IActionResult Results()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}