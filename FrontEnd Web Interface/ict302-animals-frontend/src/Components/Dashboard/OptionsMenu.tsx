import React from 'react';
import {
    Button,
    dividerClasses,
    listClasses,
    ListItemIcon,
    listItemIconClasses,
    ListItemText,
    Menu,
    MenuItem as MuiMenuItem,
    paperClasses,
    styled
} from '@mui/material';

import LogoutRoundedIcon from '@mui/icons-material/LogoutRounded';
import MoreVertRoundedIcon from '@mui/icons-material/MoreVertRounded';
import MenuButton from './MenuButton';

import {DashboardMenuProps} from './DashboardHelpers';

import {Link, useNavigate} from 'react-router-dom';

const MenuItem = styled(MuiMenuItem)({
    margin: '2px 0',
});

/**
 * OptionsMenu component.
 *
 * This component provides a dropdown menu for the dashboard interface.
 * It includes navigation and state management to handle the visibility
 * and interaction of the menu items. The menu itself triggers the navigation
 * and dashboard page updates by using React hooks and context properties.
 *
 * @component
 * @param {object} props - The properties passed to this component.
 * @param {DashboardPage} props.currentDashboardPage - The current page in the dashboard.
 * @param {function} props.setCurrentDashboardPage - The function to update the current dashboard page.
 * @returns {JSX.Element} A dropdown menu component with navigation capabilities.
 */
const OptionsMenu: React.FC<DashboardMenuProps> = ({currentDashboardPage, setCurrentDashboardPage}) => {
    const nav = useNavigate();

    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
        //setCurrentDashboardPage(DashboardPages.Account);
        //nav('/dashboard/account');
    };
    return (
        <React.Fragment>
            <MenuButton
                aria-label="Open menu"
                onClick={handleClick}
                sx={{borderColor: 'transparent'}}
            >
                <MoreVertRoundedIcon/>
            </MenuButton>
            <Menu
                anchorEl={anchorEl}
                id="menu"
                open={open}
                onClose={handleClose}
                onClick={handleClick}
                transformOrigin={{horizontal: 'right', vertical: 'top'}}
                anchorOrigin={{horizontal: 'right', vertical: 'bottom'}}
                sx={{
                    [`& .${listClasses.root}`]: {
                        padding: '4px',
                    },
                    [`& .${paperClasses.root}`]: {
                        padding: 0,
                    },
                    [`& .${dividerClasses.root}`]: {
                        margin: '4px -4px',
                    },
                }}
            >
                <Button
                    component={Link}
                    to="/signout"
                    sx={{
                        [`& .${listItemIconClasses.root}`]: {
                            ml: 'auto',
                            minWidth: 0,
                        },
                    }}
                >
                    <MenuItem
                        sx={{
                            [`& .${listItemIconClasses.root}`]: {
                                ml: 'auto',
                                minWidth: 0,
                            },
                        }}
                    >
                        <ListItemText>Logout</ListItemText>
                        <ListItemIcon>
                            <LogoutRoundedIcon fontSize="small"/>
                        </ListItemIcon>
                    </MenuItem>
                </Button>
            </Menu>
        </React.Fragment>
    );
}

export default OptionsMenu;
