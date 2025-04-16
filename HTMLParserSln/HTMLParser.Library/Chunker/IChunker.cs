
namespace HTMLParser.Library.Chunker
{
    public interface IChunker
    {
        IEnumerable<DocumentChunk> Chunk(StreamReader file);
    }
}