/* eslint-disable import/no-anonymous-default-export */
export default {
  get: jest.fn().mockResolvedValue({ status: 200, data: { apiUrl: "" } }),
};
