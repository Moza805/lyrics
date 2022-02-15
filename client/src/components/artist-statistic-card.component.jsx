import axios from "axios";
import { useState } from "react";
import {
  Autocomplete,
  Box,
  Button,
  Card,
  CardContent,
  CardHeader,
  CircularProgress,
  ListItemText,
  Modal,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import Boxplot, { computeBoxplotStats } from "react-boxplot";

import { useAxiosFetch, FetchStatuses } from "../hooks/use-axios-fetch.hook";

const modalStyle = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  maxHeight: "90%",
  p: 4,
  overflow: "scroll",
};

const ArtistStatisticCard = () => {
  const [artistSearchTerm, setArtistSearchTerm] = useState("");
  const [artistSearchStatus, artistSearchData, setArtistSearchPromise] =
    useAxiosFetch();
  const [
    artistStatisticStatus,
    artistStatisticData,
    setArtistStatisticPromise,
  ] = useAxiosFetch();

  const [modalOpen, setModalOpen] = useState(false);
  const [modalContent, setModalContent] = useState("😜");

  const handleSearchClick = (e) => {
    e.preventDefault();
    setArtistSearchPromise(
      axios
        .get(
          `https://localhost:7186/SearchArtistsByName/${encodeURIComponent(
            artistSearchTerm
          )}`
        )
        .then((response) => {
          response.data = response.data.map((artist) => {
            const label =
              artist.disambiguation === null
                ? artist.name
                : `${artist.name} (${artist.disambiguation})`;

            return { label, id: artist.id };
          });

          return response;
        })
    );
  };

  const handleArtistChange = (event, newValue) => {
    setArtistStatisticPromise(
      axios.get(
        `https://localhost:7186/GetStatisticsForArtist/${encodeURIComponent(
          newValue.id
        )}`
      )
    );
  };

  const openModalWithContent = (content) => {
    setModalContent(content);
    setModalOpen(true);
  };

  const handleModalClose = () => {
    setModalOpen(false);
  };

  return (
    <>
      <Card sx={{ width: 500, height: 500 }} elevation={5}>
        {artistStatisticStatus === FetchStatuses.idle && (
          <CardContent>
            <form>
              <TextField
                variant="standard"
                label="Artist"
                placeholder="Enter an artist's name"
                value={artistSearchTerm}
                onChange={(e) => setArtistSearchTerm(e.target.value)}
                autoComplete="off"
                autoFocus={true}
              ></TextField>
              <Button
                type="submit"
                variant="contained"
                onClick={handleSearchClick}
              >
                {artistSearchStatus === FetchStatuses.pending
                  ? "Searching"
                  : "Search"}
              </Button>
            </form>
            {artistSearchStatus === FetchStatuses.resolved && (
              <Autocomplete
                options={artistSearchData}
                renderInput={(params) => (
                  <TextField {...params} label="Artists" />
                )}
                onChange={handleArtistChange}
              />
            )}
          </CardContent>
        )}
        {artistStatisticStatus === FetchStatuses.pending && (
          <CardContent>
            <CircularProgress color="inherit" />
          </CardContent>
        )}
        {artistStatisticStatus === FetchStatuses.resolved && (
          <>
            <CardHeader
              title={artistStatisticData.artist.name}
              subheader={artistStatisticData.artist.disambiguation}
            />
            <CardContent>
              <Stack direction="row">
                <ListItemText
                  primary="Longest song"
                  secondary={
                    <Button
                      variant="text"
                      onClick={() =>
                        openModalWithContent(
                          <>
                            <Typography variant="h3">
                              {artistStatisticData.longestSong.title}
                            </Typography>
                            <Typography variant="body1">
                              {artistStatisticData.longestSong.lyrics
                                .split(/\r?\n/)
                                .map((line, key) => (
                                  <span key={key}>
                                    {line}
                                    <br />
                                  </span>
                                ))}
                            </Typography>
                          </>
                        )
                      }
                    >
                      {artistStatisticData.longestSong?.wordCount || "0"}
                    </Button>
                  }
                />
                <ListItemText
                  primary="Shortest song"
                  secondary={
                    <Button
                      variant="text"
                      onClick={() =>
                        openModalWithContent(
                          artistStatisticData.shortestSong.lyrics
                            .split(/\r?\n/)
                            .map((line) => (
                              <Typography variant="body1">{line}</Typography>
                            ))
                        )
                      }
                    >
                      {artistStatisticData.shortestSong?.wordCount || "0"}
                    </Button>
                  }
                />
              </Stack>
              <Stack direction="row">
                <ListItemText
                  primary="Mean"
                  secondary={artistStatisticData.averageWordsPerSong.toFixed(2)}
                />
                <ListItemText
                  primary="Variance"
                  secondary={artistStatisticData.variance.toFixed(2)}
                />
                <ListItemText
                  primary="Standard deviation"
                  secondary={artistStatisticData.standardDeviation.toFixed(2)}
                />
              </Stack>
              {artistStatisticData.songs.length > 0 && (
                <Boxplot
                  width={400}
                  height={25}
                  orientation="horizontal"
                  min={artistStatisticData.shortestSong?.wordCount || 0}
                  max={artistStatisticData.longestSong?.wordCount || 0}
                  stats={computeBoxplotStats(
                    artistStatisticData.songs
                      .filter((song) => song.wordCount > 0)
                      .map((song) => song.wordCount)
                  )}
                />
              )}
            </CardContent>
          </>
        )}
      </Card>
      <Modal open={modalOpen} onClose={handleModalClose}>
        <Box sx={modalStyle}>{modalContent}</Box>
      </Modal>
    </>
  );
};

export default ArtistStatisticCard;
