import axios from "axios";
import { createContext, useEffect, useState } from "react";
import { LyricsApiService } from "../services/lyrics-api.service";

const ConfigurationContext = createContext();

const ConfigurationProvider = ({ children }) => {
  const [configuration, setConfiguration] = useState({ apiUrl: "" });
  const [lyricsApi, setLyricsApi] = useState();

  useEffect(() => {
    const getConfiguration = async () => {
      const result = await axios.get("/configuration.json");

      setConfiguration(result.data);
      setLyricsApi(new LyricsApiService(result.data.apiUrl));
    };
    getConfiguration();
  }, []);

  return (
    <ConfigurationContext.Provider value={{ configuration, lyricsApi }}>
      {configuration === null ? "Loading configuration" : children}
    </ConfigurationContext.Provider>
  );
};

export { ConfigurationProvider, ConfigurationContext };
