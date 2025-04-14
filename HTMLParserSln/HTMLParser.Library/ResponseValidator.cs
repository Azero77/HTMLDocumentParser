using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library
{

    /// <summary>
    /// Encapsulate the logic of dealing with improper response and correct it and throw exception if needed
    /// </summary>
    public class ResponseValidator
    {
        /// <summary>
        /// Handle the initial stream and modify it in way that can be used to fit the program
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<Stream> Validate(Stream stream)
        {
            if (stream is null || !stream.CanRead || !stream.CanSeek || !stream.CanWrite)
            {
                throw new ArgumentException("Stream Must be readable,seekable,writable");
            }
            int startJsonArray = FintJsonArray(stream);
            if (startJsonArray == -1)
                throw new ArgumentException("Stream Must Contain a json array of objects");

            stream.Position = startJsonArray;
            bool inString = false;
            bool escaped = false;
            Stack<char> openTags = new();
            //Adding the first [ open tag for the json array
            Stream correctedStream = new MemoryStream();
            await using StreamWriter writer = new StreamWriter(correctedStream,new UTF8Encoding(false),leaveOpen:true);
            openTags.Push('[');
            writer.Write('[');
            int currentByte;

            while ((currentByte = stream.ReadByte())!= -1)
            {
                char c = (char)currentByte;
                writer.Write(c);
                await Console.Out.WriteAsync(c);
                if (c == '"' && !escaped) inString = !inString;
                escaped = c == '\\' && !escaped;
                if (!inString)
                {
                    if (c == '{' || c == '[')
                        openTags.Push(c);
                    else if (openTags.Count != 0)
                    {
                        if ((c == '}' && openTags.Peek() == '{') || (c == ']' && openTags.Peek() == '['))
                            openTags.Pop();
                    }
                }
            }
            if (inString)
                writer.Write('"');

            while (openTags.Count > 0)
            {
                char openTag = openTags.Pop();
                await writer.WriteAsync(openTag == '{' ? '}' : ']');
            }
            await writer.FlushAsync();
            correctedStream.Position = 0;

            return correctedStream;
        }

        private int FintJsonArray(Stream stream)
        {
            stream.Position = 0;
            int currentByte;
            while ((currentByte = stream.ReadByte()) != -1)
            {
                if ((char)currentByte == '[')
                    return (int)(stream.Position - 1);
            }
            return -1;
        }
    }
}
