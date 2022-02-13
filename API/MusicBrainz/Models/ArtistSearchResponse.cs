using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lyrics.MusicBrainz.Models
{
    public class Tag
    {
        public int Count { get; set; }
        public string Name { get; set; }
    }

    public class Artist
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Disambiguation { get; set; }
        public List<Tag> Tags { get; set; }
    }

    public class ArtistSearchResponse
    {
        public DateTime Created { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
        public List<Artist> Artists { get; set; }
    }


}
