using System;
using System.Text;
using HtmlAgilityPack;
using HTMLParser.Library;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Console;

namespace HTMLParser.Console
{
    internal class Program
    {   
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((context,services) => services.AddParser()).Build();
            OutputEncoding = Encoding.UTF8;

            DocumentChunker chunker = host.Services.GetRequiredService<DocumentChunker>();
            foreach (var item in chunker.Chunk(new StreamReader(@"E:\Chemistry\Bank\output.html")))
            {
                WriteLine("--------------------------------------");
                WriteLine(item.Content);
                WriteLine("--------------------------------------");
            }
        }
    }
}
