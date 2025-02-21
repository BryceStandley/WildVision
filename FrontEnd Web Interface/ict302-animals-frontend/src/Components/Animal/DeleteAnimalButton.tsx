import React, {useContext} from 'react';
import API from '../../Internals/API';
import {IconButton, Tooltip} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import {FrontendContext} from "../../Internals/ContextStore";

/**
 * Properties for the DeleteAnimalButton component.
 *
 * @typedef {Object} DeleteAnimalButtonProps
 * @property {string} animalToDeleteId - The ID of the animal to delete.
 * @property {Function} [onDeleteSuccess] - Optional callback invoked upon successful deletion.
 */
interface DeleteAnimalButtonProps {
    animalToDeleteId: string;  // The ID of the animal to delete
    onDeleteSuccess?: () => void;  // Optional callback for success
}

/**
 * DeleteAnimalButton is a React functional component that renders a button used for deleting a specified animal.
 * When clicked, the button triggers an asynchronous operation to delete the animal from the database.
 * Upon successful deletion, an optional callback function `onDeleteSuccess` is invoked.
 *
 * @param {DeleteAnimalButtonProps} props - The properties passed to the component.
 * @param {string} props.animalToDeleteId - The ID of the animal to be deleted.
 * @param {function} [props.onDeleteSuccess] - Optional callback function to be called upon successful deletion.
 *
 * @component
 * @example
 * <DeleteAnimalButton
 *   animalToDeleteId="12345"
 *   onDeleteSuccess={handleSuccess}
 * />
 */
const DeleteAnimalButton: React.FC<DeleteAnimalButtonProps> = ({animalToDeleteId, onDeleteSuccess}) => {
    const frontendContext = useContext(FrontendContext);
    //@ts-ignore
    const handleDelete = (e) => {
        async function deleteAnimal() {
            try {
                const deleteUrl = API.DeleteAnimal(animalToDeleteId);
                const response = await fetch(deleteUrl, {
                    method: 'DELETE',
                });

                if (response.ok) {
                    if (onDeleteSuccess) {
                        onDeleteSuccess();  // Call the success callback if provided
                    }
                    alert('Animal and associated videos deleted successfully');
                } else {
                    const errorData = await response.json();
                    alert(`Failed to delete animal: ${errorData.message}`);
                }
            } catch (error) {
                console.error('Error deleting animal:', error);
                alert('An error occurred while deleting the animal.');
            }
        }

        e.stopPropagation();
        e.preventDefault();
        deleteAnimal();
        //TODO: Propagate this back to the animal grid to refresh the list.
        // The event propagation is stopped here to prevent the card from being clicked when the delete button is clicked.
    };

    return (
        <Tooltip title={'Delete Animal'} placement={'top'}>
            <IconButton onClick={handleDelete} sx={{color: 'darkred'}}>
                <DeleteIcon/>
            </IconButton>
        </Tooltip>
    );
};

export default DeleteAnimalButton;
