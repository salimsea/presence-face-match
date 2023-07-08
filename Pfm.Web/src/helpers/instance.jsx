import axios from "axios";
import Swal from "sweetalert2";

const TOKEN = localStorage.getItem("TOKEN");
export const instanceAuth = axios.create({
  baseURL: import.meta.env.VITE_BASE_URL_API,
  timeout: 60000,
  withCredentials: false,
  headers: {},
});

instanceAuth.interceptors.request.use(
  (request) => {
    const token = TOKEN;
    if (token && token.length > 0)
      request.headers.Authorization = `Bearer ${token}`;
    return request;
  },
  (error) => {
    return Promise.reject(error);
  }
);

instanceAuth.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response) {
      const { data } = error.response;
      if (error.response.status === 401) {
        console.log("401", error.response);

        if (
          data.name === "NotAuthenticated" &&
          data.data &&
          data.data.name === "TokenExpiredError"
        ) {
          Swal.fire(
            "Session Expired",
            `Token expired. Please try login again`,
            "error"
          ).then(() => (window.location = "/"));
          return Promise.reject({
            message: "Token expired. Please try login again.",
          });
        } else {
          Swal.fire(
            "Session Expired",
            `Token expired. Please try login again`,
            "error"
          ).then(() => (window.location = "/"));
          localStorage.clear();
          return Promise.reject({
            message: "Login failed. Please check your email and password!",
          });
        }
      } else {
        Swal.fire(
          "Terjadi Kesalahan",
          `${JSON.stringify(error.message)}`,
          "error"
        );
        let message = data.message || error.message;
        return Promise.reject({ message, raw: data });
      }
    } else if (error.request) {
      // Swal.fire('Error Connection', `There is problem connecting to server. Please check your connection!`, 'error').then(() => window.location = '/')
      // localStorage.clear();
      return Promise.reject({
        message:
          "There is problem connecting to server. Please check your connection!",
      });
    } else {
      return Promise.reject({ message: error.message });
    }
  }
);

export const instance = axios.create({
  baseURL: import.meta.env.VITE_BASE_URL_API,
  timeout: 20000,
  withCredentials: false,
  headers: {},
});
