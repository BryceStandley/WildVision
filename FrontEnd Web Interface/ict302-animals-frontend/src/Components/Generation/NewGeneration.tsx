import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Typography,
} from "@mui/material";
import React, {useState} from "react";
import {SelectChangeEvent} from "@mui/material/Select";

/**
 * Interface representing the properties required for the Generate component.
 *
 * @interface
 */
interface GenerateProps {
    open: boolean;
    handleClose: () => void;
    onGenerate: () => void;
    graphicID: string;
}

/**
 * A dialog component for generating options for a graphic.
 *
 * @param {Object} props - The properties object.
 * @param {boolean} props.open - A boolean indicating whether the dialog is open.
 * @param {function} props.handleClose - A function to handle the closing of the dialog.
 * @param {function} props.onGenerate - A function to handle the generation action.
 * @param {string} props.graphicID - The identifier for the graphic.
 *
 * @return {JSX.Element} The dialog component for generating options.
 */
export default function NewGeneration({open, handleClose, onGenerate, graphicID}: GenerateProps) {
    const [selectedOption, setSelectedOption] = useState("");

    /**
     * Handles change events for a select element.
     *
     * This function is triggered when the selected option in the select element changes.
     * It updates the `selectedOption` state based on the selected value.
     *
     * @param {SelectChangeEvent} event - The event object representing the change event.
     */
    const handleSelectChange = (event: SelectChangeEvent) => {
        setSelectedOption(event.target.value);
    };

    /**
     * Handles the click event to initiate the generate process.
     *
     * The function performs two main actions:
     * 1. Calls the `onGenerate` function to execute the generate operation.
     * 2. Calls the `handleClose` function to close any related UI elements or perform cleanup.
     *
     * This function is typically used as a callback for a UI button or similar element.
     *
     * @function
     */
    const handleGenerateClick = () => {
        onGenerate();
        handleClose();
    };

    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle>Generate Options for Graphic: {graphicID}</DialogTitle>

            <DialogContent>
                <Typography variant="subtitle1">
                    Please select a generator option for this graphic.
                </Typography>

                <Box mt={2}>
                    <FormControl fullWidth>
                        <InputLabel>Generator Type</InputLabel>
                        <Select value={selectedOption} onChange={handleSelectChange}>
                            <MenuItem value="GART">GART</MenuItem>
                            <MenuItem value="BITE">BITE</MenuItem>
                        </Select>
                    </FormControl>
                </Box>
            </DialogContent>

            <DialogActions>
                <Button onClick={handleClose} color="primary">
                    Cancel
                </Button>
                <Button onClick={handleGenerateClick} color="primary">
                    Generate
                </Button>
            </DialogActions>
        </Dialog>
    );
}
