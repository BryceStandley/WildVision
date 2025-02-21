import React, {useContext, useEffect, useState} from 'react';
import {Navigate, Route, Routes, useLocation} from 'react-router-dom';

// Import of the Main App CSS file
import './App.css';

// Imports of our components
import LandingNav from './Components/LandingNav';

// Imports of our pages
import LandingPage from './Pages/LandingPage';
import About from './Pages/About';
import Enterprise from './Pages/Enterprise';
import SignIn from './Pages/SignIn';
import SignUp from './Pages/SignUp';
import SignOut from './Pages/SignOut';
import Dashboard from './Pages/Dashboard';
import {FrontendContext} from './Internals/ContextStore';


// Imports of MUI and custom Theme components
import {createTheme, CssBaseline, PaletteMode, ThemeProvider} from "@mui/material";

import getDashboardTheme from './Theme/getDashboardTheme';


/**
 * The main App component that initializes and renders the entire application.
 * It sets up themes, routing, and context for managing page navigation and rendering.
 * Utilizes React's state and effect hooks to update navigation properties and theme modes.
 *
 * @constant
 * @type {React.FC}
 */
const App: React.FC = () => {

    const [mode, setMode] = React.useState<PaletteMode>('light');
    const dashboardTheme = createTheme(getDashboardTheme(mode));

    const [activeTab, setActiveTab] = useState(0);


    var currentLocation = useLocation();
    var currentPath = currentLocation.pathname;
    var currentSearch = currentLocation.search;
    var currentHash = currentLocation.hash;

    /**
     * Extracts the current URL path, search query, and hash fragment
     * from the `currentLocation` object and assigns them to variables.
     */
    useEffect(() => {
        currentPath = currentLocation.pathname;
        currentSearch = currentLocation.search;
        currentHash = currentLocation.hash;
    }, [currentLocation]);


    /**
     * `frontendContext` holds the context instance from `FrontendContext`.
     * This context provides the state and functions essential for the frontend
     * part of the application, enabling components to share and access common
     * data and methods without prop drilling.
     *
     * It is typically obtained using the `useContext` hook from React and
     * should be used within components to access or update the frontend
     * specific state and actions defined in `FrontendContext`.
     *
     * Note: The `FrontendContext` must be a valid React Context object created
     * and provided at a higher level in the component tree.
     *
     * @type {Object}
     */
    const frontendContext = useContext(FrontendContext);

    /**
     * An array containing paths used for routing in the dashboard component of the application.
     *
     * Each path represents a specific route within the dashboard section, enabling navigation
     * to different parts of the dashboard such as home, upload, completed tasks, settings,
     * about page, help section, user account, list of animals, and details about a specific animal.
     *
     * Example paths include:
     * - "/dashboard/home"
     * - "/dashboard/upload"
     * - "/dashboard/animals/:animalId"
     */
    const dashboardPagePaths = [
        "/dashboard",
        "/dashboard/home",
        "/dashboard/upload",
        "/dashboard/completed",
        "/dashboard/settings",
        "/dashboard/about",
        "/dashboard/help",
        "/dashboard/account",
        "/dashboard/animals",
        "/dashboard/animals/:animalId"
    ];

// Returns the main component of the app with the navigation bar and the routes
    return (
        <ThemeProvider theme={dashboardTheme}>
            <CssBaseline enableColorScheme/>
            <main>
                <div className="d-flex flex-row">
                    {!currentPath.includes("dashboard") && <LandingNav/>}
                    <div className="d-flex flex-column content" id="page-wrap">
                        <Routes> {/* This is where the routes are defined */}

                            <Route path="/" element={<LandingPage/>}/> {/* This is the default route */}
                            <Route path="/about" element={<About/>}/> {/* This is the about page */}
                            <Route path="/enterprise" element={<Enterprise/>}/> {/* This is the enterprise page */}
                            <Route path="/signin" element={<SignIn/>}/> {/* This is the sign in page */}
                            <Route path="/signup" element={<SignUp/>}/> {/* This is the sign up page */}
                            <Route path="/signout" element={<SignOut/>}/> {/* This is the sign out page */}
                            {dashboardPagePaths.map((dashboardPath, index) => (
                                <Route key={index} path={dashboardPath}
                                       element={frontendContext.user.valid ? <Dashboard renderedPage={dashboardPath}/> :
                                           <Navigate to="/"/>}/>
                            ))};
                            <Route path="*" element={<Navigate
                                to="/"/>}/> {/* This will redirect to the landing page if the route is not found */}

                        </Routes>
                    </div>
                </div>
            </main>
        </ThemeProvider>
    );
}

export default App;