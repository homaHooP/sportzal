import axios from "axios";

export const api = axios.create({
    baseURL: "http://localhost:5213/api",
    withCredentials: true,
});

let accessToken = null;

export const setAccessToken = (token) => {
    accessToken = token;
};

api.interceptors.request.use((config) => {
    if (accessToken) {
        config.headers.Authorization = `Bearer ${accessToken}`;
    }
    return config;
});

let isRefreshing = false;
let pendingQueue = [];

const processQueue = (error, token = null) => {
    pendingQueue.forEach(({ resolve, reject }) => {
        if (error) reject(error);
        else resolve(token);
    });
    pendingQueue = [];
};

export async function silentRefresh() {
    try {
        const { data } = await axios.post(
            "http://localhost:5213/api/users/refreshtoken",
            {},
            { withCredentials: true }
        );
        setAccessToken(data.accessToken);
        return data.accessToken;
    } catch {
        setAccessToken(null);
        return null;
    }
}

api.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        if (error.response?.status !== 401 || originalRequest._retry) {
            return Promise.reject(error);
        }

        if (isRefreshing) {
            // якщо refresh вже виконується — чекаємо його результату
            return new Promise((resolve, reject) => {
                pendingQueue.push({ resolve, reject });
            }).then((token) => {
                originalRequest.headers.Authorization = `Bearer ${token}`;
                return api(originalRequest);
            });
        }

        originalRequest._retry = true;
        isRefreshing = true;

        try {
            const { data } = await axios.post(
                "http://localhost:5213/api/users/refreshtoken",
                {},
                { withCredentials: true }
            );

            setAccessToken(data.accessToken);
            processQueue(null, data.accessToken);

            originalRequest.headers.Authorization = `Bearer ${data.accessToken}`;
            return api(originalRequest);
        } catch (refreshError) {
            processQueue(refreshError, null);
            setAccessToken(null);
            return Promise.reject(refreshError);
        } finally {
            isRefreshing = false;
        }
    }
);