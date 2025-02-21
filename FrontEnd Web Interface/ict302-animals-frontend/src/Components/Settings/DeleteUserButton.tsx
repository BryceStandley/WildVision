import React, {useContext} from 'react';
import API from '../../Internals/API';
import Button from "@mui/material/Button";
import {FrontendContext} from '../../Internals/ContextStore'; // Assuming the context is here

/**
 * DeleteUserButton is a React functional component that renders a button
 * which, when clicked, will delete a user by their ID. The user ID is retrieved
 * from the FrontendContext and passed to an API call that performs the deletion.
 *
 * @param {Object} props - The props for the component.
 * @param {Function} [props.onDeleteSuccess] - Optional callback function that
 * will be called if the user deletion is successful.
 */
const DeleteUserButton: React.FC<{ onDeleteSuccess?: () => void }> = ({onDeleteSuccess}) => {
    const frontendContext = useContext(FrontendContext);  // Access the frontend context
    const userId = frontendContext.user.contextRef.current.userId;  // Get the user email from context

    const handleDelete = async () => {
        try {
            console.log("User ID from backend:", userId);
            const deleteUrl = API.DeleteUserByID(userId);  // Pass the email directly to API.DeleteUser
            const response = await fetch(deleteUrl, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.ok) {
                if (onDeleteSuccess) {
                    onDeleteSuccess();  // Call the success callback if provided
                }
                alert('User deleted successfully');
            } else {
                const errorData = await response.json();
                alert(`Failed to delete User: ${errorData.message}`);
            }
        } catch (error) {
            console.error('Error deleting user:', error);
            alert('An error occurred while deleting the user.');
        }
    };

    return (
        <Button
            variant="contained"
            color="error"
            onClick={handleDelete}
        >
            Confirm Delete Account
        </Button>
    );
};

export default DeleteUserButton;