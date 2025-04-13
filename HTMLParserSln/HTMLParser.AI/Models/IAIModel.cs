namespace HTMLParser.AI.Models
{
    public interface IAIModel
    {
        public Task<AIModelResult> GetResponseAsync(string prompt);
        public Task<AIModelResult> GetResponseAsync(string prompt, byte[] fileBytes);
        public IAsyncEnumerable<string> GetStreamingResponseAsync(string prompt);
        public IAsyncEnumerable<string> GetStreamingResponseAsync(string prompt, byte[] fileBytes);
        public Task WriteStreamingResponseAsync(Stream stream, string prompt);
        public Task WriteStreamingResponseAsync(Stream stream, string prompt, byte[] fileBytes);
    }
}
