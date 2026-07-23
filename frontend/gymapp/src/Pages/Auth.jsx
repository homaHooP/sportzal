import { useState } from "react";
import { useAuth } from "../Context/AuthContext.js";
import { useNavigate } from "react-router-dom";
import Spinner from "../components/Spinner";
import { Navigate } from "react-router-dom";

export default function AuthPage() {
    const [mode, setMode] = useState("login");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const isLogin = mode === "login";
    const { login, register, user, loading } = useAuth();
    const navigate = useNavigate();

    if (loading) return <Spinner size="lg" fullScreen />;
    if (user) return <Navigate to="/dashboard" replace />;

    const validate = () => {
        if (!email.trim()) return "Email is required";
        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) return "Enter a valid email";
        if (!password) return "Password is required";
        if (password.length < 8) return "Password must be at least 8 characters";
        return "";
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const newError = validate();
        setError(newError);

        if (newError !== "") return;
        try{
            if (mode === "register") { await register(email, password); }
            else { await login(email, password); }
            navigate("/dashboard");
        }
        catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="min-h-screen bg-back flex items-center justify-center px-6 font-sans">
            <div className="w-full max-w-sm">
                <h1 className="font-hero text-2xl text-text mb-1">
                    {isLogin ? "Log in" : "Sign up"}
                </h1>
                <p className="text-sm text-text-calm mb-8">
                    {isLogin ? "Enter your email and password to continue" : "Create an account to get started"}
                </p>
                <form onSubmit={handleSubmit} className="space-y-6">
                    <div>
                        <label htmlFor="email" className="block text-xs text-text-calm mb-1.5">Email</label>
                        <input id="email" type="email" value={email} onChange={(e) => setEmail(e.target.value)} required className="w-full border-b bg-back-light p-2 text-text outline-none focus:border-accent transition-colors"/>
                    </div>

                    <div>
                        <label htmlFor="password" className="block text-xs text-text-calm mb-1.5">Password</label>
                        <input id="password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required minLength={8} className="w-full border-b bg-back-light p-2 text-text font-hero outline-none focus:border-accent transition-colors"/>
                    </div>

                    {error && (
                        <p className="text-xs text-accent-2">{error}</p>
                    )}

                    <button type="submit" className="w-full bg-accent text-back py-2.5 rounded-md text-sm mt-2 hover:opacity-90 transition-opacity cursor-pointer">
                        {isLogin ? "Log in" : "Sign up"}
                    </button>
                </form>

                <p className="text-sm text-text-calm mt-8 text-center">{isLogin ? "Don't have an account? " : "Already have an account? "}
                    <button type="button" onClick={() => setMode(isLogin ? "register" : "login")} className="text-accent hover:underline cursor-pointer">
                        {isLogin ? "Sign up" : "Log in"}
                    </button>
                </p>
            </div>
        </div>
    );
}
