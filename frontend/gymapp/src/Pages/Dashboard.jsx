import Header from "../components/Header.jsx";
import Container from "../components/Container.jsx";
import {useAuth} from "../Context/AuthContext.js";
import {useNavigate} from "react-router-dom";

function Dashboard(){
    const {user} = useAuth();
    const navigate = useNavigate();
    return (
        <>
            <Header/>
            <Container>
                <h1 className="text-text font-hero text-center">Welcome, {user.fullName === "" ? "Guest" : user.fullName}!</h1>
                {user.roles[0] === "Client" &&
                    <div className="sm:grid sm:grid-cols-2 flex flex-col gap-2 items-center justify-center">
                        <div onClick={()=>navigate("/memberships/...")} className="flex items-center w-35 h-35 p-3 bg-back-light text-text font-sans text-center hover:bg-border hover:scale-105 transition cursor-pointer">My memberships</div>
                        <div onClick={()=>navigate("/sessions/...")} className="flex items-center w-35 h-35 p-3 bg-back-light text-text font-sans text-center hover:bg-border hover:scale-105 transition cursor-pointer">Book a training</div>
                    </div>
                }
            </Container>
        </>
    )
}

export default Dashboard;