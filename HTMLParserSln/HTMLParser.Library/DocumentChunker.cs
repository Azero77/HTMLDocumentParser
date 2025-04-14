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
        private long cursor = 0; //pointer to the token selected while reading
        public DocumentChunker(IConfiguration configuration)
        {
            _tokenLimit = int.Parse(configuration["TokenLimit"] ?? string.Empty);
        }

        public IEnumerable<DocumentChunk> Chunk(StreamReader file)
        {
            HtmlDocument document = LoadDocument(file);
            HtmlNode? body = document.DocumentNode;
            Console.WriteLine($"Document-Length:{document.Text.Length}");
            if(body is null)
                throw new InvalidDataException("This format is not provided");
            return Chunk(body);
        }
        private IEnumerable<DocumentChunk> Chunk(HtmlNode root)
        {
            //Node to return when adding chunks to make a document chunk
            HtmlNode result_node = new HtmlDocument().CreateElement("div");
            int token_counter = 0;
            foreach (HtmlNode node in root.ChildNodes)
            {
                int node_token = ApproximateTokenCount(node);
                if (node_token == 0)
                    continue;
                if (token_counter + node_token > _tokenLimit)
                {
                    if (result_node.ChildNodes.Count() > 0)
                    {
                        DocumentChunk returnedChunk =  new DocumentChunk() { Content = result_node.OuterHtml,StartedToken = cursor - token_counter, TokenLength = token_counter }; //add logic for staring and ending token
                        yield return returnedChunk;
                        token_counter = 0;
                        result_node = new HtmlDocument().CreateElement("div");
                    }
                    else
                    {
                        //make chunking to child nodes in the child node of the body
                        foreach (DocumentChunk chunk in Chunk(node))
                        {
                            yield return chunk;
                        }
                        continue;
                    }
                }
                result_node.AppendChild(node.Clone());

                cursor += node_token;
                token_counter += node_token;
            }
            if (result_node.ChildNodes.Any())
            {
                DocumentChunk chunk = new DocumentChunk()
                {
                    Content = result_node.InnerHtml,
                    StartedToken = cursor - token_counter,
                    TokenLength = token_counter
                };
                yield return chunk;
            }
                
        }
        private static HtmlDocument LoadDocument(StreamReader file)
        {
            HtmlDocument document = new HtmlDocument();
            document.Load(file);
            return document;
        }

        private static int ApproximateTokenCount(HtmlNode node)
        {
            return (int) Math.Ceiling((double)(node.OuterLength / 4));
        }
    }

}
