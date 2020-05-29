using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoteApplication.Repositories;
using VoteApplication.Services;
using Xunit;

namespace VoteApplication.Tests
{
    public class VoteServiceTests
    {
        private AppDbContext _context;

        [Fact]
        public async Task AddVote_WithCorrectData_ShouldAddVoteAsync()
        {
            //Arrange
            InitializeDatabase();
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
            InitializeDatabase();
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
            InitializeDatabase();
            var service = new VoteService(_context);
            var candidate = await _context.Candidates.FirstAsync();

            //Act
            var result = await service.AddVoteAsync(string.Empty, candidate.Id);

            //Assert
            Assert.Equal(Messages.NicknameCannotBeEmpty, result);
        }

        [Fact]
        public async Task AddVote_WithNotExistingCandidateId_ShouldReturnErrorMessageAsync()
        {
            //Arrange
            InitializeDatabase();
            var service = new VoteService(_context);

            //Act
            var result = await service.AddVoteAsync("test", -1);

            //Assert
            Assert.Equal(Messages.CandidateDoesNotExist, result);
        }

        private void InitializeDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase($"TestVoteApplicationDatabase{Guid.NewGuid()}");
            _context = new AppDbContext(optionsBuilder.Options);
        }
    }
}