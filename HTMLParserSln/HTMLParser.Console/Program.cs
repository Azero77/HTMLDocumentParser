using System;
using System.Runtime.InteropServices;
using System.Text;
using HtmlAgilityPack;
using HTMLParser.AI.Models;
using HTMLParser.Library;
using HTMLParser.Library.Chunker;
using HTMLParser.Library.DocumentParser;
using HTMLParser.Library.Prompter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using static System.Console;

namespace HTMLParser.Console
{
    internal class Program
    {   
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((context,services) => services.AddParser()).Build();
            OutputEncoding = Encoding.UTF8;

            using var parser = host.Services.GetRequiredService<IDocumentParser>();

            await parser.Parse(@"E:\Chemistry\Bank\output.html");
            ReadKey();
        }
    }
}
