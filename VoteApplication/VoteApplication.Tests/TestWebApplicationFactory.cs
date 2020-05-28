using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VoteApplication.Repositories;
using VoteApplication.Services;

namespace VoteApplication.Tests
{
    public class TestWebApplicationFactory : WebApplicationFactory<ServicesStartup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestVoteApplicationDatabase");
                });
            });
        }
    }
}