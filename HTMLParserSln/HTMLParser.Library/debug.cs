using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library
{
    internal static class Debug
    {
        public static string writeStream(Stream stream)
        {
            StringBuilder sb = new();
            long position = stream.Position;
            using (StreamReader reader = new StreamReader(stream, leaveOpen: true))
            {
                sb.Append(reader.ReadToEnd());
            }
            stream.Position = position;
            return sb.ToString();
        }
    }
}
