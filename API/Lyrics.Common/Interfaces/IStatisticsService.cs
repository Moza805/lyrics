using Lyrics.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrics.Common.Interfaces
{
    public interface IStatisticsService
    {
        Task<ArtistStatistics> GetStatistics(Guid artistId);
    }
}
