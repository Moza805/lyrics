using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Lyrics.MusicBrainz.Tests")]
namespace Lyrics.MusicBrainz.Models
{
    internal class Relation
    {
        public string Type { get; set; }
        public Artist Artist { get; set; }
    }

    internal class Work
    {
        public Guid Id { get; set; }
        public List<Relation> Relations { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public string Disambiguation { get; set; }
        public string Title { get; set; }
        public List<string> Languages { get; set; }
    }

    internal class SongListResponse
    {
        public List<Work> Works { get; set; }
    }


}
