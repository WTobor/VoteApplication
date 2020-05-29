using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoteApplication.Services;
using Xunit;

namespace VoteApplication.Tests
{
    public class VoteServiceTests
    {
        private readonly TestDatabaseHelper _testDatabaseHelper;

        public VoteServiceTests()
        {
            _testDatabaseHelper = new TestDatabaseHelper();
        }

        [Fact]
        public async Task AddVote_WithCorrectData_ShouldAddVoteAsync()
        {
            //Arrange
            _testDatabaseHelper.InitializeDatabase();
            var service = new VoteService(_testDatabaseHelper.TestAppDbContext);
            var candidate = await _testDatabaseHelper.TestAppDbContext.Candidates.FirstAsync();

            //Act
            var result = await service.AddVoteAsync("test", candidate.Id);

            //Assert
            Assert.True(string.IsNullOrEmpty(result));
        }

        [Fact]
        public async Task AddVote_WithDuplicatedUserName_ShouldReturnUserAlreadyVotedErrorMessageAsync()
        {
            //Arrange
            _testDatabaseHelper.InitializeDatabase();
            var service = new VoteService(_testDatabaseHelper.TestAppDbContext);
            var candidate = await _testDatabaseHelper.TestAppDbContext.Candidates.FirstAsync();
            await service.AddVoteAsync("test", candidate.Id);

            //Act
            var result = await service.AddVoteAsync("test", candidate.Id);

            //Assert
            Assert.Equal(Messages.UserAlreadyVoted, result);
        }

        [Fact]
        public async Task AddVote_WithEmptyUserName_ShouldReturnErrorMessageAsync()
        {
            //Arrange
            _testDatabaseHelper.InitializeDatabase();
            var service = new VoteService(_testDatabaseHelper.TestAppDbContext);
            var candidate = await _testDatabaseHelper.TestAppDbContext.Candidates.FirstAsync();

            //Act
            var result = await service.AddVoteAsync(string.Empty, candidate.Id);

            //Assert
            Assert.Equal(Messages.NicknameCannotBeEmpty, result);
        }

        [Fact]
        public async Task AddVote_WithNotExistingCandidateId_ShouldReturnErrorMessageAsync()
        {
            //Arrange
            _testDatabaseHelper.InitializeDatabase();
            var service = new VoteService(_testDatabaseHelper.TestAppDbContext);

            //Act
            var result = await service.AddVoteAsync("test", -1);

            //Assert
            Assert.Equal(Messages.CandidateDoesNotExist, result);
        }
    }
}