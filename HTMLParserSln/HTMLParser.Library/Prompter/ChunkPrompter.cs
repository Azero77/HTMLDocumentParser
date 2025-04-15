using HTMLParser.AI.Models;
using HTMLParser.Library.Mediator;
using HTMLParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.Prompter
{
    public class ChunkPrompter : IPrompter
    {
        private readonly IAIModel _model;
        private RawQuestion? lastQuestionFromPreviousPrompt = null;

        public ChunkPrompter(IAIModel model, IResponseMediator mediator)
        {
            _model = model;
            mediator.ResponseParsed += OnResponseParsed;
        }

        private void OnResponseParsed(RawQuestion lastQuestion)
        {
            lastQuestionFromPreviousPrompt = lastQuestion;
        }

        public async Task<Stream> Answer(DocumentChunk chunk)
        {
            await using MemoryStream stream = new();
            await using StreamWriter writer = new StreamWriter(stream: stream);

            string prompt = chunk.Content;
            prompt = AddLastQuestionPrompt(prompt);
            await foreach (string? item in _model.GetStreamingResponseAsync(prompt))
            {
                writer.Write(item);
            }
            return stream;
        }
        public static string Getprompt()
        {
            return @"Here is my HTML content. I want you to extract questions from it and write a JSON file using the following structure:

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
"; 
        }
        private string AddLastQuestionPrompt(string prompt)
        {
            if (lastQuestionFromPreviousPrompt is not null)
            {
                string choices = string.Concat(lastQuestionFromPreviousPrompt?.QuestionChoices ?? new List<object?>() { });
                string append = @"\n i am writing the file for you in chunks so this is the last question of the previous chunk: \n";
                append += @$"
            {{
                ""QuestionText"" : ""{lastQuestionFromPreviousPrompt?.QuestionText ?? ""}"",
                ""QuestionChoices"" [{choices}],
                ""QuestionAnswer"" : ""{lastQuestionFromPreviousPrompt?.QuestionAnswer ?? ""}"",
            }}
    ";
                append += @"i want you to update the question i provided in this prompt on the top of of your json array after validating parsing the chunk and see if anything in the chunk can be added to that last question, if the question i provided for you is completed and no need for updating please put it as it is in the top of your json array";
                prompt += append;
            }

            return prompt;
        }
    }
}
