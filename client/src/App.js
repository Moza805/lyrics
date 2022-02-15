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
import { useState, cloneElement } from "react";

import "./App.css";
import ArtistStatisticCard from "./components/artist-statistic-card.component";

const App = () => {
  const [searches, setSearches] = useState([
    <Grid item>
      <ArtistStatisticCard />
    </Grid>,
  ]);

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
