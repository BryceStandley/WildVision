import {PaletteMode, ThemeOptions} from '@mui/material/styles';
import {getDesignTokens} from './themePrimitives';
import {
    dataDisplayCustomizations,
    feedbackCustomizations,
    inputsCustomizations,
    navigationCustomizations,
    surfacesCustomizations,
} from './customizations';

export default function getSignInTheme(mode: PaletteMode): ThemeOptions {
    return {
        ...getDesignTokens(mode),
        components: {
            ...inputsCustomizations,
            ...dataDisplayCustomizations,
            ...feedbackCustomizations,
            ...navigationCustomizations,
            ...surfacesCustomizations,
        },
    };
}
