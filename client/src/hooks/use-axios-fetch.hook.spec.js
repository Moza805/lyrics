import React from 'react';
import { useAxiosFetch } from './use-axios-fetch.hook';
import { screen, render, fireEvent } from '@testing-library/react';

const TestComponent = () => {
  const [status, data, promise] = useAxiosFetch();

  const happyPromise = () => {
    const p = new Promise((resolve, reject) => {
      resolve({ data: 'someData!', status: 200 });
    });
    promise(p);
  };

  const errorPromise = () => {
    const p = new Promise((resolve, reject) => {
      reject('sad face :(');
    });
    promise(p);
  };

  return (
    <div>
      <p>Status: {status}</p>
      <p>Data: {(data !== undefined).toString()}</p>
      <button onClick={happyPromise}>execute happy promise</button>
      <button onClick={errorPromise}>execute bad promise</button>
    </div>
  );
};

describe('useFetch hook', () => {
  it('will start on idle', async () => {
    render(<TestComponent />);

    const findStatus = await screen.findByText(/idle/);

    expect(findStatus).toBeDefined();
  });
  it('will change to pending upon axios promise', async () => {
    render(<TestComponent />);

    fireEvent.click(await screen.findByText('execute happy promise'));
    const findStatus = await screen.findByText(/pending/);

    expect(findStatus).toBeDefined();
  });
  it('will change to resolved with data when axios promise complete', async () => {
    render(<TestComponent />);

    fireEvent.click(await screen.findByText('execute happy promise'));
    const findStatus = await screen.findByText(/resolved/);
    const findData = await screen.findByText(/true/);

    expect(findStatus).toBeDefined();
    expect(findData).toBeDefined();
  });
  it('will change to error if an error occured in axios promise', async () => {
    render(<TestComponent />);

    fireEvent.click(await screen.findByText('execute bad promise'));
    const findStatus = await screen.findByText(/error/);
    const findData = await screen.findByText(/false/);

    expect(findStatus).toBeDefined();
    expect(findData).toBeDefined();
  });
});
