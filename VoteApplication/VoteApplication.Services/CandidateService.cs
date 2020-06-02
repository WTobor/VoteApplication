using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        }

        public async Task<List<CandidateModel>> GetAllCandidatesAsync()
        {
            return await _dbContext.Candidates.Select(x => new CandidateModel(x.Id, x.Surname, x.Name)).ToListAsync();
        }
    }
}