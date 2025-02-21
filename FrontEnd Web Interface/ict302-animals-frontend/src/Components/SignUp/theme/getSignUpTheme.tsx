import {PaletteMode, ThemeOptions} from '@mui/material/styles';
import {getDesignTokens} from './themePrimitives';
import {
    dataDisplayCustomizations,
    feedbackCustomizations,
    inputsCustomizations,
    navigationCustomizations,
    surfacesCustomizations,
} from './customizations';

export default function getSignUpTheme(mode: PaletteMode): ThemeOptions {
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
