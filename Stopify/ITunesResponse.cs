using System.Collections.Generic;

namespace Stopify
{
    public class ITunesResponse
    {
        public int resultCount { get; set; }
        public List<ITunesSong> results { get; set; }
    }

    public class ITunesSong
    {
        public string artworkUrl100 { get; set; }
    }
}
