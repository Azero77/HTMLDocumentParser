
namespace HTMLParser.Library.Prompter
{
    public interface IPrompter
    {
        Task<Stream> Answer(DocumentChunk chunk);
    }
}