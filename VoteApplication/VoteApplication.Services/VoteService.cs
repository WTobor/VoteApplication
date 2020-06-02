using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        }

        public async Task<string> AddVoteAsync(string userNickname, int candidateId)
        {
            if (CheckIfNickNameIsCorrect(userNickname))
            {
                return Messages.NicknameCannotBeEmpty;
            }

            if (await CheckIfUserVotedAsync(userNickname))
            {
                return Messages.UserAlreadyVoted;
            }

            if (CheckIfCandidateExists(candidateId))
            {
                return Messages.CandidateDoesNotExist;
            }

            await _dbContext.Votes.AddAsync(new Vote(userNickname, candidateId));
            await _dbContext.SaveChangesAsync();
            return string.Empty;
        }

        private async Task<bool> CheckIfUserVotedAsync(string nickName)
        {
            return await _dbContext.Votes.AnyAsync(x =>
                string.Equals(x.UserNickname, nickName, StringComparison.InvariantCulture));
        }

        private static bool CheckIfNickNameIsCorrect(string nickname)
        {
            return string.IsNullOrWhiteSpace(nickname);
        }

        private bool CheckIfCandidateExists(int candidateId)
        {
            var candidate = _dbContext.Candidates.Find(candidateId);
            return candidate == null;
        }
    }
}
