import { useEffect, useRef, useState } from "react";
import { AuthContext } from "./AuthContext.js";
import {
    login as loginRequest,
    register as registerRequest,
    logout as logoutRequest,
    setAdditionalInfo,
    decodeUser
} from "../api/AuthService.js";
import { silentRefresh } from "../api/api.js";

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);
    const didInit = useRef(false);

    useEffect(() => {
        if (didInit.current) return;
        didInit.current = true;
        (async () => {
            const accessToken = await silentRefresh();
            if (accessToken) {
                setUser(decodeUser(accessToken));
            }
            setLoading(false);
        })();
    }, []);

    const login = async (email, password) => {
        const accessToken = await loginRequest(email, password);
        setUser(decodeUser(accessToken));
    };

    const register = async (email, password) => {
        const accessToken = await registerRequest(email, password);
        setUser(decodeUser(accessToken));
    };

    const logout = async () => {
        await logoutRequest();
        setUser(null);
    };

    const setUserInfo = async (userId, fullName,gender, birthDate) => {
        console.log({
            userId,
            fullName,
            birthDate,
            gender
        });
        const accessToken = await setAdditionalInfo(userId, fullName,gender, birthDate);
        setUser(decodeUser(accessToken));
    }

    return (
        <AuthContext.Provider value={{ user, loading, login, register, logout, setUserInfo }}>
            {children}
        </AuthContext.Provider>
    );
}