import React from 'react';
import API from '../../Internals/API';

/**
 * Represents the properties for a DeleteGraphicButton component.
 *
 * @typedef {Object} DeleteGraphicButtonProps
 * @property {string} animaltoDelId - The ID of the animal associated with the graphic to be deleted.
 * @property {string} graphictoDelId - The ID of the graphic to be deleted.
 * @property {function} [onDeleteSuccess] - Optional callback function to be executed after a successful delete operation.
 */
interface DeleteGraphicButtonProps {
    animaltoDelId: string;
    graphictoDelId: string;
    onDeleteSuccess?: () => void;
}

/**
 * DeleteGraphicButton component is responsible for handling the deletion
 * of a graphic associated with a specific animal. When the button is clicked,
 * it triggers an asynchronous operation to delete the graphic.
 *
 * Props:
 * - `animaltoDelId` (string): The ID of the animal to which the graphic belongs.
 * - `graphictoDelId` (string): The full URL of the graphic to be deleted.
 * - `onDeleteSuccess` (function): Optional callback function to be called upon successful deletion of the graphic.
 *
 * The component displays a button with the text "Delete Graphic". Upon clicking the button,
 * it initiates an HTTP DELETE request to the server to remove the specified graphic.
 * If the operation succeeds, a success message is displayed and the optional success callback is invoked.
 * If the operation fails, an error message is displayed, either from the server response or a generic error message.
 *
 * The button is styled with red text to indicate the delete action.
 */
const DeleteGraphicButton: React.FC<DeleteGraphicButtonProps> = ({animaltoDelId, graphictoDelId, onDeleteSuccess}) => {
    const handleDelete = async () => {
        const graphicFileName = graphictoDelId.split('/').pop();  // Extract the file name from the full URL
        if (!graphicFileName) {
            alert('Invalid graphic URL');
            return;
        }

        try {
            const deleteUrl = API.DeleteGraphic(animaltoDelId, graphicFileName);  // Pass only the file name
            const response = await fetch(deleteUrl, {
                method: 'DELETE',
            });

            if (response.ok) {
                alert('Graphic deleted successfully');
                if (onDeleteSuccess) {
                    onDeleteSuccess();  // Call the success callback if provided
                }
            } else {
                try {
                    const errorData = await response.json();
                    alert(`Failed to delete graphic: ${errorData.message}`);
                } catch {
                    alert('Failed to delete graphic. Server returned an invalid response.');
                }
            }
        } catch (error) {
            console.error('Error deleting graphic:', error);
            alert('An error occurred while deleting the graphic.');
        }
    };

    return (
        <button onClick={handleDelete} style={{color: 'red'}}>
            Delete Graphic
        </button>
    );
};

export default DeleteGraphicButton;