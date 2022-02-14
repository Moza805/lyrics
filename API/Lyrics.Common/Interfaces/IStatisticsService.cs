using Lyrics.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrics.Common.Interfaces
{
    /// <summary>
    /// A service for getting stats about an artists catalogue of songs
    /// </summary>
    public interface IStatisticsService
    {
        /// <summary>
        /// Get statistics for a given artist
        /// </summary>
        /// <param name="artistId">Artist ID</param>
        /// <returns></returns>
        Task<ArtistStatistics> GetStatistics(Guid artistId);
    }
}
