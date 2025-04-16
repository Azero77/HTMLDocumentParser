using HTMLParser.AI.Models;
using HTMLParser.Library.Chunker;
using HTMLParser.Library.DocumentParser;
using HTMLParser.Library.Formatter;
using HTMLParser.Library.Mediator;
using HTMLParser.Library.Messager;
using HTMLParser.Library.Prompter;
using HTMLParser.Library.ResponseParser;
using HTMLParser.Library.Writer;
using HTMLParser.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
            services.AddSingleton<IChunker,DocumentChunker>();
            services.AddSingleton<IAIModel, QuizAIModel>();
            services.AddSingleton<IResponseMediator, ResponseMediator>();
            services.AddSingleton<IResponseFormatter, Formatter.Formatter>();
            services.AddSingleton<IResponseParser, ResponseParser.ResponseParser>();
            services.AddSingleton<IWriter, JsonFileWriter>();
            services.AddSingleton<IPrompter, ChunkPrompter>();
            services.AddSingleton<ResponseValidator>();
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IDocumentParser, DocumentParser.DocumentParser>();
            services.AddSingleton<IMessanger, ConsoleMessanger>();
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
