using HTMLParser.Library.ResponseParser;
using HTMLParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.Formatter
{
    public class Formatter : IResponseFormatter
    {
        private readonly ReverseMarkdown.Converter _converter;
        private readonly IResponseParser _parser;

        public Formatter(IResponseParser parser)
        {
            ReverseMarkdown.Config config = new ReverseMarkdown.Config() { GithubFlavored = true };
            _converter = new ReverseMarkdown.Converter(config);
            _parser = parser;
        }

        public async IAsyncEnumerable<Question> Format(Stream prompt)
        {
            await foreach (var rawQuestion in _parser.Parse(prompt))
            {
                yield return await FormatRawQuestion(rawQuestion);
            }
        }

        private Task<Question> FormatRawQuestion(RawQuestion raw)
        {
            var questionText = _converter.Convert(raw.QuestionText?.ToString() ?? string.Empty);
            var questionChoices = raw.QuestionChoices
                .Select(choice => _converter.Convert(choice?.ToString() ?? string.Empty));
            var questionAnswer = _converter.Convert(raw.QuestionAnswer?.ToString() ?? string.Empty);

            return Task.FromResult(new Question()
            {
                QuestionAnswer = questionAnswer,
                QuestionChoices = questionChoices,
                QuestionText = questionText
            });
        }
    }
}
