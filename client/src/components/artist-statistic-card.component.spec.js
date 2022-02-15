import { fireEvent, render, screen } from "@testing-library/react";
import axios from "axios";
import ArtistStatisticCard from "./artist-statistic-card.component";

describe("Artist statistic card", () => {
  beforeEach(() => {
    axios.get
      .mockResolvedValueOnce({
        status: 200,
        data: [
          {
            id: "7dc8f5bd-9d0b-4087-9f73-dc164950bbd8",
            name: "Queens of the Stone Age",
            type: "Group",
            disambiguation: null,
          },
          {
            id: "5335debb-e4f3-463f-8a4f-215d858be085",
            name: "Stoneage",
            type: null,
            disambiguation: null,
          },
        ],
      })
      .mockResolvedValueOnce({
        status: 200,
        data: {
          artist: {
            id: "7dc8f5bd-9d0b-4087-9f73-dc164950bbd8",
            name: "Queens of the Stone Age",
            type: "Group",
            disambiguation: "",
          },
          songs: [
            {
              id: "02b0200f-5a13-4196-b01f-debb64a6f6e5",
              title: "A Song for the Deaf",
              lyrics: "",
              wordCount: 128,
            },
            {
              id: "0dbb01c2-4126-4f7c-bd21-9b81f99e92d0",
              title: "Fairweather Friends",
              lyrics: "",
              wordCount: 111,
            },
            {
              id: "15d29c4e-9938-318f-94eb-46b73b88700d",
              title: "Better Living Through Chemistry",
              lyrics: "",
              wordCount: 91,
            },
          ],
          longestSong: {
            id: "548d45ab-6afa-4358-a90c-9ab067b4d508",
            title: "Domesticated Animals",
            lyrics: "Pretend this is 235 words",
            wordCount: 235,
          },
          shortestSong: {
            id: "5795f106-29b7-4224-a688-5edd0ea06d23",
            title: "Hispanic Impressions",
            lyrics: "[Instrumental]",
            wordCount: 1,
          },
          averageWordsPerSong: 121.80701754385964,
          variance: 2031.8399507540782,
          standardDeviation: 45.07593538412795,
        },
      });
  });

  afterEach(() => {
    axios.get.mockReset();
  });

  it("should initialise with search input", async () => {
    render(<ArtistStatisticCard />);

    await screen.findByLabelText(/Artist/);
  });

  it("should fetch a set of artists when they are searched for", async () => {
    // Test
    render(<ArtistStatisticCard />);

    const artistInput = await screen.findByLabelText(/Artist/);
    fireEvent.change(artistInput, { target: { value: "QOTSA" } });

    const searchButton = await screen.findByText(/Search/);
    fireEvent.click(searchButton);

    await screen.findByLabelText(/Artists/);

    // Assert
    expect(axios.get).toHaveBeenCalledTimes(1);
  });

  it("should show lyrics when you click on longest song", async () => {
    // Test
    render(<ArtistStatisticCard />);

    const artistInput = await screen.findByLabelText(/Artist/);
    fireEvent.change(artistInput, { target: { value: "QOTSA" } });

    const searchButton = await screen.findByText(/Search/);
    fireEvent.click(searchButton);

    const dropdownButton = await screen.findByTestId(/ArrowDropDownIcon/);
    fireEvent.click(dropdownButton);

    const options = await screen.findAllByRole("option");
    fireEvent.click(options[1]);

    await screen.findByText(/Longest song/);
    await screen.findByText(/235/);
    // Assert
    expect(axios.get).toHaveBeenCalledTimes(2);
  });

  it("should show stats when an artist is selected", async () => {
    // Test
    render(<ArtistStatisticCard />);

    const artistInput = await screen.findByLabelText(/Artist/);
    fireEvent.change(artistInput, { target: { value: "QOTSA" } });

    const searchButton = await screen.findByText(/Search/);
    fireEvent.click(searchButton);

    const dropdownButton = await screen.findByTestId(/ArrowDropDownIcon/);
    fireEvent.click(dropdownButton);

    const options = await screen.findAllByRole("option");
    fireEvent.click(options[1]);

    await screen.findByText(/Longest song/);
    const lyricsButton = await screen.findByText(/235/);

    fireEvent.click(lyricsButton);

    // Assert
    await screen.findByText(/Pretend this is 235 words/);
  });

  it("should show an error message when it gets a bad artist search response", async () => {
    axios.get.mockReset();

    axios.get.mockRejectedValueOnce({
      status: 500,
      data: { error: "Problems!" },
    });

    // Test
    render(<ArtistStatisticCard />);

    const artistInput = await screen.findByLabelText(/Artist/);
    fireEvent.change(artistInput, { target: { value: "QOTSA" } });

    const searchButton = await screen.findByText(/Search/);
    fireEvent.click(searchButton);

    // Assert
    await screen.findByText(
      /Ahh shucks, something went wrong. Please refresh and try again./
    );
    expect(axios.get).toHaveBeenCalledTimes(1);
  });

  it("should show an error message when it gets a bad statistics response", async () => {
    axios.get.mockReset();

    axios.get
      .mockResolvedValueOnce({
        status: 200,
        data: [
          {
            id: "7dc8f5bd-9d0b-4087-9f73-dc164950bbd8",
            name: "Queens of the Stone Age",
            type: "Group",
            disambiguation: null,
          },
          {
            id: "5335debb-e4f3-463f-8a4f-215d858be085",
            name: "Stoneage",
            type: null,
            disambiguation: null,
          },
        ],
      })
      .mockRejectedValueOnce({
        status: 500,
        data: { error: "Problems!" },
      });

    // Test
    render(<ArtistStatisticCard />);

    const artistInput = await screen.findByLabelText(/Artist/);
    fireEvent.change(artistInput, { target: { value: "QOTSA" } });

    const searchButton = await screen.findByText(/Search/);
    fireEvent.click(searchButton);

    const dropdownButton = await screen.findByTestId(/ArrowDropDownIcon/);
    fireEvent.click(dropdownButton);

    const options = await screen.findAllByRole("option");
    fireEvent.click(options[1]);

    // Assert
    await screen.findByText(
      /Ahh shucks, something went wrong. Please refresh and try again./
    );
    expect(axios.get).toHaveBeenCalledTimes(2);
  });
});
