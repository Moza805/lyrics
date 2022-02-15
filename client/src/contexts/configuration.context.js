import axios from "axios";
import { createContext, useEffect, useState } from "react";
import { LyricsApiService } from "../services/lyrics-api.service";

// Context used for configuration values
// Currently being misused as a way to pass a service around too
const ConfigurationContext = createContext();

const ConfigurationProvider = ({ children }) => {
  const [configuration, setConfiguration] = useState({ apiUrl: "" });
  const [lyricsApi, setLyricsApi] = useState();

  // On load, fetch configuration values and store in state
  useEffect(() => {
    const getConfiguration = async () => {
      const result = await axios.get("/configuration.json");

      setConfiguration(result.data);
      setLyricsApi(new LyricsApiService(result.data.apiUrl));
    };
    getConfiguration();
  }, []);

  // Return either a loading message or the context wrapped children depending on whether or not we have configuration yet
  return (
    <ConfigurationContext.Provider value={{ configuration, lyricsApi }}>
      {configuration === null ? "Loading configuration" : children}
    </ConfigurationContext.Provider>
  );
};

export { ConfigurationProvider, ConfigurationContext };
