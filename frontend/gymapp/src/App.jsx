import './App.css'
import {BrowserRouter as Router,Routes,Route} from "react-router-dom";
import { AuthProvider } from "./Context/AuthProvider.jsx";
import { ProtectedRoute} from "./Routes.jsx";
import AuthPage from "./Pages/Auth.jsx";
import Dashboard from "./Pages/Dashboard.jsx";
import ProfilePage from "./Pages/Profile.jsx";
import CompleteProfilePage from "./Pages/CompleteProfilePage.jsx";

function App() {
  return (
    <>
        <AuthProvider>
            <Router>
                <Routes>
                    <Route path="/" element={<AuthPage/>} />
                    <Route path="/dashboard" element={<ProtectedRoute><Dashboard/></ProtectedRoute>} />
                    <Route path="/complete-profile" element={<ProtectedRoute><CompleteProfilePage /></ProtectedRoute>} />
                    <Route path="/profile/:userId" element={<ProtectedRoute><ProfilePage /></ProtectedRoute>} />
                </Routes>
            </Router>
        </AuthProvider>
    </>
  )
}

export default App
