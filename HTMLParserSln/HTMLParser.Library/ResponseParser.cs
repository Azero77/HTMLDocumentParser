using HTMLParser.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library
{
    public class ResponseParser
    {

        private readonly JsonSerializer _serializer = new() { Formatting = Formatting.Indented };
        public async IAsyncEnumerable<RawQuestion> Parse(string chunk)
        {
            StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(chunk)));
            JsonTextReader jsonReader = new(reader) { SupportMultipleContent = false};

            while (await jsonReader.ReadAsync())
            {
                if (jsonReader.TokenType == JsonToken.StartArray)
                    break;
            }
            while (await jsonReader.ReadAsync())
            {
                if (jsonReader.TokenType == JsonToken.EndArray)
                    yield break;
                else if (jsonReader.TokenType == JsonToken.StartObject)
                {
                    RawQuestion? rawQuestion = _serializer.Deserialize<RawQuestion>(jsonReader);
                    if (rawQuestion is null)
                        yield break;
                    yield return rawQuestion;
                }
            }
            yield break;
        }
    }
}
