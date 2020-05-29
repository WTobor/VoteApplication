using System;
using System.Threading.Tasks;
using VoteApplication.Repositories;
using VoteApplication.Repositories.Models;

namespace VoteApplication.Services
{
    public class VoteService
    {
        private readonly AppDbContext _dbContext;

        public VoteService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            //solution only for in-memory database provider https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
            _dbContext.Database.EnsureCreated();
        }

        public async Task<string> AddVoteAsync(string userNickname, int candidateId)
        {
            try
            {
                await _dbContext.Votes.AddAsync(new Vote(userNickname, candidateId));
                await _dbContext.SaveChangesAsync();
                return string.Empty;
            }
            catch (Exception e)
            {
                //temporary solution to return only exception message
                return e.Message;
            }

        }
    }
}
