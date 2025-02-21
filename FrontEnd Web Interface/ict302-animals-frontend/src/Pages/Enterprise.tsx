import React, {useContext} from 'react';
import {FrontendContext} from "../Internals/ContextStore";

import {AppBar, Box, Fab, Tab, Tabs, Typography, useTheme} from '@mui/material';
import NavigationIcon from '@mui/icons-material/Navigation';


/**
 * Interface representing the properties for the TabPanel component.
 *
 * @interface
 * @property {React.ReactNode} [children] - The content to be displayed inside the TabPanel.
 * @property {string} [dir] - The text direction of the TabPanel. Typically 'ltr' for left-to-right or 'rtl' for right-to-left.
 * @property {number} index - The index of the TabPanel. This property is required and should correspond to the index of the tab associated with this panel.
 * @property {number} value - The currently selected tab's index. This property determines which TabPanel is currently visible.
 */
interface TabPanelProps {
    children?: React.ReactNode;
    dir?: string;
    index: number;
    value: number;
}

/**
 * TabPanel component renders content based on the selected tab index.
 *
 * @param {object} props - Properties passed to the component.
 * @param {React.ReactNode} props.children - The content to be displayed within the panel.
 * @param {number} props.value - The current selected tab index.
 * @param {number} props.index - The index of the tab associated with this panel.
 * @param {object} [props.other] - Additional properties to be spread into the component.
 * @return {JSX.Element} The rendered TabPanel component.
 */
function TabPanel(props: TabPanelProps) {
    const {children, value, index, ...other} = props;

    return (
        <Typography
            component="div"
            role="tabpanel"
            hidden={value !== index}
            id={`action-tabpanel-${index}`}
            aria-labelledby={`action-tab-${index}`}
            {...other}
        >
            {value === index && <Box sx={{p: 3}}>{children}</Box>}
        </Typography>
    );
}

/**
 * Generates accessibility properties for a tab element.
 *
 * @param {any} index - The index of the tab.
 * @return {Object} An object containing the id and aria-controls attributes for the tab.
 */
function a11yProps(index: any) {
    return {
        id: `action-tab-${index}`,
        'aria-controls': `action-tabpanel-${index}`,
    };
}


/**
 * Enterprise component.
 *
 * This component displays a set of packages available for enterprise users,
 * including their storage and 3D generator use per year. It uses a tab-based
 * interface to switch between different packages and provides a "Purchase"
 * floating action button.
 *
 * State:
 *  - value: Tracks the current selected tab.
 *
 * Functions:
 *  - handleChange: Handles the change event when selecting a different tab.
 *
 * Constants:
 *  - packages: An array of package objects, each containing a label, storage,
 *    and generator properties.
 *
 * Context:
 *  - frontendContext: Consumed from FrontendContext.
 */
const Enterprise = () => {
    const frontendContext = useContext(FrontendContext);
//export default function FloatingActionButtonZoom() {
    const theme = useTheme();
    const [value, setValue] = React.useState(0);

    /**
     * This function is an event handler for handling changes in a component or form element.
     * It updates the value state with the new value when a change event is triggered.
     *
     * @param {unknown} event - The event object that triggered the change.
     * @param {number} newValue - The new value to be set.
     */
    const handleChange = (event: unknown, newValue: number) => {
        setValue(newValue);
    };

    /**
     * An array of package objects.
     * Each package includes details about storage capacity and generator usage.
     *
     * Structure:
     * - label: The name of the package.
     * - storage: The storage capacity offered by the package per year.
     * - generator: The number of times the package generator can be used.
     */
    const packages = [
        {label: "Package One", storage: "500 GB /year", generator: "450 times"},
        {label: "Package Two", storage: "400 GB /year", generator: "350 times"},
        {label: "Package Three", storage: "200 GB /year", generator: "150 times"},
    ];


    return (
        <Box sx={{bgcolor: 'background.paper', width: 500, position: 'relative', minHeight: 200,}}>
            <AppBar position="static" color="default">
                <Tabs
                    value={value}
                    onChange={handleChange}
                    indicatorColor="primary"
                    textColor="primary"
                    variant="fullWidth"
                    aria-label="action tabs example"
                >
                    {packages.map((pkg, index) => (
                        <Tab label={pkg.label} {...a11yProps(index)} key={index}/>
                    ))}
                </Tabs>
            </AppBar>

            {packages.map((pkg, index) => (
                <TabPanel value={value} index={index} dir={theme.direction} key={index}>
                    {pkg.label}:<br/>
                    Storage: {pkg.storage}<br/>
                    3D Generator: {pkg.generator}
                </TabPanel>
            ))}

            <Fab variant="extended">
                <NavigationIcon sx={{mr: 1}}/>
                Purchase
            </Fab>
        </Box>

    );
}

export default Enterprise;