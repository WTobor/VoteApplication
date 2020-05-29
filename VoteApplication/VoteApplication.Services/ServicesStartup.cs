using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoteApplication.Repositories;

namespace VoteApplication.Services
{
    public class ServicesStartup
    {
        public const string DefaultDateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("VoteApplicationDatabase"));

            var key = configuration.GetSection("VotingSettings");
            ValidateResultPublicationDateTime(key.GetSection("ResultPublicationDateTimeValue").Value);
            services.Configure<VotingSettings>(key);

            services.AddScoped<AppDbContext>();

            services.AddScoped<CandidateService>();
            services.AddScoped<VoteService>();
            services.AddScoped<ResultService>();
        }

        private static void ValidateResultPublicationDateTime(string resultPublicationDateTimeValue)
        {
            var correctDateTimeFormat = DateTimeOffset.TryParseExact(resultPublicationDateTimeValue, ServicesStartup.DefaultDateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTimeOffset dateValue);
            if (!correctDateTimeFormat)
            {
                throw new InvalidOperationException(
                    $"Niepoprawny format daty zmiennej ResultPublicationDateTime w appsettings ({ServicesStartup.DefaultDateTimeFormat})");
            }
        }
    }
}