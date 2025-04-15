using System;
using System.Runtime.InteropServices;
using System.Text;
using HtmlAgilityPack;
using HTMLParser.AI.Models;
using HTMLParser.Library;
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

            DocumentChunker chunker = host.Services.GetRequiredService<DocumentChunker>();
            IAIModel model = host.Services.GetRequiredService<IAIModel>();
            /*string prompt = ChunkPrompter.Getprompt();
            StreamWriter writer = new("test.txt");

            foreach (var chunk in chunker.Chunk(new StreamReader(@"E:\Chemistry\Bank\output.html")))
            {
                await foreach (var item in model.GetStreamingResponseAsync($"{prompt} \n {chunk.Content}"))
                {
                    writer.Write(item);
                    Write(item);
                }
                writer.Write("-------------------------------");

            }*/
           
        }
    }
}
