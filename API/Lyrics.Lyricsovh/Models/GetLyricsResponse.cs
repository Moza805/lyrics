using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Lyrics.Lyricsovh.Tests")]
namespace Lyrics.Lyricsovh.Models
{
    internal class GetLyricsResponse
    {
        /// <summary>
        /// All the lyrics, lines separated by \r\n
        /// </summary>
        public string Lyrics { get; set; }

        /// <summary>
        /// Line by line lyrics
        /// </summary>
        [JsonIgnore]
        public string[] Lines { get => Lyrics.Split("\r\n"); }
    }
}
