using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Lyrics.MusicBrainz.Tests")]
namespace Lyrics.MusicBrainz.Models
{
    internal class Tag
    {
        public int Count { get; set; }
        public string Name { get; set; }
    }

    internal class Artist
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Disambiguation { get; set; }
        public List<Tag> Tags { get; set; }
    }

    internal class ArtistSearchResponse
    {
        public DateTime Created { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
        public List<Artist> Artists { get; set; }
    }


}
