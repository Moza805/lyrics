import axios from "axios";

export class LyricsApiService {
  _baseUrl;
  constructor(baseUrl) {
    this._baseUrl = baseUrl;
  }

  SearchArtistsByNameAsync = async (name) =>
    axios.get(
      `${this._baseUrl}/SearchArtistsByName/${encodeURIComponent(name)}`
    );

  GetStatisticsForArtistAsync = async (artistId) =>
    axios.get(
      `${this._baseUrl}/GetStatisticsForArtist/${encodeURIComponent(artistId)}`
    );
}
