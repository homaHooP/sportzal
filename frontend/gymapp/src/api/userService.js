import { api } from "./api.js";

export async function getUserById(id) {
    const { data } = await api.get(`/users/${id}`);
    return data;
}