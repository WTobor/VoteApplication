using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using VoteApplication.Repositories;
using VoteApplication.Services.Models;

namespace VoteApplication.Services
{
    public class ResultService
    {
        private readonly AppDbContext _dbContext;
        private readonly IOptions<AppSettings> _appSettings;

        public ResultService(AppDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;
            this._appSettings = appSettings;
            //solution only for in-memory database provider https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
            _dbContext.Database.EnsureCreated();
        }

        public List<ResultModel> GetResults()
        {
            var result = new List<ResultModel>();
            foreach (var candidate in _dbContext.Candidates)
            {
                var candidateVoteCount = _dbContext.Votes.Count(x => x.CandidateId == candidate.Id);
                result.Add(new ResultModel(string.Join(" ", candidate.Surname, candidate.Name), candidateVoteCount));
            }
            return result;
        }
    }
}