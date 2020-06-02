using System.Threading.Tasks;
using VoteApplication.Services;
using Xunit;

namespace VoteApplication.Tests
{
    public class CandidateServiceTests
    {
        public CandidateServiceTests()
        {
            _testHelper = new TestHelper();
        }

        private readonly TestHelper _testHelper;

        [Fact]
        public async Task GetAllCandidates_ShouldReturn3Candidates()
        {
            //Arrange
            await _testHelper.InitializeDatabaseAsync();
            var service = new CandidateService(_testHelper.TestAppDbContext);

            //Act
            var result = await service.GetAllCandidatesAsync();

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