import * as React from 'react';
import {PaletteMode} from '@mui/material/styles';
import WbSunnyRoundedIcon from '@mui/icons-material/WbSunnyRounded';
import ModeNightRoundedIcon from '@mui/icons-material/ModeNightRounded';
import MenuButton, {MenuButtonProps} from './MenuButton';

interface ToggleColorModeProps extends MenuButtonProps {
    mode: PaletteMode;
    toggleColorMode: () => void;
}

/**
 * Toggles the color mode between 'dark' and 'light' themes.
 *
 * @param {Object} props - The properties object.
 * @param {string} props.mode - The current color mode, either 'dark' or 'light'.
 * @param {function} props.toggleColorMode - The function to toggle the color mode.
 * @returns {JSX.Element} A button component that toggles the color mode on click.
 */
export default function ToggleColorMode({
                                            mode,
                                            toggleColorMode,
                                            ...props
                                        }: ToggleColorModeProps) {
    return (
        <MenuButton
            onClick={toggleColorMode}
            size="small"
            aria-label="button to toggle theme"
            {...props}
        >
            {mode === 'dark' ? (
                <WbSunnyRoundedIcon fontSize="small"/>
            ) : (
                <ModeNightRoundedIcon fontSize="small"/>
            )}
        </MenuButton>
    );
}
