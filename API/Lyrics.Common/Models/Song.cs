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

        public Song(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
