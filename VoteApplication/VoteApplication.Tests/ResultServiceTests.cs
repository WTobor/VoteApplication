using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VoteApplication.Services;
using Xunit;

namespace VoteApplication.Tests
{
    public class ResultServiceTests
    {
        public ResultServiceTests()
        {
            _testHelper = new TestHelper();
        }

        private readonly TestHelper _testHelper;

        [Fact]
        public async Task GetResults_WhenAddedVoteAndBeforePublishTime_ShouldReturnEmptyResultsAsync()
        {
            //Arrange
            await _testHelper.InitializeDatabaseAsync();
            var voteService = new VoteService(_testHelper.TestAppDbContext);
            var testAppSettings = _testHelper.GetSettings(DateTimeOffset.Now.AddDays(1));
            var resultService =
                new ResultService(_testHelper.TestAppDbContext, Options.Create(testAppSettings));
            var candidate = await _testHelper.TestAppDbContext.Candidates.FirstAsync();
            await voteService.AddVoteAsync("test1", candidate.Id);

            //Act
            var result = await resultService.GetResultsAsync();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetResults_WhenAddedVoteAndInPublishTime_ShouldReturnCorrectResultsAsync()
        {
            //Arrange
            await _testHelper.InitializeDatabaseAsync();
            var voteService = new VoteService(_testHelper.TestAppDbContext);
            var testAppSettings = _testHelper.GetSettings(DateTimeOffset.Now);
            var resultService =
                new ResultService(_testHelper.TestAppDbContext, Options.Create(testAppSettings));
            var candidate = await _testHelper.TestAppDbContext.Candidates.FirstAsync();
            await voteService.AddVoteAsync("test1", candidate.Id);

            //Act
            var result = await resultService.GetResultsAsync();

            //Assert
            Assert.Equal(3, result.Count());
            Assert.Equal(1, result.First(x => x.CandidateFullName.StartsWith(candidate.Surname)).CandidateVoteCount);
        }

        [Fact]
        public async Task GetResults_WhenAddedVotesAndAfterPublishTime_ShouldReturnCorrectResultsAsync()
        {
            //Arrange
            await _testHelper.InitializeDatabaseAsync();
            var voteService = new VoteService(_testHelper.TestAppDbContext);
            var testAppSettings = _testHelper.GetSettings(DateTimeOffset.Now.AddDays(-1));
            var resultService =
                new ResultService(_testHelper.TestAppDbContext, Options.Create(testAppSettings));
            var candidate1 = await _testHelper.TestAppDbContext.Candidates.FindAsync(1);
            var candidate2 = await _testHelper.TestAppDbContext.Candidates.FindAsync(2);
            var candidate3 = await _testHelper.TestAppDbContext.Candidates.FindAsync(3);
            await voteService.AddVoteAsync("test1", candidate1.Id);
            await voteService.AddVoteAsync("test2", candidate2.Id);
            await voteService.AddVoteAsync("test3", candidate1.Id);

            //Act
            var result = await resultService.GetResultsAsync();

            //Assert
            Assert.Equal(3, result.Count());
            Assert.Equal(2, result.First(x => x.CandidateFullName.StartsWith(candidate1.Surname)).CandidateVoteCount);
            Assert.Equal(1, result.First(x => x.CandidateFullName.StartsWith(candidate2.Surname)).CandidateVoteCount);
            Assert.Equal(0, result.First(x => x.CandidateFullName.StartsWith(candidate3.Surname)).CandidateVoteCount);
        }

        [Fact]
        public async Task GetResults_WhenNoVotesAndAfterPublishTime_ShouldReturnEmptyResultsAsync()
        {
            //Arrange
            await _testHelper.InitializeDatabaseAsync();
            var testAppSettings = _testHelper.GetSettings(DateTimeOffset.Now.AddDays(-1));

            var service = new ResultService(_testHelper.TestAppDbContext, Options.Create(testAppSettings));

            //Act
            var result = await service.GetResultsAsync();

            //Assert
            Assert.Equal(3, result.Count());
            Assert.True(result.ToList().TrueForAll(x => x.CandidateVoteCount == 0));
        }
    }
}