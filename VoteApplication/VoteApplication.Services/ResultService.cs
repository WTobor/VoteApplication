using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Options;
using VoteApplication.Repositories;
using VoteApplication.Services.Models;

namespace VoteApplication.Services
{
    public class ResultService
    {
        private readonly VotingSettings _appSettings;
        private readonly AppDbContext _dbContext;

        public ResultService(AppDbContext dbContext, IOptions<VotingSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
            //solution only for in-memory database provider https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
            _dbContext.Database.EnsureCreated();
        }

        public List<ResultModel> GetResults()
        {
            var result = new List<ResultModel>();

            if (DateTimeOffset.Compare(DateTimeOffset.Now,
                DateTimeOffset.ParseExact(_appSettings.ResultPublicationDateTimeValue,
                    ServicesStartup.DefaultDateTimeFormat, CultureInfo.InvariantCulture)) >= 0)
            {
                foreach (var candidate in _dbContext.Candidates)
                {
                    var candidateVoteCount = _dbContext.Votes.Count(x => x.CandidateId == candidate.Id);
                    result.Add(new ResultModel(string.Join(" ", candidate.Surname, candidate.Name),
                        candidateVoteCount));
                }
            }

            return result;
        }
    }
}