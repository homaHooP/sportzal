import { useState, useEffect } from "react";
import {useAuth} from "../Context/AuthContext.js";
import {useNavigate} from "react-router-dom";

function Header() {
    const [visibility, setVisibility] = useState(100);
    const [adminPanel, setAdminPanel] = useState(false);
    const navigate = useNavigate();
    const {user} = useAuth();

    useEffect(() => {
        let ticking = false;

        const updateVisibility = () => {
            if (window.scrollY >= 50) {
                let a = 100 - Math.round(window.scrollY - 50)
                a <= 0? setVisibility(0) : setVisibility(a);
            }
            else {
                setVisibility(100);
            }
            ticking = false;
        };

        const handleScroll = () => {
            if (!ticking) {
                requestAnimationFrame(updateVisibility);
                ticking = true
            }
        };

        window.addEventListener("scroll", handleScroll);
        return () => window.removeEventListener("scroll", handleScroll);
    }, []);

    return (
        <header style={{opacity: visibility/100}} className={`sticky top-0 right-0 left-0 flex items-center justify-between p-4 bg-back-light transition-opacity duration-200 ${visibility < 50 && "pointer-events-none"}`}>
            <div className="flex items-center justify-center gap-3">
                <div className="flex flex-col justify-center">
                    <p className="text-text text-lg leading-tight font-hero">The best gym</p>
                    <p className="text-text text-xs leading-tight font-mono hidden sm:block -mt-0.5 ml-0.5">Lets get better together</p>
                </div>
            </div>
            <div className=" relative flex gap-4 items-center justify-center cursor-pointer select-none">
                <div className="text-text font-sans" onClick={()=> navigate(`/profile/${user.id}`)}>
                    Profile
                </div>
                {(user.roles[0] === "Client" || user.roles[0] === "Manager") &&(
                    <div className="text-text font-sans cursor-pointer">
                        Gym sessions
                    </div>
                )}
                {(user.roles[0] === "Client" || user.roles[0] === "Trainer") &&(
                    <div className="text-text font-sans cursor-pointer">
                        My Schedule
                    </div>
                )}
                {user.roles[0] === "HeadManager" &&
                    <div className="text-text font-sans cursor-pointer" onClick={()=>setAdminPanel(prev => !prev)}>
                        Admin panel
                        <div style={{opacity: Number(adminPanel)}} className={`bg-accent w-25 max-w-[calc(100vw-2rem)] absolute transition-opacity duration-200 ${!adminPanel && "pointer-events-none"}`}>
                            HELLO
                        </div>
                    </div>
                }
            </div>
        </header>
    );
}

export default Header;