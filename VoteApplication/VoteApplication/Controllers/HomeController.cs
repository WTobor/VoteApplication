using System.Diagnostics;
using System.Threading.Tasks;
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
        private readonly VoteService _voteService;
        private readonly ResultService _resultService;

        public HomeController(ILogger<HomeController> logger, CandidateService candidateService, VoteService voteService, ResultService resultService)
        {
            _resultService = resultService;
            _logger = logger;
            _candidateService = candidateService;
            _voteService = voteService;
        }

        public IActionResult Vote()
        {
            var candidates = _candidateService.GetAllCandidates();
            return View(candidates);
        }

        [HttpPost]
        public async Task<IActionResult> Vote(string nickname, int candidateId)
        {
            if (ModelState.IsValid)
            {
                var result = await _voteService.AddVoteAsync(nickname, candidateId);
                if (!string.IsNullOrEmpty(result))
                {
                    ModelState.AddModelError(string.Empty, result);
                }
            }

            var candidates = _candidateService.GetAllCandidates();
            return View(candidates);
        }

        public IActionResult Results()
        {
            var results = _resultService.GetResults();
            return View(results);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}