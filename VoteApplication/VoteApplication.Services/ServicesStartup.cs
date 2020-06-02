using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoteApplication.Repositories;

namespace VoteApplication.Services
{
    public class ServicesStartup
    {
        public const string DefaultDateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("VoteApplicationDatabase"));

            var votingSettings = configuration.GetSection("VotingSettings");
            ValidateResultPublicationDateTime(votingSettings.GetSection("ResultPublicationDateTimeValue").Value);
            services.Configure<VotingSettings>(votingSettings);

            services.AddScoped<AppDbContext>();

            services.AddScoped<CandidateService>();
            services.AddScoped<VoteService>();
            services.AddScoped<ResultService>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
                //solution only for in-memory database provider https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
                dbContext.Database.EnsureCreated();
            }
        }

        private static void ValidateResultPublicationDateTime(string resultPublicationDateTimeValue)
        {
            var correctDateTimeFormat = DateTimeOffset.TryParseExact(resultPublicationDateTimeValue, ServicesStartup.DefaultDateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var dateValue);
            if (!correctDateTimeFormat)
            {
                throw new InvalidOperationException(
                    $"Niepoprawny format daty zmiennej ResultPublicationDateTime w appsettings ({ServicesStartup.DefaultDateTimeFormat})");
            }
        }
    }
}