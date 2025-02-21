import React, {useCallback, useEffect, useState} from 'react';
import {Card, CardContent, Chip, Grid2 as Grid, Typography} from '@mui/material';
import {useNavigate} from 'react-router-dom';
import AnimalCardOptionsMenu from "./AnimalCardOptionsMenu";

// Define AnimalCardProps for the component
/**
 * Type definition for the props to be passed to an AnimalCard component.
 *
 * @typedef {Object} AnimalCardProps
 * @property {string} animalID - Unique identifier for the animal.
 * @property {string} animalName - Name of the animal.
 * @property {string} animalDOB - Date of birth of the animal, represented as a string.
 * @property {string} animalType - Type or species of the animal.
 * @property {function} onClick - Callback function to be invoked when the card is clicked.
 * @property {function} onDeleteSuccess - Callback function to be invoked when the delete operation is successful.
 */
export type AnimalCardProps = {
    animalID: string;
    animalName: string;
    animalDOB: string; // animalDOB will be passed as a string
    animalType: string;
    onClick: () => void; // Add onClick prop
    onDeleteSuccess: () => void; // Add onDeleteSuccess prop
};

/**
 * AnimalCard is a React functional component that displays a card with information about an animal.
 *
 * @component
 * @param {Object} props - The component's props.
 * @param {number} props.animalID - The unique identifier for the animal.
 * @param {string} props.animalName - The name of the animal.
 * @param {string} props.animalDOB - The date of birth of the animal.
 * @param {string} props.animalType - The type/species of the animal.
 * @param {Function} props.onClick - Handler for when the card is clicked.
 * @param {Function} props.onDeleteSuccess - Handler for when the animal is successfully deleted.
 *
 * @returns {JSX.Element} Returns a styled card component containing animal details such as image, name, date of birth, type, and options menu.
 */
const AnimalCard: React.FC<AnimalCardProps> = ({
                                                   animalID,
                                                   animalName,
                                                   animalDOB,
                                                   animalType,
                                                   onClick,
                                                   onDeleteSuccess
                                               }) => {
    const navigate = useNavigate();
    const [animalImage, setAnimalImage] = useState<string>('');

    const handleGetAnimalImage = useCallback(async () => {
        try {// Use local fallback images rather than fetching missing images from the API
            const fallbackImage = `/assets/images/fallback/${animalType.toLowerCase()}.png`;
            setAnimalImage(fallbackImage);

        } catch (error) {
            console.error('Error fetching animal image:', error);
        }
    }, [animalType]);

    useEffect(() => {
        handleGetAnimalImage();
    }, [handleGetAnimalImage]);

    const formattedDOB = new Date(animalDOB).toLocaleDateString();

    const handleDeleteSuccess = () => {
        // Refresh the page or go backwards
        onDeleteSuccess();
    };


    return (
        <Card
            variant="outlined"
            sx={{height: '100%', cursor: 'pointer', ':hover': {backgroundColor: 'whiteSmoke'}}}
            onClick={onClick}
        >
            <CardContent sx={{padding: '10px', width: '100%'}}>
                <Grid container sx={{width: '100%'}}>
                    <Grid size={3}>
                        {animalImage && (
                            <img
                                src={animalImage}
                                alt={animalName}
                                style={{width: '60px', height: '60px', borderRadius: '50%'}}
                            />
                        )}
                    </Grid>
                    <Grid size={6}>
                        <Typography variant="subtitle2" gutterBottom fontWeight={'bold'}>
                            {animalName}
                        </Typography>
                        <Typography variant="body2" color="textSecondary">
                            DoB: {formattedDOB}
                        </Typography>
                        <Chip label={animalType}/>
                    </Grid>
                    <Grid size={3}>
                        <AnimalCardOptionsMenu animalId={animalID} onDeleteSuccess={handleDeleteSuccess}/>
                    </Grid>
                </Grid>
            </CardContent>
        </Card>
    );
};

export default AnimalCard;
