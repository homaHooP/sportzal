import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../Context/AuthContext.js";

export default function CompleteProfilePage() {
    const [fullName, setFullName] = useState("");
    const [birthDate, setBirthDate] = useState("");
    const [gender, setGender] = useState("");
    const [error, setError] = useState("");

    const { user, setUserInfo } = useAuth();
    const navigate = useNavigate();

    const validate = () => {
        if (!fullName.trim()) return "Full name is required";
        if (!birthDate) return "Birth date is required";
        if (!gender) return "Please select your gender";
        return "";
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const validationError = validate();
        setError(validationError);
        if (validationError) return;

        try {
            await setUserInfo(user.id, fullName,gender, birthDate);
            navigate("/dashboard");
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="min-h-screen bg-back flex items-center justify-center px-6 font-sans">
            <div className="w-full max-w-sm">
                <h1 className="font-hero text-2xl text-text mb-1">Complete your profile</h1>
                <p className="text-sm text-text-calm mb-8">Almost there! We just need a few more details.</p>
                <form onSubmit={handleSubmit} className="space-y-6">
                    <div>
                        <label htmlFor="fullName" className="block text-xs text-text-calm mb-1.5">Full name</label>
                        <input id="fullName" type="text" value={fullName} onChange={(e) => setFullName(e.target.value)} className="w-full border-b bg-back-light p-2 text-text outline-none focus:border-accent transition-colors"/>
                    </div>
                    <div>
                        <label htmlFor="birthDate" className="block text-xs text-text-calm mb-1.5">Birth date</label>
                        <input id="birthDate" type="date" value={birthDate} onChange={(e) => setBirthDate(e.target.value)} className="w-full border-b bg-back-light p-2 text-text outline-none focus:border-accent transition-colors"/>
                    </div>

                    <div>
                        <label htmlFor="gender" className="block text-xs text-text-calm mb-1.5">Gender</label>
                        <select id="gender" value={gender} onChange={(e) => setGender(e.target.value)} className="w-full border-b bg-back-light p-2 text-text outline-none focus:border-accent transition-colors cursor-pointer">
                            <option value="">Select gender</option>
                            <option value="Male">Male</option>
                            <option value="Female">Female</option>
                            <option value="Other">Other</option>
                        </select>
                    </div>

                    {error && (
                        <p className="text-xs text-accent-2">
                            {error}
                        </p>
                    )}

                    <button type="submit" className="w-full bg-accent text-back py-2.5 rounded-md text-sm mt-2 hover:opacity-90 transition-opacity cursor-pointer">
                        Save and continue
                    </button>

                </form>
            </div>
        </div>
    );
}