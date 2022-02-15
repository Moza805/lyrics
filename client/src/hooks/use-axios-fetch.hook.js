import { useState } from "react";

/**
 * List of statuses for this hook
 */
 export const FetchStatuses = {
    idle: 'idle',
    pending: 'pending',
    resolved: 'resolved',
    error: 'error',
  };

  /**
 * Holds a status of a Axios promise, initializes at 'idle'.
 * moves to pending when a promise is being made (AxiosPromise) and finishes on either 'resolved' if success
 * or 'error' if failed
 * @returns status -  holds the status of the axios call
 * @returns data - returned from axios call, or undefined if not in 'resolved' status
 * @param setPromise - function taking in a AxiosPromise and sets status and data based on result
 */
export const useAxiosFetch = () => {
    const [fetchState, setFetchState] = useState({ status: FetchStatuses.idle, data: undefined });
  
    const setPromise = (fetchPromise) => {
      setFetchState({ status: FetchStatuses.pending, data: undefined });
      fetchPromise
        .then((response) => {
          if (response.status === 200) {
            setFetchState({ status: FetchStatuses.resolved, data: response.data });
            return response.data;
          }
        })
        .catch((err) => {
          setFetchState({ status: FetchStatuses.error, data: undefined });
        });
    };
  
    return [fetchState.status, fetchState.data, setPromise];
  };
  