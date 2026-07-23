import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "./Context/AuthContext.js";
import Spinner from "./components/Spinner";

export function ProtectedRoute({ children }) {
    const { user, loading } = useAuth();
    const location = useLocation();

    if (loading) return <Spinner size="lg" fullScreen />;
    if (!user) return <Navigate to="/" replace />;

    if (!user.profileComplete && location.pathname !== "/complete-profile") {
        return <Navigate to="/complete-profile" replace />;
    }

    if (user.profileComplete && location.pathname === "/complete-profile") {
        return <Navigate to="/dashboard" replace />;
    }

    return children;
}