import axios, { AxiosRequestConfig, AxiosResponse } from 'axios';

const axiosClient = axios.create({
    headers: {
        'Content-Type': 'application/json'
    }
})
// Add a request interceptor
axiosClient.interceptors.request.use(function (config: AxiosRequestConfig) {
    return config;
}, function (error) {
    return Promise.reject(error);
});

// Add a response interceptor
axiosClient.interceptors.response.use(function (response: AxiosResponse) {
    return response.data;
}, function (error) {
    return Promise.reject(error);
});

export { axiosClient };
