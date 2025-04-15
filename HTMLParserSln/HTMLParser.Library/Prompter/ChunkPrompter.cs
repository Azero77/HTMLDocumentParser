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
