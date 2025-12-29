import React from 'react';

const LoaderPage = () => {
    return (
        <div style={{
            height: window.innerHeight - 100,
            display: "flex",
            alignItems: "center",
            width: "100%",
            background: "transparent",
            flexDirection: "column",
            justifyContent: "center"

        }}>
            <img width="60px" src={"../../images/logo.svg"} alt="loader" style={{
                height: 50,
                width: "auto"
            }} />

        </div>
    );
};

export default LoaderPage;