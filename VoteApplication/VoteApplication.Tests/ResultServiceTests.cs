using System.Linq;
using System.Threading.Tasks;
using VoteApplication.Services;
using Xunit;

namespace VoteApplication.Tests
{
    public class ResultServiceTests
    {
        public ResultServiceTests()
        {
            _testDatabaseHelper = new TestDatabaseHelper();
        }

        private readonly TestDatabaseHelper _testDatabaseHelper;

        [Fact]
        public void GetResults_WhenNoVotes_ShouldReturnEmptyResultsAsync()
        {
            //Arrange
            _testDatabaseHelper.InitializeDatabase();
            var service = new ResultService(_testDatabaseHelper.TestAppDbContext);

            //Act
            var result = service.GetResults();

            //Assert
            Assert.Equal(3, result.Count);
            Assert.True(result.TrueForAll(x => x.CandidateVoteCount == 0));
        }

        [Fact]
        public async Task GetResults_WhenAddedVotes_ShouldReturnCorrectResultsAsync()
        {
            //Arrange
            _testDatabaseHelper.InitializeDatabase();
            var voteService = new VoteService(_testDatabaseHelper.TestAppDbContext);
            var resultService = new ResultService(_testDatabaseHelper.TestAppDbContext);
            var candidate1 = await _testDatabaseHelper.TestAppDbContext.Candidates.FindAsync(1);
            var candidate2 = await _testDatabaseHelper.TestAppDbContext.Candidates.FindAsync(2);
            var candidate3 = await _testDatabaseHelper.TestAppDbContext.Candidates.FindAsync(3);
            await voteService.AddVoteAsync("test1", candidate1.Id);
            await voteService.AddVoteAsync("test2", candidate2.Id);
            await voteService.AddVoteAsync("test3", candidate1.Id);


            //Act
            var result = resultService.GetResults();

            //Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(2, result.First(x => x.CandidateFullName.StartsWith(candidate1.Surname)).CandidateVoteCount);
            Assert.Equal(1, result.First(x => x.CandidateFullName.StartsWith(candidate2.Surname)).CandidateVoteCount);
            Assert.Equal(0, result.First(x => x.CandidateFullName.StartsWith(candidate3.Surname)).CandidateVoteCount);
        }
    }
}