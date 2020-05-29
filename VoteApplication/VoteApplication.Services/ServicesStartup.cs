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
        private const string DefaultDateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("VoteApplicationDatabase"));

            var key = configuration.GetSection("ResultPublicationDateTime");
            ValidateResultPublicationDateTime(key);
            services.Configure<AppSettings>(key);

            services.AddScoped<AppDbContext>();

            services.AddScoped<CandidateService>();
            services.AddScoped<VoteService>();
            services.AddScoped<ResultService>();
        }

        private static void ValidateResultPublicationDateTime(IConfigurationSection resultPublicationDateTimeKey)
        {
            var correctDateTimeFormat = DateTimeOffset.TryParseExact(resultPublicationDateTimeKey.Value, ServicesStartup.DefaultDateTimeFormat,
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