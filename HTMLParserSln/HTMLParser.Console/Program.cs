using System;
using System.Runtime.InteropServices;
using System.Text;
using HtmlAgilityPack;
using HTMLParser.AI.Models;
using HTMLParser.Library;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            string prompt = @"Here is my HTML content. I want you to extract questions from it and write a JSON file using the following structure:

[
  {
    ""QuestionText"": ""HTML string of the question"",
    ""QuestionChoices"": [""HTML string of choice A"", ""HTML string of choice B"", ""...""],
    ""QuestionAnswer"": ""HTML string of the answer""
  }
]

Instructions:
- The values of all fields must be raw HTML strings from the input, including any inline formatting like <b>, <i>, <u>, <code>, <sup>, <sub>, etc.
- LaTeX math should be preserved and properly escaped using double backslashes. Inline math must be in \\\\( ... \\\\), block math in \\\\[ ... \\\\].
- Strings must be fully escaped for JSON (e.g., double quotes, backslashes).
- If you encounter incomplete or ambiguous questions, leave the corresponding field(s) blank.
- The response **must** be only the JSON file — no explanations or comments.
- The JSON must be syntactically valid, fully closed, and well-formatted, so it can be parsed directly.

The HTML may be truncated, so partial questions are acceptable. Your reasoning is needed to identify and extract what qualifies as a question and its parts. Focus on structure and correctness.
in the last element please close any open bracket or object so the json can be valid
";
            StreamWriter writer = new("test.txt") { AutoFlush = true };

            foreach (var chunk in chunker.Chunk(new StreamReader(@"E:\Chemistry\Bank\output.html")))
            {
                await foreach (var item in model.GetStreamingResponseAsync($"{prompt} \n {chunk.Content}"))
                {
                    writer.Write(item);
                    Write(item);
                }
                
                writer.Write("-------------------------------\n");
                await Task.Delay(10000);

            }
        }
    }
}
