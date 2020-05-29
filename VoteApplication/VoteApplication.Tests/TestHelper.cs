using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using VoteApplication.Repositories;
using VoteApplication.Services;

namespace VoteApplication.Tests
{
    public class TestHelper
    {
        public AppDbContext TestAppDbContext;

        public void InitializeDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase($"TestVoteApplicationDatabase{Guid.NewGuid()}");
            TestAppDbContext = new AppDbContext(optionsBuilder.Options);
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