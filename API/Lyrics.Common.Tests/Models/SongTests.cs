using Lyrics.Common.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrics.Common.Tests.Models
{
    public class SongTests
    {
        [Test]
        public void WordCount_ReturnsNumberOfWords()
        {
            // Setup
            var song = new Song(Guid.NewGuid(), "The first song") { Lyrics = "This song has five words" };

            // Test
            var wordCount = song.WordCount;

            // Assert
            Assert.That(wordCount, Is.EqualTo(5));
        }

        [Test]
        public void WordCount_IgnoresNewLineCharacters()
        {
            // Setup
            var song = new Song(Guid.NewGuid(), "The first song") { Lyrics = "This song has\r\nfive words" };

            // Test
            var wordCount = song.WordCount;

            // Assert
            Assert.That(wordCount, Is.EqualTo(5));
        }

        [Test]
        public void WordCount_WorksWithNoLyrics()
        {
            // Setup
            var song = new Song(Guid.NewGuid(), "The first song");

            // Test
            var wordCount = song.WordCount;

            // Assert
            Assert.That(wordCount, Is.EqualTo(0));
        }
    }
}
