function Container({children,className = ""}){
    return (
        <div className={`flex flex-col items-center justify-center w-full max-w-6xl mx-auto px-4 pt-24 sm:px-6 lg:px-8 ${className}`}>
            {children}
        </div>
    )
}

export default Container;