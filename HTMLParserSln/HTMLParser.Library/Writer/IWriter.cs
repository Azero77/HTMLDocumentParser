using HTMLParser.Models;

namespace HTMLParser.Library.Writer
{
    public interface IWriter : IAsyncDisposable
    {
        Task WriteAsync(Question question);
    }
}