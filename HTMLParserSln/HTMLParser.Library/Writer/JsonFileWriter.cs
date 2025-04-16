using HTMLParser.Models;
using Newtonsoft.Json;

namespace HTMLParser.Library.Writer
{
    public class JsonFileWriter : IWriter
    {

        private readonly JsonWriter _writer;
        private readonly JsonSerializer _serializer = new() { Formatting = Formatting.Indented };

        public JsonFileWriter(string path)
        {
            _writer = new JsonTextWriter(new StreamWriter(path));
            InitializeWriter();
        }

        public JsonFileWriter(JsonWriter writer)
        {
            _writer = writer;
            InitializeWriter();
        }

        private void InitializeWriter()
        {
            _writer.WriteStartArray();
        }

        public Task WriteAsync(Question question)
        {
            return Task.Run(() =>
            {
                _serializer.Serialize(_writer, question);
            });
        }


        public async ValueTask DisposeAsync()
        {
            _writer.WriteEndArray();
            await _writer.CloseAsync();
        }
    }
}
