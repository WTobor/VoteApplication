using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VoteApplication.Repositories;

namespace VoteApplication.Services
{
    public class ServicesStartup
    {
        public static void Configure(IServiceCollection services)
        {
            services
                .AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("VoteApplicationDatabase"));

            services.AddScoped<AppDbContext>();
        }
    }
}