import * as React from 'react';
import Stack from '@mui/material/Stack';
import NotificationsRoundedIcon from '@mui/icons-material/NotificationsRounded';
import CustomDatePicker from './CustomDatePicker';
import NavbarBreadcrumbs from './NavbarBreadcrumbs';
import MenuButton from './MenuButton';

import Search from './Search';

import {DashboardMenuProps} from './DashboardHelpers';

/**
 * Header component for the dashboard menu.
 *
 * This component displays a header that includes breadcrumbs for navigation,
 * a search bar, a custom date picker, and a menu button for notifications.
 *
 * Props:
 * - `currentDashboardPage`: The current page of the dashboard.
 * - `setCurrentDashboardPage`: Function to set the current dashboard page.
 *
 * The header is responsive and adjusts layout based on the screen size.
 */
const Header: React.FC<DashboardMenuProps> = ({currentDashboardPage, setCurrentDashboardPage}) => {
    return (
        <Stack
            direction="row"
            sx={{

                display: {xs: 'none', md: 'flex'},
                width: '100%',
                alignItems: {xs: 'flex-start', md: 'center'},
                justifyContent: 'space-between',
                maxWidth: {sm: '100%', md: '1700px'},
                pt: 1.5,

            }}
            spacing={2}
        >
            <NavbarBreadcrumbs currentDashboardPage={currentDashboardPage}
                               setCurrentDashboardPage={setCurrentDashboardPage}/>
            <Stack direction="row" sx={{gap: 1}}>
                <Search/>
                <CustomDatePicker/>
                <MenuButton showBadge aria-label="Open notifications">
                    <NotificationsRoundedIcon/>
                </MenuButton>
            </Stack>
        </Stack>
    );
}

export default Header;