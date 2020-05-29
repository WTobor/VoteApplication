using System;
using Microsoft.EntityFrameworkCore;
using VoteApplication.Repositories;

namespace VoteApplication.Tests
{
    public class TestDatabaseHelper
    {
        public AppDbContext TestAppDbContext;

        public void InitializeDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase($"TestVoteApplicationDatabase{Guid.NewGuid()}");
            TestAppDbContext = new AppDbContext(optionsBuilder.Options);
        }
    }
}