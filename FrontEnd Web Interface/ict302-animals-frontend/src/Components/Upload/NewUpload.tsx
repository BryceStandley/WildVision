// Combined NewUpload.tsx
import React, {useContext, useEffect, useState} from 'react';
import {
  Alert,
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Snackbar,
  Typography
} from '@mui/material';
import API from '../../Internals/API';
import {FrontendContext} from '../../Internals/ContextStore';
import {updateLoggedInUserAnimals} from "../User/userUtils";

/**
 * Represents the properties required for the NewUpload component.
 */
interface NewUploadProps {
    open: boolean;
    handleClose: (canceled: boolean) => void;
    animalID?: string; // Optional for existing animals
    animalDetails?: { // For new animals or if `animalID` is included here
        animalID?: string;
        animalName: string;
        animalType: string;
        animalDOB?: string;
        dateOfBirth?: string;
    };
    filesToUpload: File[] | null;
    onUploadSuccess?: () => void;
}

/**
 * Handles the uploading of files related to an animal's details.
 *
 * @param {Object} props - The properties for the NewUpload component.
 * @param {boolean} props.open - Indicates whether the upload dialog is open.
 * @param {Function} props.handleClose - Function to handle the closing of the upload dialog.
 * @param {string} [props.animalID] - The ID of the existing animal to upload files for.
 * @param {Object} [props.animalDetails] - The details of the new animal to upload files for.
 * @param {string} [props.animalDetails.animalID] - The ID of the animal.
 * @param {string} [props.animalDetails.animalName] - The name of the animal.
 * @param {string} [props.animalDetails.animalType] - The type of the animal.
 * @param {string} [props.animalDetails.animalDOB] - The date of birth of the animal.
 * @param {string} [props.animalDetails.dateOfBirth] - An alternate date of birth for the animal.
 * @param {File[]} props.filesToUpload - The list of files to be uploaded.
 * @param {Function} [props.onUploadSuccess] - Callback function to be called on successful upload of files.
 * @return {React.Component} The NewUpload component.
 */
export default function NewUpload({
                                      open,
                                      handleClose,
                                      animalID,
                                      animalDetails,
                                      filesToUpload,
                                      onUploadSuccess
                                  }: NewUploadProps) {
    const [isSnackbarOpen, setIsSnackbarOpen] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [isUploading, setIsUploading] = useState(false);
    const [uploadSuccess, setUploadSuccess] = useState<boolean | null>(null);
    const frontendContext = useContext(FrontendContext);
    const userid = frontendContext.user.contextRef.current.userId;

    const handleFileUpload = async () => {
        if (!filesToUpload || filesToUpload.length === 0) {
            setErrorMessage('No files selected for upload.');
            setIsSnackbarOpen(true);
            return;
        }

        setIsUploading(true);
        const formData = new FormData();
        formData.append('userId', userid);

        const effectiveAnimalID = animalID || animalDetails?.animalID;
        let endpoint = API.Upload();
        if (effectiveAnimalID) {
            formData.append('animalID', effectiveAnimalID);
            endpoint += `/existing`;
            console.log("Uploading to existing animal endpoint:", endpoint, "Animal ID:", effectiveAnimalID);
        } else if (animalDetails) {
            const {animalName, animalType, animalDOB, dateOfBirth} = animalDetails;
            const dob = animalDOB || dateOfBirth;
            if (!animalName || !animalType || !dob) {
                setErrorMessage('Missing animal details. Please provide all required information.');
                setIsSnackbarOpen(true);
                setIsUploading(false);
                return;
            }
            formData.append('animalName', animalName);
            formData.append('animalType', animalType);
            formData.append('dateOfBirth', dob);
            console.log("Uploading new animal with details:", animalDetails);
        } else {
            setErrorMessage('Missing animal identification.');
            setIsSnackbarOpen(true);
            setIsUploading(false);
            return;
        }

        filesToUpload.forEach((file, index) => {
            formData.append('files', file);
            console.log(`File ${index + 1} appended:`, file.name);
        });

        console.log("Final endpoint for request:", endpoint);

        try {
            const response = await fetch(endpoint, {method: 'POST', body: formData});
            console.log("Request sent to:", endpoint);

            if (!response.ok) {
                const errorData = await response.json();
                setErrorMessage(errorData.message || 'Upload failed. Unsupported file type or invalid data.');
                setIsSnackbarOpen(true);
            } else {
                const data = await response.json();
                console.log('Upload success:', data);

                await updateLoggedInUserAnimals(frontendContext);
                onUploadSuccess && onUploadSuccess();

                setUploadSuccess(true);
                setTimeout(() => {
                    handleClose(false);
                    setIsUploading(false);
                }, 1000);
            }
        } catch (error) {
            console.error('Upload error:', error);
            setErrorMessage('Upload failed. Please try again.');
            setIsSnackbarOpen(true);
        } finally {
            setIsUploading(false);
        }
    };

    useEffect(() => {
        if (open && filesToUpload && filesToUpload.length > 0) {
            handleFileUpload();
        }
    }, [open, filesToUpload]);

    const handleSnackbarClose = () => setIsSnackbarOpen(false);

    return (
        <>
            <Dialog open={open} onClose={() => handleClose(true)}>
                <DialogTitle>{isUploading ? 'Uploading...' : 'Upload Videos'}</DialogTitle>
                <DialogContent>
                    {isUploading ? (
                        <Box sx={{display: 'flex', flexDirection: 'column', alignItems: 'center'}}>
                            <CircularProgress/>
                            <Typography variant="body1" mt={2}>Uploading your video for the
                                animal: {animalDetails?.animalName || ''}</Typography>
                            {uploadSuccess &&
                                <Typography variant="body1" mt={2} color="success.main">Upload Successful!</Typography>}
                        </Box>
                    ) : (
                        <Typography variant="body1">Preparing to upload your videos...</Typography>
                    )}
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => handleClose(true)} disabled={isUploading}>Cancel</Button>
                </DialogActions>
            </Dialog>

            <Snackbar open={isSnackbarOpen} autoHideDuration={6000} onClose={handleSnackbarClose}>
                <Alert onClose={handleSnackbarClose} severity="error" sx={{width: '100%'}}>{errorMessage}</Alert>
            </Snackbar>
        </>
    );
}
