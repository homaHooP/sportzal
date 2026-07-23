import { api, setAccessToken } from "./api.js";
import { extractErrorMessage } from "./ErrorHandler.js";
import { jwtDecode } from "jwt-decode";

export async function login(email, password) {
    try{
        const { data } = await api.post("/users/login", { email, password });
        console.log(data);
        setAccessToken(data.accessToken);
        return data.accessToken;
    }
    catch(err){
        throw new Error(extractErrorMessage(err),{cause: err});
    }
}

export async function register(email, password) {
    try{
        const { data } = await api.post("/users/register", { email, password });
        setAccessToken(data.accessToken);
        return data.accessToken;
    }
    catch(err){
        throw new Error(extractErrorMessage(err),{cause: err});
    }
}

export async function logout() {
    try{
        await api.post("/users/logout");
        setAccessToken(null);
    }
    catch(err){
        throw new Error(extractErrorMessage(err), { cause: err });
    }
}

export function decodeUser(token) {
    const payload = jwtDecode(token);
    return {
        id: payload.sub,
        email: payload.email,
        fullName: payload.name,
        roles: [].concat(payload.role ?? []),
        profileComplete: payload.profileComplete === "True"
    };
}

export async function setAdditionalInfo(userId, fullName,gender, birthDate) {
    try {
        const { data } = await api.post("/users/additionalinfo", {userId, fullName, gender , birthday: birthDate });
        setAccessToken(data.accessToken);
        return data.accessToken;
    } catch (err) {
        throw new Error(extractErrorMessage(err), { cause: err });
    }
}