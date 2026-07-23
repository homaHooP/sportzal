import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useAuth } from "../Context/AuthContext.js";
import { getUserById } from "../api/userService.js";
import Header from "../components/Header";
import Spinner from "../components/Spinner";
import Container from "../components/Container";

export default function ProfilePage() {
    const { userId } = useParams();
    const { user, logout } = useAuth();
    const [profile, setProfile] = useState(null);
    const [loading, setLoading] = useState(true);

    const isOwnProfile = user.id === userId;

    useEffect(() => {
        setLoading(true);
        getUserById(userId)
            .then(setProfile)
            .finally(() => setLoading(false));
    }, [userId]);

    if (loading) return <Spinner size="lg" fullScreen />;

    return (
        <>
            <Header/>
            <Container>
                <h1 className="text-text font-hero text-center">Profile:</h1>
                <div className="flex flex-col align-middle justify-center gap-1 bg-back-light p-5 rounded">
                    {user.roles[0] === "Manager" || user.roles[0] === "HeadManager" &&
                        <p className="text-text font-sans">Id: {profile.id}</p>
                    }
                    <p className="text-text font-sans">Full name: {profile.fullName}</p>
                    <p className="text-text font-sans">Email address: {profile.email}</p>
                    <p className="text-text font-sans">Gender: {profile.gender}</p>
                    <p className="text-text font-sans">Birthday: {profile.birthday}</p>
                    {user.roles[0] === "Manager" || user.roles[0] === "HeadManager" &&
                    <p className="text-text font-sans">Role: {profile.roles[0]}</p>
                    }
                    { profile.wasDeactivated !== "Not deactivated" &&
                        <p className="text-text font-sans">Date of deactivation: {profile.wasDeactivated}</p>
                    }
                    {isOwnProfile && (
                        <div className="flex justify-between pt-2">
                            <button className="bg-border cursor-pointer hover:opacity-90 transition-opacity p-2 rounded">My membership</button>
                            <button onClick={logout} className="bg-red-400 cursor-pointer hover:opacity-90 transition-opacity p-2 rounded">Logout</button>
                        </div>
                    )}
                </div>
            </Container>
        </>
    );
}