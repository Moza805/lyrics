namespace Lyrics.Common.Models
{
    public class Artist
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// E.g. band, person, character.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Description of the artist to help identify the correct one when many have the same name
        /// </summary>
        public string Disambiguation { get; set; }

        public Artist(Guid id, string name, string type, string? disambiguation)
        {
            Id = id;
            Name = name;
            Type = type;
            Disambiguation = disambiguation;
        }
    }
}
