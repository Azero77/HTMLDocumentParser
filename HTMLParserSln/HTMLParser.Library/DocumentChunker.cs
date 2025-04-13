using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library
{
    public class DocumentChunker
    {
        private readonly int _tokenLimit;
        public DocumentChunker(IConfiguration configuration)
        {
            _tokenLimit = int.Parse(configuration["TokenLimit"] ?? string.Empty);
        }

        public IEnumerable<DocumentChunk> Chunk(StreamReader file)
        {
            HtmlDocument document = LoadDocument(file);
            HtmlNode? body = document.DocumentNode;
            if(body is null)
                yield break;
            HtmlNode result_node = new HtmlDocument().CreateElement("div");

            int token_counter = 0;
            foreach (HtmlNode node in body.ChildNodes)
            {
                token_counter += ApproximateTokenCount(node);
                if (token_counter > _tokenLimit)
                {
                    if (result_node.ChildNodes.Count() > 0)
                        yield return new DocumentChunk() { Content = result_node.OuterHtml }; //add logic for staring and ending token
                    else
                    {
                        //make chunking to child nodes in the child node of the body
                        foreach (DocumentChunk chunk in LargeChunkHandler(node))
                        {
                            yield return chunk;
                        }
                    }
                }
                else
                    result_node.ChildNodes.Add(node);
            }

            if (result_node.ChildNodes.Any())
                yield return new DocumentChunk()
                {
                    Content = result_node.InnerHtml
                };
        }


        private IEnumerable<DocumentChunk> LargeChunkHandler(HtmlNode node)
        {
            throw new NotImplementedException();
        }

        private static HtmlDocument LoadDocument(StreamReader file)
        {
            HtmlDocument document = new HtmlDocument();
            document.Load(file);
            return document;
        }

        private static int ApproximateTokenCount(HtmlNode node)
        {
            return node.OuterLength / 4;
        }
    }

    public class DocumentChunk
    {
        public string Content { get; set; } = string.Empty;
        public long StartedToken { get; set; }
        public long EndToken { get; set; }
    }
}
