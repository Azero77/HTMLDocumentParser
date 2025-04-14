using HTMLParser.AI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library
{
    public class ChunkPrompter
    {
        private readonly IAIModel _model;

        public ChunkPrompter(IAIModel model)
        {
            _model = model;
        }

        public async Task<MemoryStream> Answer(DocumentChunk chunk)
        {
            await using MemoryStream stream = new();
            await using StreamWriter writer = new StreamWriter(stream: stream);

            string prompt = chunk.Content;
            await foreach (string? item in _model.GetStreamingResponseAsync(prompt))
            {
                writer.Write(item);
            }

            return stream;
        }
    }
}
