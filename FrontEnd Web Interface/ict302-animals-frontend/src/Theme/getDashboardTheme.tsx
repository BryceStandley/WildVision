import {PaletteMode, ThemeOptions} from '@mui/material/styles';
import {getDesignTokens} from './themePrimitives';
import {
    chartsCustomizations,
    dataDisplayCustomizations,
    dataGridCustomizations,
    datePickersCustomizations,
    feedbackCustomizations,
    inputsCustomizations,
    navigationCustomizations,
    surfacesCustomizations,
    treeViewCustomizations,
} from './customizations';

export default function getDashboardTheme(mode: PaletteMode): ThemeOptions {
    return {
        ...getDesignTokens(mode),
        components: {
            ...chartsCustomizations,
            ...dataGridCustomizations,
            ...datePickersCustomizations,
            ...treeViewCustomizations,
            ...inputsCustomizations,
            ...inputsCustomizations,
            ...dataDisplayCustomizations,
            ...feedbackCustomizations,
            ...navigationCustomizations,
            ...surfacesCustomizations,
        },
    };
}
