﻿namespace HTMLParser.Library
{
    public class DocumentChunk
    {
        public string Content { get; set; } = string.Empty;
        public long StartedToken { get; set; }
        public long TokenLength { get; set; }
    }

}
