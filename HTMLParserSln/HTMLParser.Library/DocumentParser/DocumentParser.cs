using HTMLParser.Library.Chunker;
using HTMLParser.Library.Formatter;
using HTMLParser.Library.Messager;
using HTMLParser.Library.Prompter;
using HTMLParser.Library.ResponseParser;
using HTMLParser.Library.Writer;
using HTMLParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.DocumentParser
{
    public class DocumentParser : IDocumentParser
    {
        private readonly IChunker _chunker;
        private readonly IPrompter _prompter;
        private readonly IResponseFormatter _formatter;
        private readonly IWriter _writer = new JsonFileWriter("test.json"); //change
        private readonly IMessanger _messanger;

        public DocumentParser(IChunker chunker,
                              IResponseFormatter formatter,
                              //IWriter writer,
                              IPrompter prompter,
                              IMessanger messanger)
        {
            _chunker = chunker;
            _formatter = formatter;
            //_writer = writer; change
            _prompter = prompter;
            _messanger = messanger;
        }

        public void Dispose()
        {
            _writer.DisposeAsync();
        }

        public async Task Parse(string filePath)
        {
            /*if (!Directory.Exists(filePath))
            {
                throw new ArgumentException("File Not found");
            }*/
            using StreamReader reader = new(filePath);
            foreach (var chunk in _chunker.Chunk(reader))
            {
                await using Stream stream = await _prompter.Answer(chunk);
                await foreach (Question? question in _formatter.Format(stream))
                {
                    _messanger.Message(question);
                    await _writer.WriteAsync(question);
                }
            }
        }
    }
}
