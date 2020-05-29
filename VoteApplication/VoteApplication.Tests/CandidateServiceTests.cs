using Microsoft.EntityFrameworkCore;
using VoteApplication.Repositories;
using VoteApplication.Services;
using Xunit;

namespace VoteApplication.Tests
{
    public class CandidateServiceTests
    {
        private AppDbContext _context;

        public CandidateServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase("TestVoteApplicationDatabase");
            _context = new AppDbContext(optionsBuilder.Options);
        }

        [Fact]
        public void GetAllCandidates_ShouldReturn3Candidates()
        {
            //Arrange
            var service = new CandidateService(_context);

            //Act
            var result = service.GetAllCandidates();

            //Assert
            Assert.Equal(3, result.Count);
            Assert.All(result, model =>
            {
                Assert.False(string.IsNullOrEmpty(model.FullName));
                Assert.True(model.Id > 0);
            });
        }
    }
}