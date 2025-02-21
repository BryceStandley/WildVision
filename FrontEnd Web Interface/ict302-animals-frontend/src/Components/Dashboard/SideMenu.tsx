import React, {useContext} from 'react';
import {Avatar, Box, Divider, Drawer as MuiDrawer, drawerClasses, Stack, styled, Typography} from '@mui/material';
import MenuContent from './MenuContent';
import OptionsMenu from './OptionsMenu';
import {Link} from 'react-router-dom';
import {FrontendContext} from "../../Internals/ContextStore";
import DashboardPages from './DashboardHelpers';


/**
 * Properties for configuring the SideMenu component.
 *
 * @typedef {Object} SideMenuProps
 * @property {DashboardPages} currentDashboardPage - The current dashboard page being displayed.
 * @property {React.Dispatch<React.SetStateAction<DashboardPages>>} setCurrentDashboardPage - Function to update the current dashboard page.
 * @property {('permanent' | 'temporary')} [variant] - The display variant of the side menu.
 * @property {boolean} [open] - Whether the side menu is open.
 * @property {() => void} [onClose] - Callback function invoked when the side menu is closed.
 */
interface SideMenuProps {
    currentDashboardPage: DashboardPages;
    setCurrentDashboardPage: React.Dispatch<React.SetStateAction<DashboardPages>>;
    variant?: 'permanent' | 'temporary';
    open?: boolean;
    onClose?: () => void;
}


const drawerWidth = 240;

/**
 * Styled Drawer component based on the Material UI Drawer.
 *
 * This Drawer component is customized using styled-components to override
 * default styles and enhance the appearance and layout.
 * It has a fixed width (determined by drawerWidth) and does not shrink.
 * The component ensures border-box sizing for layout consistency.
 * Additionally, the margin-top is set to 10 units.
 *
 * Special stylings for the paper class of the Drawer include:
 * - Setting the width equal to drawerWidth.
 * - Applying border-box sizing.
 *
 * Uses:
 * - `drawerWidth`: The width of the drawer, ensuring a consistent size.
 * - `mt`: Margin-top, adjusted to 10 units for proper spacing.
 *
 * Ensures:
 * - Consistent width and sizing behavior across different usages.
 *
 * Dependencies:
 * - `MuiDrawer`: Base component from Material UI.
 * - `styled`: Utility from styled-components for creating styled versions of components.
 */
const Drawer = styled(MuiDrawer)({
    width: drawerWidth,
    flexShrink: 0,
    boxSizing: 'border-box',
    mt: 10,
    [`& .${drawerClasses.paper}`]: {
        width: drawerWidth,
        boxSizing: 'border-box',
    },
});

/**
 * SideMenu component renders the side navigation drawer for the dashboard
 * page. It includes the project logo, user information, and navigation links.
 * This component can be customized to use different variants such as 'permanent'
 * or 'temporary', and can be toggled open or closed.
 *
 * @param {Object} props - Component props.
 * @param {string} props.currentDashboardPage - The identifier of the currently active dashboard page.
 * @param {function} props.setCurrentDashboardPage - Function to set the currently active dashboard page.
 * @param {string} [props.variant='permanent'] - The variant to use for the drawer ('permanent' for larger screens).
 * @param {boolean} [props.open=true] - Determines if the drawer is open (always true for permanent drawer).
 * @param {function} [props.onClose=() => {}] - Callback for handling drawer close action (no-op for permanent drawer).
 *
 * @returns {JSX.Element} The rendered component.
 */
const SideMenu: React.FC<SideMenuProps> = ({
                                               currentDashboardPage,
                                               setCurrentDashboardPage,
                                               variant = 'permanent', // Default to permanent for larger screens
                                               open = true, // For permanent, the drawer is always open
                                               onClose = () => {
                                               } // No-op for permanent drawer
                                           }) => {
    const frontendContext = useContext(FrontendContext);

    return (
        <Drawer
            variant={variant}
            open={open}
            onClose={onClose}
            sx={{
                width: drawerWidth,
                flexShrink: 0,
                [`& .MuiDrawer-paper`]: {
                    width: drawerWidth,
                    boxSizing: 'border-box',
                },
            }}
        >
            <div style={{justifyContent: 'center', alignItems: 'center', display: 'flex'}}>
                <Link style={{textDecoration: 'none'}} to="/">
                    <img style={{width: 100, height: 100, alignSelf: 'center'}}
                         src={"/assets/images/project/ProjectLogo-Trim.png"} alt="logo"/>
                    <Typography sx={{
                        textAlign: 'center',
                        mr: 2,
                        fontFamily: 'monospace',
                        fontWeight: 700,
                        color: 'black',
                    }} variant="h6" component="h2" gutterBottom>
                        WildVision
                    </Typography>
                </Link>
            </div>
            <Box
                sx={{
                    display: 'flex',
                    p: 1.5,
                }}
            >

            </Box>
            <Divider/>

            <MenuContent currentDashboardPage={currentDashboardPage}
                         setCurrentDashboardPage={setCurrentDashboardPage}/> {/* This renders a List of pages the user can navigate to from the dashboard */}

            <Stack
                direction="row"
                sx={{
                    p: 2,
                    gap: 1,
                    alignItems: 'center',
                    borderTop: '1px solid',
                    borderColor: 'divider',
                }}
            >
                <Avatar
                    sizes="small"
                    alt={frontendContext.user.contextRef?.current.username}
                    sx={{width: 36, height: 36}}
                >{frontendContext.user.contextRef?.current.initials}</Avatar>
                <Box sx={{mr: 'auto'}}>
                    <Typography variant="body2" sx={{fontWeight: 500, lineHeight: '16px'}}>
                        {frontendContext.user.contextRef?.current.username}
                    </Typography>
                    <Typography variant="caption" sx={{color: 'text.secondary'}}>
                        {frontendContext.user.contextRef?.current.email}
                    </Typography>
                </Box>
                <OptionsMenu currentDashboardPage={currentDashboardPage}
                             setCurrentDashboardPage={setCurrentDashboardPage}/>
            </Stack>
        </Drawer>
    );
}

export default SideMenu;