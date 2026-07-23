export default function Spinner({ size = "md", fullScreen = false }) {
    const sizes = {
        sm: "w-4 h-4 border-2",
        md: "w-6 h-6 border-2",
        lg: "w-10 h-10 border-4",
    };

    return (
        <div className={fullScreen ? "min-h-screen flex items-center justify-center" : "flex items-center justify-center"}>
            <div className={`${sizes[size]} border-border border-t-accent rounded-full animate-spin`} />
        </div>
    );
}