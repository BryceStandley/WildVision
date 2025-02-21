import React, {useContext} from 'react';
import {alpha, Box, Stack, SwipeableDrawer, Theme, useMediaQuery} from '@mui/material';

import AppNavbar from '../Components/Dashboard/AppNavbar';
import Header from '../Components/Dashboard/Header';
import SideMenu from '../Components/Dashboard/SideMenu';

import {FrontendContext} from "../Internals/ContextStore";
import DashboardPages, {
    getDashboardPageFromPath,
    getDashboardPageRenderFromDashboardPage
} from '../Components/Dashboard/DashboardHelpers';


/**
 * Interface representing the properties of a Dashboard component.
 *
 * @typedef {Object} DashboardProps
 *
 * @property {string} [renderedPage] - Optional property indicating the page that has been rendered.
 */
interface DashboardProps {
    renderedPage?: string;
}


/**
 * Dashboard component for rendering the main user interface of the application.
 *
 * This component conditionally renders different interfaces based on screen size
 * and the current state. It manages the state for the current dashboard page and
 * whether the drawer is open or closed. It also contains references to other
 * components like SideMenu, AppNavbar, and Header.
 *
 * @param {DashboardProps} props - The properties passed to the Dashboard component.
 * @param {DashboardPages} props.renderedPage - The current page to be rendered on the dashboard.
 *
 * @returns {React.ReactElement} The rendered Dashboard component.
 */
const Dashboard: React.FC<DashboardProps> = ({renderedPage}) => {
    const frontendContext = useContext(FrontendContext);

    const [currentDashboardPage, setCurrentDashboardPage] = React.useState<DashboardPages>(renderedPage ? getDashboardPageFromPath(renderedPage) : DashboardPages.Upload);
    const [isDrawerOpen, setIsDrawerOpen] = React.useState(false); // State to handle drawer open/close
    const isLargeScreen = useMediaQuery((theme: Theme) => theme.breakpoints.up('md')); // Check if screen size is medium or larger

    /**
     * Toggles the state of the drawer.
     *
     * @param {boolean} newState - The desired state of the drawer where true means open and false means closed.
     * @returns {Function} A function that, when called, updates the drawer's state.
     */
    const toggleDrawer = (newState: boolean) => () => {
        setIsDrawerOpen(newState);
    };


    return (
        <Box sx={{display: 'flex', position: 'relative'}}>
            {isLargeScreen ? (
                <SideMenu currentDashboardPage={currentDashboardPage}
                          setCurrentDashboardPage={setCurrentDashboardPage}/>
            ) : (
                <SwipeableDrawer
                    anchor="left"
                    open={isDrawerOpen}
                    onClose={toggleDrawer(false)}
                    onOpen={toggleDrawer(true)}
                >
                    <SideMenu
                        currentDashboardPage={currentDashboardPage}
                        setCurrentDashboardPage={setCurrentDashboardPage}
                        variant="temporary"
                        open={isDrawerOpen}
                        onClose={toggleDrawer(false)}
                    />
                </SwipeableDrawer>
            )}
            <AppNavbar currentDashboardPage={currentDashboardPage} setCurrentDashboardPage={setCurrentDashboardPage}/>
            {/* Main content */}
            <Box
                component="main"

                sx={(theme: { palette: { background: { default: string; }; }; }) => ({
                    flexGrow: 1,
                    backgroundColor: alpha(theme.palette.background.default, 1),
                    overflow: 'auto',
                })}
            >
                <Stack
                    spacing={2}
                    sx={{
                        alignItems: 'center',
                        mx: 3,
                        pb: 10,
                        mt: {xs: 8, md: 0},
                    }}
                >
                    <>
                        <Header currentDashboardPage={currentDashboardPage}
                                setCurrentDashboardPage={setCurrentDashboardPage}/>
                        {getDashboardPageRenderFromDashboardPage(currentDashboardPage)} {/* Render the given page from the router or the main grid if on dashboard home */}
                    </>
                </Stack>
            </Box>
        </Box>
    );
}

export default Dashboard;