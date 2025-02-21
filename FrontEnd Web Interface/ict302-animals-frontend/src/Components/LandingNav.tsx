import React, {useContext, useEffect, useState} from "react";
import {Link} from 'react-router-dom';

import {
    AppBar,
    Avatar,
    Box,
    Button,
    Container,
    createTheme,
    CssBaseline,
    Grid2 as Grid,
    IconButton,
    Menu,
    MenuItem,
    PaletteMode,
    Toolbar,
    Tooltip,
    Typography
} from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import PetsIcon from '@mui/icons-material/Pets';
import TungstenIcon from '@mui/icons-material/Tungsten';
import TungstenOutlinedIcon from '@mui/icons-material/TungstenOutlined';

import {ThemeProvider} from "@mui/material/styles";
import {FrontendContext} from "../Internals/ContextStore";

import './LandingNav.css';
import getDashboardTheme from "../Theme/getDashboardTheme";

const pages = ['Home', 'About']; // TODO Add any other pages here, Must match the routes in App.tsx

const settings = ['Dashboard', 'SignOut']; // TODO Add any other settings here


/**
 * `LandingNav` is a functional React component that serves as the navigation bar for the application.
 *
 * It includes:
 * - Theme management using Material-UI's `ThemeProvider` and local storage for persistent user preferences.
 * - Navigation elements that allow switching between different pages of the application.
 * - Conditional rendering based on user authentication status, displaying user-specific options if logged in.
 * - A responsive design that adjusts visibility and layout based on screen size.
 *
 * State:
 * - `anchorElNav`: Manages the anchor element for navigation menu.
 * - `anchorElUser`: Manages the anchor element for user menu.
 * - `mode`: Tracks the current theme mode ('light' or 'dark').
 * - `showCustomTheme`: Toggles custom theme display.
 *
 * Effects:
 * - An effect to retrieve the user's preferred theme mode from local storage or system preferences on initial render.
 *
 * Handlers:
 * - `handleOpenNavMenu`: Opens the navigation menu.
 * - `handleOpenUserMenu`: Opens the user menu.
 * - `handleCloseNavMenu`: Closes the navigation menu.
 * - `handleCloseUserMenu`: Closes the user menu.
 * - `toggleColorMode`: Toggles between 'light' and 'dark' theme modes.
 */
const LandingNav: React.FC = () => {
    const frontendContext = useContext(FrontendContext);

    const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(null);
    const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);

    const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElNav(event.currentTarget);
    };
    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElUser(event.currentTarget);
    };

    const handleCloseNavMenu = () => {
        console.log('closing nav menu');
        setAnchorElNav(null);
    };

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
    };
    const [mode, setMode] = useState<PaletteMode>('light');
    const [showCustomTheme, setShowCustomTheme] = useState(true);
    const defaultTheme = createTheme({palette: {mode}});
    useEffect(() => {
        // Check if there is a preferred mode in localStorage
        const savedMode = localStorage.getItem('themeMode') as PaletteMode | null;
        if (savedMode) {
            setMode(savedMode);
        } else {
            // If no preference is found, it uses system preference
            const systemPrefersDark = window.matchMedia(
                '(prefers-color-scheme: dark)',
            ).matches;
            setMode(systemPrefersDark ? 'dark' as PaletteMode : 'light' as PaletteMode);
        }
    }, []);

    //TODO: Fix theme toggle with app
    const toggleColorMode = () => {
        const newMode = mode === 'dark' as PaletteMode ? 'light' as PaletteMode : 'dark' as PaletteMode;
        setMode(newMode);
        localStorage.setItem('themeMode', newMode); // Save the selected mode to localStorage
    };
    const dashboardTheme = createTheme(getDashboardTheme(mode));

    return (
        <ThemeProvider theme={dashboardTheme}>
            <CssBaseline enableColorScheme/>
            <AppBar position="static"
                    sx={{
                        background: 'linear-gradient(90deg, rgba(255,105,105,0.7), rgba(173,216,230,0.6))',
                    }}>
                <Container maxWidth="xl">
                    <Toolbar disableGutters>
                        <PetsIcon sx={{display: {xs: 'none', md: 'flex'}, mr: 1, color: '#FFFFFF'}}/>
                        <Typography
                            variant="h6"
                            noWrap
                            component="a"
                            href="/"
                            sx={{
                                mr: 2,
                                display: {xs: 'none', md: 'flex'},
                                fontFamily: 'monospace',
                                fontWeight: 700,
                                color: 'black',
                                textDecoration: 'none',
                            }}
                        >
                            WildVision
                        </Typography>

                        <Box sx={{flexGrow: 1, display: {xs: 'flex', md: 'none'}}}>
                            <IconButton
                                size="large"
                                aria-label="account of current user"
                                aria-controls="menu-appbar"
                                aria-haspopup="true"
                                onClick={handleOpenNavMenu}
                            >
                                <MenuIcon/>
                            </IconButton>
                            <Menu
                                id="menu-appbar"
                                anchorEl={anchorElNav}
                                anchorOrigin={{
                                    vertical: 'bottom',
                                    horizontal: 'left',
                                }}
                                keepMounted
                                transformOrigin={{
                                    vertical: 'top',
                                    horizontal: 'left',
                                }}
                                open={Boolean(anchorElNav)}
                                onClose={handleCloseNavMenu}
                                sx={{display: {xs: 'block', md: 'none'}}}
                            >
                                {pages.map((page) => (
                                    <MenuItem key={page}>
                                        <Link to={page.toLowerCase()}>
                                            <Typography sx={{textAlign: 'center'}}>{page}</Typography>
                                        </Link>
                                    </MenuItem>
                                ))}
                            </Menu>
                        </Box>
                        <PetsIcon sx={{display: {xs: 'flex', md: 'none'}, mr: 1}}/> {/* TODO Replace with team logo */}
                        <Typography
                            variant="h5"
                            noWrap
                            component="a"
                            href="/"
                            sx={{
                                mr: 2,
                                display: {xs: 'flex', md: 'none'},
                                flexGrow: 1,
                                fontFamily: 'monospace',
                                fontWeight: 700,
                                letterSpacing: '.3rem',
                                color: 'black',
                                textDecoration: 'none',
                            }}
                        >
                            WildVision
                        </Typography>
                        <Box sx={{flexGrow: 1, display: {xs: 'none', md: 'flex'}}}>
                            {pages.map((page) => (
                                <Button
                                    key={page}
                                    component={Link}
                                    sx={{my: 2, color: 'black', display: 'block'}}
                                    to={page.toLowerCase()}
                                >
                                    {page}
                                </Button>
                            ))}
                        </Box>
                        {frontendContext.user.valid && frontendContext.user.contextRef?.current.loggedInState ?
                            <Box sx={{flexGrow: 0}}> {/* TODO Replace with user avatar */}
                                <Tooltip title="Open Profile">
                                    <IconButton onClick={handleOpenUserMenu} sx={{p: 0}}>
                                        <Avatar
                                            alt={frontendContext.user.contextRef?.current.username}>{frontendContext.user.contextRef?.current.initials}</Avatar>
                                    </IconButton>
                                </Tooltip>
                                <Menu
                                    sx={{mt: '45px'}}
                                    id="menu-appbar"
                                    anchorEl={anchorElUser}
                                    anchorOrigin={{
                                        vertical: 'top',
                                        horizontal: 'right',
                                    }}
                                    keepMounted
                                    transformOrigin={{
                                        vertical: 'top',
                                        horizontal: 'right',
                                    }}
                                    open={Boolean(anchorElUser)}
                                    onClose={handleCloseUserMenu}
                                >
                                    {settings.map((setting) => (
                                        <MenuItem key={setting}>
                                            <Link
                                                to={setting.toLowerCase() === 'account' ? '/dashboard/account' : setting.toLowerCase()}>
                                                <Typography sx={{textAlign: 'center'}}>{setting}</Typography>
                                            </Link>
                                        </MenuItem>
                                    ))}

                                </Menu>
                            </Box>
                            :
                            <Box sx={{flexGrow: 0}}>
                                <Grid container spacing={4}>
                                    <Grid>
                                        <IconButton
                                            onClick={toggleColorMode}>
                                            {mode && mode === "dark" ? <TungstenOutlinedIcon/> : <TungstenIcon/>}
                                        </IconButton>
                                    </Grid>
                                    <Grid>
                                        <Button
                                            component={Link}
                                            sx={{color: 'black'}}
                                            to={'/signin'}
                                        >
                                            Sign-In
                                        </Button>
                                    </Grid>
                                </Grid>
                            </Box>
                        }
                    </Toolbar>
                </Container>
            </AppBar>
        </ThemeProvider>
    );
}
export default LandingNav;
