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
  TextField,
  Typography,
} from "@mui/material";
import React, {useContext, useState} from "react";
import {SelectChangeEvent} from '@mui/material/Select';
import {FrontendContext} from "../../Internals/ContextStore"; // Import context
import {Animal} from '../Animal/AnimalInterfaces'; // Assuming Animal includes animalID

interface AnimalDetails {
    animalID?: string;
    animalName: string;
    animalType?: string;
    animalDOB?: string;
    file?: File;
}

interface NewAnimalProps {
    open: boolean;
    handleClose: () => void;
    addNewAnimal: (animalDetails: AnimalDetails) => void; // Add the addNewAnimal prop here
    requireFile?: boolean; // New prop to indicate if file selection is required
}

const animalTypesList = ["Dog", "Cat", "Mammal", "Bird", "Reptile", "Fish"];

/**
 * NewAnimal is a React functional component that renders a dialog for adding a new animal
 * or uploading a video for an existing animal.
 *
 * The component uses various React hooks and context to manage the state and behavior,
 * including toggling between new and existing animals, handling form input changes,
 * and more.
 *
 * Props:
 * - open: Boolean indicating if the dialog is open.
 * - handleClose: Function to handle closing the dialog.
 * - addNewAnimal: Function to handle adding a new animal or uploading a video for an existing animal.
 * - requireFile: Optional boolean to specify if a file upload is required. Defaults to false.
 *
 * The component contains several internal state variables and functions:
 * - State variables to manage various input fields and UI states such as isNewAnimal, animalName, animalDOB, etc.
 * - Functions to handle changing inputs, adding new animal types, selecting files, etc.
 */
const NewAnimal: React.FC<NewAnimalProps> = ({
                                                 open,
                                                 handleClose,
                                                 addNewAnimal,
                                                 requireFile = false, // Default to false
                                             }) => {
    const frontendContext = useContext(FrontendContext); // Access context for existing animals
    const existingAnimals: Animal[] = frontendContext.user.contextRef.current.userAnimals || []; // List of existing animals with animalID
    const userId = frontendContext.user.contextRef.current.userId; // Assuming userID is available

    const formatDate = (date: string): string => {
        return date.split("T")[0]; // Ensure date is in yyyy-MM-dd format
    };

    let currentDate = new Date();
    const [isNewAnimal, setIsNewAnimal] = useState(true); // State to toggle between new and existing animal
    const [animalName, setAnimalName] = useState<string>("");
    const [animalDOB, setAnimalDOB] = useState<string>(formatDate(currentDate.toISOString()));
    const [animalType, setAnimalType] = useState<string>("Dog");
    const [animalTypes, setAnimalTypes] = useState<string[]>(animalTypesList);
    const [isAddNewTypeDialogOpen, setIsAddNewTypeDialogOpen] = useState<boolean>(false);
    const [newAnimalType, setNewAnimalType] = useState<string>("");
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [selectedExistingAnimal, setSelectedExistingAnimal] = useState<string>(""); // Selected existing animal

    const handleAnimalTypeChange = (event: SelectChangeEvent<string>) => {
        setAnimalType(event.target.value as string);
    };

    const handleAddNewTypeOpen = () => {
        setIsAddNewTypeDialogOpen(true);
    };

    const handleAddNewTypeClose = () => {
        setIsAddNewTypeDialogOpen(false);
        setNewAnimalType("");
    };

    const handleAddNewType = () => {
        if (newAnimalType && !animalTypes.includes(newAnimalType)) {
            setAnimalTypes([...animalTypes, newAnimalType]);
            setAnimalType(newAnimalType);
        }
        handleAddNewTypeClose();
    };

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setSelectedFile(event.target.files[0]);
        }
    };

    // NewAnimal.tsx
    const handleGenerate = () => {
        const formattedDOB = formatDate(animalDOB);

        if (isNewAnimal) {
            if (animalName && (!requireFile || selectedFile)) {
                addNewAnimal({
                    animalName,
                    animalType,
                    animalDOB: formattedDOB, // Use dateOfBirth to match ParentComponent
                    file: selectedFile || undefined,
                });
                handleClose();
            } else {
                alert("Please provide all required details for the new animal.");
            }
        } else if (selectedExistingAnimal) {
            const existingAnimal = existingAnimals.find(animal => animal.animalID === selectedExistingAnimal);
            if (existingAnimal) {
                addNewAnimal({
                    animalID: existingAnimal.animalID,
                    animalName: existingAnimal.animalName,
                    animalType: existingAnimal.animalType,
                    animalDOB: formatDate(existingAnimal.animalDOB), // Use dateOfBirth
                    file: selectedFile || undefined,
                });
                handleClose();
            } else {
                alert("Please select a valid existing animal.");
            }
        } else {
            alert("Please fill in all required fields.");
        }
    };


    return (
        <>
            <Dialog open={open} onClose={handleClose} maxWidth="md" fullWidth>
                <DialogTitle>{isNewAnimal ? "Add a new animal" : "Upload Video for Existing Animal"}</DialogTitle>
                <DialogContent>
                    <Box display="flex" justifyContent="center" mb={2}>
                        <Button variant="outlined" onClick={() => setIsNewAnimal(true)} sx={{mr: 2}}>
                            Upload Video for New Animal
                        </Button>
                        <Button variant="outlined" onClick={() => setIsNewAnimal(false)}>
                            Upload Video for Existing Animal
                        </Button>
                    </Box>

                    <Typography variant="body1" gutterBottom>
                        {isNewAnimal ? "Please enter the details of your new animal." : "Select an existing animal to upload a video."}
                    </Typography>

                    <Box mt={2}>
                        {isNewAnimal ? (
                            <>
                                <TextField
                                    label="Animal Name"
                                    variant="standard"
                                    fullWidth
                                    value={animalName}
                                    onChange={(e) => setAnimalName(e.target.value)}
                                    margin="normal"
                                    required
                                />

                                <TextField
                                    label="Date of Birth"
                                    variant="standard"
                                    fullWidth
                                    type="date"
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    value={animalDOB}
                                    onChange={(e) => setAnimalDOB(e.target.value)}
                                    margin="normal"
                                    required={requireFile}
                                />

                                <FormControl fullWidth margin="normal">
                                    <InputLabel id="animal-type-label" variant="standard" shrink={true}>
                                        Animal Type
                                    </InputLabel>
                                    <Select
                                        labelId="animal-type-label"
                                        value={animalType || ''}
                                        onChange={handleAnimalTypeChange}
                                        label="Animal Type"
                                        required={requireFile}
                                    >
                                        {animalTypes.map((type) => (
                                            <MenuItem key={type} value={type}>
                                                {type}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>

                                <Button
                                    onClick={handleAddNewTypeOpen}
                                    color="primary"
                                    variant="outlined"
                                    sx={{marginTop: "10px"}}
                                >
                                    Add New Animal Type
                                </Button>
                            </>
                        ) : (
                            <FormControl fullWidth margin="normal">
                                <InputLabel id="existing-animal-label" variant="standard">
                                    Select Existing Animal
                                </InputLabel>
                                <Select
                                    labelId="existing-animal-label"
                                    value={selectedExistingAnimal}
                                    onChange={(e) => {
                                        setSelectedExistingAnimal(e.target.value);
                                        console.log('Selected Existing Animal ID:', e.target.value);
                                    }}
                                    label="Select Existing Animal"
                                >
                                    {existingAnimals.map((animal) => (
                                        <MenuItem key={animal.animalID} value={animal.animalID}>
                                            {animal.animalName}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>

                        )}

                        {requireFile && (
                            <Box mt={2}>
                                <Typography variant="body1" gutterBottom>
                                    Select a video file for the animal:
                                </Typography>
                                <input
                                    type="file"
                                    accept="video/*"
                                    onChange={handleFileChange}
                                />
                                {selectedFile && (
                                    <Typography variant="body2">
                                        Selected file: {selectedFile.name}
                                    </Typography>
                                )}
                            </Box>
                        )}
                    </Box>
                </DialogContent>

                <DialogActions>
                    <Button onClick={handleGenerate} color="primary" variant="contained">
                        Submit
                    </Button>
                    <Button onClick={handleClose} color="primary" variant="outlined">
                        Close
                    </Button>
                </DialogActions>
            </Dialog>

            <Dialog open={isAddNewTypeDialogOpen} onClose={handleAddNewTypeClose}>
                <DialogTitle>Add New Animal Type</DialogTitle>
                <DialogContent>
                    <TextField
                        label="New Animal Type"
                        variant="standard"
                        fullWidth
                        value={newAnimalType}
                        onChange={(e) => setNewAnimalType(e.target.value)}
                        margin="normal"
                    />
                </DialogContent>

                <DialogActions>
                    <Button onClick={handleAddNewType} color="primary" variant="contained">
                        Add
                    </Button>
                    <Button onClick={handleAddNewTypeClose} color="secondary" variant="outlined">
                        Cancel
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
};

export default NewAnimal;
