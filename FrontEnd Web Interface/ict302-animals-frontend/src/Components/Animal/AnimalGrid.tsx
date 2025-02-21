import React, {useContext, useEffect, useState} from 'react';
import {Box, Button, Grid2 as Grid, Menu, MenuItem, Typography} from '@mui/material';

import {useNavigate} from 'react-router-dom';
import AnimalCard from './AnimalCard';
import {FrontendContext} from '../../Internals/ContextStore';
import {Animal} from './AnimalInterfaces'
import {updateLoggedInUserAnimals} from "../User/userUtils";


/**
 * Props for the AnimalsGrid component.
 *
 * @typedef {Object} AnimalsGridProps
 * @property {boolean} triggerRefresh - A boolean flag that triggers a refresh of the animals grid.
 * @property {function} onAnimalClick - A callback function that is invoked when an animal is clicked.
 * @property {string} onAnimalClick.animalID - The ID of the clicked animal.
 * @property {string} onAnimalClick.animalName - The name of the clicked animal.
 */
interface AnimalsGridProps {
    triggerRefresh: boolean;
    onAnimalClick: (animalID: string, animalName: string) => void;

}

/**
 * AnimalsGrid is a functional React component that displays a grid of animal cards.
 *
 * The grid can be filtered by animal type using a drop-down menu. The component uses
 * the useContext hook to access user-specific data, such as the list of user's animals,
 * from a global context.
 *
 * It fetches and updates the animals' data asynchronously and maintains several state
 * variables to manage the list of animals, their types, and filter selections.
 *
 * Props:
 * @param {Function} triggerRefresh - Function to trigger a refresh of the grid data.
 * @param {Function} onAnimalClick - Callback function to handle click events on an animal card.
 *
 * @returns {JSX.Element} The rendered AnimalsGrid component.
 */
const AnimalsGrid: React.FC<AnimalsGridProps> = ({triggerRefresh, onAnimalClick}) => {
    const frontendContext = useContext(FrontendContext);
    const userAnimals = frontendContext.user.contextRef.current.userAnimals;
    const [animals, setAnimals] = useState<Animal[]>(userAnimals);
    const [filteredAnimals, setFilteredAnimals] = useState<Animal[]>([]); // For filtered animals
    const [animalTypes, setAnimalTypes] = useState<string[]>([]); // For animal types
    const [selectedAnimalType, setSelectedAnimalType] = useState<string>('All'); // Default filter
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null); // State for filter menu

    const navigate = useNavigate();


    const userId = frontendContext.user.contextRef.current.userId; // Get userId from context

    // Fetch animal IDs and details for the user
    const fetchAnimalsData = async () => {
        await updateLoggedInUserAnimals(frontendContext); // Update userAnimals in the context
        const a = frontendContext.user.contextRef.current.userAnimals;
        setAnimals(a);

        if (animals !== null && animals.length > 0) {
            // Filter out duplicate animals by checking their IDs
            const uniqueAnimals = animals.filter(
                (animal, index, self) => index === self.findIndex((a) => a.animalID === animal.animalID)
            );
            setFilteredAnimals(uniqueAnimals);

            const types = Array.from(new Set(uniqueAnimals.map((animal) => animal.animalType)));
            setAnimalTypes(types);
        } else {
            setFilteredAnimals([]);
        }
    };

    //TODO: There is a bug here that causes the users animal data to be refreshed non stop until the user navigates away from the grid
    useEffect(() => {
        if (userId) {
            fetchAnimalsData(); // Fetch animals when userId is available
        }
    }, [userId, animals, fetchAnimalsData]);

    // Handle filter change
    const handleFilterSelect = (type: string) => {
        setSelectedAnimalType(type);
        setFilteredAnimals(type === 'All' ? animals : animals.filter(animal => animal.animalType === type));
        setAnchorEl(null); // Close the filter menu
    };

    const handleFilterButtonClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget); // Open the filter menu
    };

    const handleMenuClose = () => {
        setAnchorEl(null); // Close the filter menu
    };

    //display="flex" alignItems="center"
    return (
        <Box display="flex" flexDirection="column" alignItems="center"
             sx={{width: '100%', maxWidth: {sm: '100%', md: '1700px'}}}>

            <Box sx={{marginBottom: '20px', textAlign: 'left'}}>
                <Button variant="outlined" onClick={handleFilterButtonClick}>
                    Filter by Animal Type
                </Button>
                <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={handleMenuClose}>
                    <MenuItem onClick={() => handleFilterSelect('All')}>All</MenuItem>
                    {animalTypes.map((type) => (
                        <MenuItem key={type} onClick={() => handleFilterSelect(type)}>{type}</MenuItem>
                    ))}
                </Menu>
            </Box>

            <Grid container spacing={2} sx={{width: '100%'}}>
                {filteredAnimals.length > 0 && filteredAnimals.map((animal, index) => (
                    <Grid size={{xs: 12, sm: 6, md: 4}} key={index}>
                        <AnimalCard
                            key={animal.animalID}
                            animalID={animal.animalID}
                            animalName={animal.animalName}
                            animalDOB={animal.animalDOB || ''}
                            animalType={animal.animalType}
                            onClick={() => onAnimalClick(animal.animalID, animal.animalName)}
                            onDeleteSuccess={fetchAnimalsData}
                        />
                    </Grid>
                ))}
                {filteredAnimals.length === 0 && (
                    <Grid size={{xs: 12, sm: 6, md: 4}}>
                        <Typography variant="h6">No animals found.</Typography>
                    </Grid>
                )}
            </Grid>
        </Box>
    );
};

export default AnimalsGrid;
