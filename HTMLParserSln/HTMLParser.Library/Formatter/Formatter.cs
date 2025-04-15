using HTMLParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.Formatter
{
    public class Formatter : IResponseFormatter<RawQuestion,Question>
    {
        private readonly ReverseMarkdown.Converter _converter;

        public Formatter()
        {
            ReverseMarkdown.Config config = new ReverseMarkdown.Config() { GithubFlavored = true};
            _converter = new ReverseMarkdown.Converter(config);
        }

        public Task<Question> Format(RawQuestion raw)
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
