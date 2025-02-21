import React, {useContext} from 'react';

import {FrontendContext} from "../Internals/ContextStore";
import {useNavigate} from 'react-router-dom';

import Accordion from '@mui/material/Accordion';
import AccordionDetails from '@mui/material/AccordionDetails';
import AccordionSummary from '@mui/material/AccordionSummary';
import Typography from '@mui/material/Typography';
import Button from "@mui/material/Button";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

import DeleteUserButton from '../Components/Settings/DeleteUserButton';


/**
 * Component representing the settings page.
 *
 * This component provides options for users to manage their account settings
 * such as deleting their account and logging out. The settings are organized
 * within expandable accordions for a clean and structured user interface.
 *
 * @component
 * @example
 * // Basic usage
 * const element = <Settings />;
 *
 * Note: This component utilizes React, React context, and hooks like `useState`
 * and `useContext` to manage component state and navigation.
 */
const Settings: React.FC = () => {
    const frontendContext = useContext(FrontendContext);
    const nav = useNavigate();

    const [expanded, setExpanded] = React.useState<string | false>(false);

    /**
     * The HandleLogOut function initiates the user logout process.
     * When invoked, it navigates the user to the signout page.
     *
     * @function
     * @name HandleLogOut
     */
    const HandleLogOut = () => {
        nav('/signout');
    };

    /**
     * @function handleDeleteSuccess
     * @description Callback function that handles the successful deletion of a user. Upon successful deletion, this function logs a success message to the console and navigates the user to the signout page. This function should encompass any logic necessary to refresh the graphics list or handle UI changes.
     */
    const handleDeleteSuccess = () => {
        // Implement your logic to refresh the graphics list or handle UI changes
        console.log('User deleted successfully');
        nav('/signout');
    };

    /**
     * `handleChange` is a function that returns an event handler for managing the
     * expansion state of a UI component, such as an accordion panel.
     *
     * @param {string} panel - The identifier for the panel whose state is being managed.
     * @returns {Function} An event handler function that receives a `React.SyntheticEvent`
     * and a boolean indicating whether the panel is expanded. It sets the expanded
     * state based on the boolean value.
     */
    const handleChange =
        (panel: string) => (event: React.SyntheticEvent, isExpanded: boolean) => {
            setExpanded(isExpanded ? panel : false);
        };

    //TODO: Add light/dark mode toggle
    return (
        <div>
            <h1>Settings</h1>

            <Accordion expanded={expanded === "panel1"} onChange={handleChange("panel1")}>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon/>}
                    aria-controls="panel1bh-content"
                    id="panel1bh-header"
                >
                    <Typography sx={{width: "33%", flexShrink: 0}}>Delete Account</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <Typography>
                        Are you sure you want to delete your account? This action cannot be undone.
                    </Typography>
                    <DeleteUserButton onDeleteSuccess={handleDeleteSuccess}/>
                </AccordionDetails>
            </Accordion>

            <Accordion expanded={expanded === "panel2"} onChange={handleChange("panel2")}>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon/>}
                    aria-controls="panel2bh-content"
                    id="panel2bh-header"
                >
                    <Typography sx={{width: "33%", flexShrink: 0}}>Log Out</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <Typography>
                        Do you want to log out of your account?
                    </Typography>
                    <Button variant="contained" color="warning" onClick={HandleLogOut}>
                        Log Out
                    </Button>
                </AccordionDetails>
            </Accordion>
        </div>
    );
}

export default Settings;