import React from "react";
import { useAxiosFetch } from "./use-axios-fetch.hook";
import { screen, render, fireEvent } from "@testing-library/react";

const TestComponent = () => {
  const [status, data, promise] = useAxiosFetch();

  const happyPromise = () => {
    promise(Promise.resolve({ data: "😀", status: 200 }));
  };

  const errorPromise = () => {
    promise(Promise.reject("😢"));
  };

  return (
    <div>
      <p>Status: {status}</p>
      <p>Has data: {data ? "true" : "false"}</p>
      <p>Data: {data}</p>
      <button onClick={happyPromise}>execute happy promise</button>
      <button onClick={errorPromise}>execute bad promise</button>
    </div>
  );
};

describe("useFetch hook", () => {
  it("will start on idle", async () => {
    render(<TestComponent />);

    await screen.findByText(/idle/);
  });
  it("will change to pending upon axios promise", async () => {
    render(<TestComponent />);

    fireEvent.click(await screen.findByText("execute happy promise"));
    await screen.findByText(/pending/);
  });
  it("will change to resolved with data when axios promise complete", async () => {
    render(<TestComponent />);

    fireEvent.click(await screen.findByText("execute happy promise"));
    await screen.findByText(/resolved/);
    await screen.findByText(/true/);
  });
  it("will change to error if an error occured in axios promise", async () => {
    render(<TestComponent />);

    fireEvent.click(await screen.findByText("execute bad promise"));
    await screen.findByText(/error/);
    await screen.findByText(/false/);
  });
});
