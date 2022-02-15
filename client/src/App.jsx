import { useState, cloneElement } from "react";
import {
  Button,
  Card,
  CardContent,
  Container,
  Grid,
  Link,
  Paper,
  Typography,
} from "@mui/material";

import ArtistStatisticCard from "./components/artist-statistic-card.component";
import { ConfigurationProvider } from "./contexts/configuration.context";

import "./App.css";

const App = () => {
  // Array of Artists statistic card elements to be rendered
  const [searches, setSearches] = useState([
    <Grid item>
      <ArtistStatisticCard />
    </Grid>,
  ]);

  // Add another card
  const handleCheckAnotherArtistClick = () => {
    setSearches([
      ...searches,
      <Grid item>
        <ArtistStatisticCard />
      </Grid>,
    ]);
  };

  return (
    <div className="App">
      <header className="App-header" role="banner">
        <h1>Lyrics Analyser</h1>
      </header>
      <main className="App-main" role="main">
        <ConfigurationProvider>
          <Paper>
            <Container>
              <Grid container spacing={2}>
                {searches.map((Element, key) => cloneElement(Element, { key }))}
                <Grid item>
                  <Card sx={{ width: 500, height: 500 }} elevation={5}>
                    <CardContent>
                      <Button onClick={handleCheckAnotherArtistClick}>
                        Check another artist
                      </Button>
                    </CardContent>
                  </Card>
                </Grid>
              </Grid>
            </Container>
          </Paper>
        </ConfigurationProvider>
      </main>
      <footer className="App-footer" role="contentinfo">
        <Typography component="p">
          Powered with{" "}
          <Link href="https://musicbrainz.org/doc/MusicBrainz_API">
            MusicBrainz
          </Link>{" "}
          and <Link href="https://lyricsovh.docs.apiary.io/#">Lyrics.ovh</Link>
        </Typography>
      </footer>
    </div>
  );
};

export default App;
