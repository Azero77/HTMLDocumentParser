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
            //start reading from 0
            stream.Position = 0;
            if (stream is null || !stream.CanRead || !stream.CanSeek)
            {
                throw new ArgumentException("Stream Must be readable,seekable");
            }
            int startJsonArray = FintJsonArray(stream);
            if (startJsonArray == -1)
                throw new ArgumentException("Stream Must Contain a json array of objects");

            stream.Position = startJsonArray;
            bool inString = false;
            bool escaped = false;
            bool openColon = false;
            Stack<char> openTags = new();
            Stream correctedStream = new MemoryStream();
            await using StreamWriter writer = new StreamWriter(correctedStream,Encoding.UTF8,leaveOpen:true);
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: false);
            //Adding the first [ open tag for the json array
            int currentByte;
            while ((currentByte = reader.Read())!= -1)
            {
                char c = (char)currentByte;
                if (c == '"' && !escaped) inString = !inString;
                if (inString)
                {
                    openColon = false;
                    if (c == '\\' && !escaped)
                    {
                        escaped = true;
                        await writer.WriteAsync(c);
                        continue;
                    }
                    if (escaped)
                    {
                        if (!IsValidEscapeCharacter(c))
                        {
                            await writer.WriteAsync('\\');
                        }
                        escaped = false;
                    }
                }
                if (!inString)
                {
                    if (c == '{' || c == '[')
                        openTags.Push(c);
                    else if (openTags.Count != 0)
                    {
                        if ((c == '}' && openTags.Peek() == '{') || (c == ']' && openTags.Peek() == '['))
                            openTags.Pop();
                    }
                    if (c == ':')
                        openColon = true;
                }
                await writer.WriteAsync(c);
            }

            if (openColon)
                await writer.WriteAsync("null");
            if (inString)
                await writer.WriteAsync('"');
            
            while (openTags.Count > 0)
            {
                char openTag = openTags.Pop();
                await writer.WriteAsync(openTag == '{' ? '}' : ']');
            }
            await writer.FlushAsync();
            correctedStream.Position = 0;

            return correctedStream;
        }

        private bool IsValidEscapeCharacter(char c)
        {
            return c == '"' || c == '\\' || c == '/' ||
          c == 'b' || c == 'f' || c == 'n' ||
          c == 'r' || c == 't' || c == 'u';
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
