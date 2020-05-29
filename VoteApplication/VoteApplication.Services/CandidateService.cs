using System.Collections.Generic;
using System.Linq;
using VoteApplication.Repositories;
using VoteApplication.Services.Models;

namespace VoteApplication.Services
{
    public class CandidateService
    {
        private readonly AppDbContext _dbContext;

        public CandidateService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            //solution only for in-memory database provider https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
            _dbContext.Database.EnsureCreated();
        }

        public List<CandidateModel> GetAllCandidates()
        {
            return _dbContext.Candidates.Select(x => new CandidateModel(x.Id, string.Join(" ", x.Surname, x.Name))).ToList();
        }
    }
}