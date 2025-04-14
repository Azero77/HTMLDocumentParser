using HTMLParser.AI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library
{
    public static class AddServiceExtension
    {
        public static void AddParser(this IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(sp => Configuration);
            services.AddSingleton<DocumentChunker>();
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IAIModel, QuizAIModel>();
            
        }
        private static IConfiguration? _configuration = null;

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration is null)
                    _configuration = SetConfiguration();
                return _configuration;
            }
        }

        private static IConfiguration SetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config.json")
                .Build();
        }
    }
}
