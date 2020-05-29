using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoteApplication.Repositories;
using VoteApplication.Services;
using Xunit;

namespace VoteApplication.Tests
{
    public class VoteServiceTests
    {
        public VoteServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase("TestVoteApplicationDatabase");
            _context = new AppDbContext(optionsBuilder.Options);
        }

        private readonly AppDbContext _context;

        [Fact]
        public async Task AddVote_WithCorrectData_ShouldAddVoteAsync()
        {
            //Arrange
            var service = new VoteService(_context);
            var candidate = await _context.Candidates.FirstAsync();

            //Act
            var result = await service.AddVoteAsync("test", candidate.Id);

            //Assert
            Assert.True(string.IsNullOrEmpty(result));
        }

        [Fact]
        public async Task AddVote_WithDuplicatedUserName_ShouldReturnUserAlreadyVotedErrorMessageAsync()
        {
            //Arrange
            var service = new VoteService(_context);
            var candidate = await _context.Candidates.FirstAsync();
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
            var service = new VoteService(_context);
            var candidate = await _context.Candidates.FirstAsync();

            //Act
            var result = await service.AddVoteAsync(string.Empty, candidate.Id);

            //Assert
            Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact]
        public async Task AddVote_WithNotExistingCandidateId_ShouldReturnErrorMessageAsync()
        {
            //Arrange
            var service = new VoteService(_context);

            //Act
            var result = await service.AddVoteAsync("test", -1);

            //Assert
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}