using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoteApplication.Repositories;
using VoteApplication.Services;

namespace VoteApplication.Tests
{
    public class TestHelper
    {
        public AppDbContext TestAppDbContext;

        public async Task InitializeDatabaseAsync()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase($"TestVoteApplicationDatabase{Guid.NewGuid()}");
            TestAppDbContext = new AppDbContext(optionsBuilder.Options);
            await TestAppDbContext.Database.EnsureCreatedAsync();
        }

        public VotingSettings GetSettings(DateTimeOffset dateTimeOffset)
        {
            var dateTimeOffsetValue = dateTimeOffset.ToString(ServicesStartup.DefaultDateTimeFormat, CultureInfo.InvariantCulture);
            return new VotingSettings
            {
                ResultPublicationDateTimeValue = dateTimeOffsetValue
            };
        }
    }
}