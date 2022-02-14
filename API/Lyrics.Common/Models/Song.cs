using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrics.Common.Models
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Lyrics { get; set; }

        /// <summary>
        /// Number of words in the song
        /// </summary>
        public int WordCount { get => string.IsNullOrEmpty(Lyrics) ? 0 : Lyrics.Replace("\r\n", " ").Split(' ').Count(); }

        public Song(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
