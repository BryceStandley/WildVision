import React from 'react';
import {alpha, Menu, MenuProps, styled} from "@mui/material";

/**
 * `StyledMenu` is a styled component that customizes the appearance of a `Menu` component.
 *
 *  - It applies various styles to the `MuiPaper-root` class to define the look of the menu.
 *  - It sets a few properties for the `Menu` component such as elevation and anchor origin.
 *  - It customizes styles for child elements like `MuiMenu-list` for padding and `MuiMenuItem-root`
 *    for item-specific UI customizations, including icon size, color, and active state.
 *  - The component adapts to the current theme, including adjustments for dark mode.
 *
 * @typedef {object} MenuProps
 */
const StyledMenu = styled((props: MenuProps) => (
    <Menu elevation={0} anchorOrigin={{vertical: 'bottom', horizontal: 'right'}}
          transformOrigin={{vertical: 'top', horizontal: 'right'}}{...props}/>
))(({theme}) => ({
    '& .MuiPaper-root': {
        borderRadius: 6,
        marginTop: theme.spacing(1),
        minWidth: 180,
        color: 'rgb(55, 65, 81)',
        boxShadow:
            'rgb(255, 255, 255) 0px 0px 0px 0px, rgba(0, 0, 0, 0.05) 0px 0px 0px 1px, rgba(0, 0, 0, 0.1) 0px 10px 15px -3px, rgba(0, 0, 0, 0.05) 0px 4px 6px -2px',
        '& .MuiMenu-list': {
            padding: '4px 0',
        },
        '& .MuiMenuItem-root': {
            '& .MuiSvgIcon-root': {
                fontSize: 18,
                color: theme.palette.text.secondary,
                marginRight: theme.spacing(1.5),
            },
            '&:active': {
                backgroundColor: alpha(
                    theme.palette.primary.main,
                    theme.palette.action.selectedOpacity,
                ),
            },
        },
        ...theme.applyStyles('dark', {
            color: theme.palette.grey[300],
        }),
    },
}));

export default StyledMenu;