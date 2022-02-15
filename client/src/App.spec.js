import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import App from "./App";

jest.mock("./components/artist-statistic-card.component", () => () => (
  <div>A lyrics card</div>
));

it("shows the header, footer and a blank starting card", async () => {
  render(<App />);
  await screen.findByText(/Lyrics Analyser/);
  await screen.findByText(/A lyrics card/);
  await screen.findByRole(/contentinfo/);
});

it("adds another card when you click to add one", async () => {
  render(<App />);
  const addCardButton = await screen.findByText(/Check another artist/);

  fireEvent.click(addCardButton);

  await waitFor(() => {
    const cards = screen.queryAllByText(/A lyrics card/);
    expect(cards.length).toBe(2);
  });
});
