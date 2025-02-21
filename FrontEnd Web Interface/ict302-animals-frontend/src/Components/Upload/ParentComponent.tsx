// ParentComponent.tsx
import React, {useState} from "react";
import NewAnimal from "./NewAnimal";
import {Button} from "@mui/material";
import API from '../../Internals/API';

/**
 * ParentComponent is a functional React component that manages the UI for adding a new animal.
 * It includes the following functionality:
 * - A button to open a dialog for adding a new animal.
 * - A dialog component (`NewAnimal`) that accepts animal details and handles the form submission.
 *
 * State:
 * - `isNewAnimalDialogOpen`: A boolean state to manage the visibility of the new animal dialog.
 *
 * Handlers:
 * - `handleAddNewAnimal`: A function that processes the animal details form submission. It performs
 *   a HTTP POST request to upload the animal details along with a file. Handles success and error
 *   responses by alerting the user and performing other actions as needed.
 */
const ParentComponent: React.FC = () => {
    const [isNewAnimalDialogOpen, setIsNewAnimalDialogOpen] = useState<boolean>(false);

    const handleAddNewAnimal = (animalDetails: {
        animalName: string;
        animalType: string;
        dateOfBirth: string;
        file: File;
    }) => {
        const {animalName, animalType, dateOfBirth, file} = animalDetails;

        // Create FormData object
        const formData = new FormData();
        formData.append("file", file);
        formData.append("animalName", animalName);
        formData.append("animalType", animalType);
        formData.append("dateOfBirth", dateOfBirth);

        // Perform the upload
        fetch(API.Upload(), {
            method: "POST",
            body: formData,
        })
            .then((response) => {
                if (response.ok) {
                    // Handle success
                    alert("Animal added successfully!");
                    // Refresh the list or perform other actions
                } else {
                    // Handle error
                    response.json().then((data) => {
                        alert(`Error: ${data.message}`);
                    });
                }
            })
            .catch((error) => {
                // Handle error
                console.error("Error uploading file:", error);
                alert("An error occurred while uploading the file.");
            });
    };

    return (
        <>
            {/* Other content */}
            <Button
                onClick={() => setIsNewAnimalDialogOpen(true)}
                variant="contained"
                color="primary"
            >
                Add New Animal
            </Button>
            <NewAnimal
                open={isNewAnimalDialogOpen}
                handleClose={() => setIsNewAnimalDialogOpen(false)}
                addNewAnimal={handleAddNewAnimal}
            />
        </>
    );
};

export default ParentComponent;
