using Lyrics.Lyricsovh.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrics.Lyricsovh.Tests.Models
{
    public class GetLyricsResponseTests
    {
        [Test]
        public void LyricsSplitsOnNewline()
        {
            // Arrange
            var lyricsUnderTest = new GetLyricsResponse
            {
                Lyrics = "I had plans\r\nbut sure, OK then\r\nI guess not"
            };

            // Assert
            Assert.That(lyricsUnderTest.Lines.Count, Is.EqualTo(3));
            Assert.AreEqual("I had plans", lyricsUnderTest.Lines[0]);
            Assert.AreEqual("but sure, OK then", lyricsUnderTest.Lines[1]);
            Assert.AreEqual("I guess not", lyricsUnderTest.Lines[2]);
        }

        [Test]
        public void LyricsSplitsWhenNoContent()
        {
            // Arrange
            var lyricsUnderTest = new GetLyricsResponse();

            // Assert
            Assert.That(lyricsUnderTest.Lines.Count, Is.EqualTo(0));
        }

        [Test]
        public void LyricsSplitsWhenOnlyOneLine()
        {

        }
    }
}
