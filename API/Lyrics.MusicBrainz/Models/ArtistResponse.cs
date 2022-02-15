using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Lyrics.MusicBrainz.Tests")]
namespace Lyrics.MusicBrainz.Models
{
    internal class ArtistResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Disambiguation { get; set; }
    }
}
