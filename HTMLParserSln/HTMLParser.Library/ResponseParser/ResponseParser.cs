using HTMLParser.Library.Mediator;
using HTMLParser.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.ResponseParser
{
    public class ResponseParser : IResponseParser
    {
        private readonly IResponseMediator _mediator;
        private readonly JsonSerializer _serializer = new() { Formatting = Formatting.Indented };
        public ResponseParser(IResponseMediator mediator)
        {
            _mediator = mediator;
        }

        public async IAsyncEnumerable<RawQuestion> Parse(Stream response)
        {
            StreamReader reader = new StreamReader(response, Encoding.UTF8);
            JsonTextReader jsonReader = new(reader) { SupportMultipleContent = false };
            RawQuestion? rawQuestion = null;
            while (await jsonReader.ReadAsync())
            {
                if (jsonReader.TokenType == JsonToken.StartArray)
                    break;
            }
            while (await jsonReader.ReadAsync())
            {
                if (jsonReader.TokenType == JsonToken.EndArray)
                {
                    _mediator.OnResponseParsed(rawQuestion ?? throw new NullReferenceException("last Question can't be null"));
                    yield break;
                }
                else if (jsonReader.TokenType == JsonToken.StartObject)
                {
                    rawQuestion = _serializer.Deserialize<RawQuestion>(jsonReader);
                    if (rawQuestion is null || anyFieldNull(rawQuestion))
                        continue;
                    yield return rawQuestion;
                }
            }
            yield break;
        }

        private bool anyFieldNull(RawQuestion? rawQuestion)
        {
            return rawQuestion?.QuestionAnswer is null || rawQuestion?.QuestionChoices is null || rawQuestion?.QuestionText is null;
        }
    }
}
