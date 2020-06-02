using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        }

        public async Task<IEnumerable<ResultModel>> GetResultsAsync()
        {
            if (DateTimeOffset.Compare(DateTimeOffset.Now,
                DateTimeOffset.ParseExact(_appSettings.ResultPublicationDateTimeValue,
                    ServicesStartup.DefaultDateTimeFormat, CultureInfo.InvariantCulture)) < 0)
            {
                return new List<ResultModel>();
            }

            //solution to fix Unable to cast object of type 'System.Linq.Expressions.NewExpression' to type 'System.Linq.Expressions.MethodCallExpression' exception 
            //return await _dbContext.Candidates.Include(x => x.Votes).Select(x =>
            //    new ResultModel(x.Surname, x.Name, x.Votes.Count)).ToListAsync();
            return (await _dbContext.Candidates.Include(x => x.Votes)
                .Select(x => new {surname = x.Surname, name = x.Name, votesCount = x.Votes.Count })
                .ToListAsync())
                .Select(y => new ResultModel(y.surname, y.name, y.votesCount));
        }
    }
}