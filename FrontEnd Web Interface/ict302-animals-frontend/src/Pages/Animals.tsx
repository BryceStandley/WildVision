import React, {useContext, useEffect, useState} from 'react';
import {useNavigate} from 'react-router-dom';
import {FrontendContext} from "../Internals/ContextStore";
import {Box, Button, CircularProgress, Typography} from "@mui/material";
import AnimalsGrid from '../Components/Animal/AnimalGrid';
import AnimalDetails from "../Components/Animal/AnimalDetails"; // Adjust import based on your structure
import {Animal} from "../Components/Animal/AnimalInterfaces";

interface AnimalProps {
    actTab: number; // Tab index
}

/**
 * Animals is a React functional component that manages and displays user animals.
 * It supports navigating between an animal grid and animal details view based on the
 * current active tab.
 *
 * @param {AnimalProps} props - The props object.
 * @param {number} props.actTab - The initial active tab. 0 for AnimalGrid, 1 for AnimalDetails.
 *
 * Context:
 * - Uses FrontendContext to retrieve the user's animals and userId.
 *
 * State:
 * - `animals`: An array of Animal objects representing user animals.
 * - `selectedAnimalId`: The ID of the currently selected animal.
 * - `activeTab`: The current active tab, 0 for AnimalGrid, 1 for AnimalDetails.
 * - `loading`: A boolean indicating whether the data is being loaded.
 */
const Animals: React.FC<AnimalProps> = ({actTab}) => {
    const frontendContext = useContext(FrontendContext);
    const userAnimals = frontendContext.user.contextRef.current.userAnimals;
    const userId = frontendContext.user.contextRef.current.userId; // Get userId from context
    const navigate = useNavigate();
    const [animals, setAnimals] = useState<Animal[]>(userAnimals);
    const [selectedAnimalId, setSelectedAnimalId] = useState<string | null>(null);
    const [activeTab, setActiveTab] = useState(actTab); // 0 for AnimalGrid, 1 for AnimalDetails
    const [loading, setLoading] = useState<boolean>(true);

    /**
     * Asynchronously fetches the animal data associated with the currently
     * logged-in user and updates the state with the retrieved information.
     *
     * This function accesses the animal data from the `frontendContext`, which
     * contains the `current` reference to the user's context, specifically
     * their `userAnimals`. Once the data is retrieved, it sets this data into
     * local state and updates the loading state to false.
     *
     * @function fetchAnimalsData
     * @async
     */
    const fetchAnimalsData = async () => {
        const animalData = frontendContext.user.contextRef.current.userAnimals;
        setAnimals(animalData);
        setLoading(false);
    };

    useEffect(() => {
        if (userId) {
            fetchAnimalsData();
        }
    }, []);


    /**
     * Handles the event when an animal is clicked by the user. The function sets the selected animal's ID,
     * switches the active tab to display animal details, and navigates to the animal's detail page.
     *
     * @param {string} animalID - The unique identifier of the selected animal.
     */
    const handleAnimalClick = (animalID: string) => {
        setSelectedAnimalId(animalID);
        setActiveTab(1); // Switch to AnimalDetails view
        navigate(`/dashboard/animals/${animalID}`); // Navigate to details page
    };

    /**
     * AnimalGridContent is a functional component that renders a grid of animals.
     * It displays a loading indicator while data is being fetched,
     * and the actual grid of animals content once the data is ready.
     * The component triggers a refresh mechanism for the grid and handles animal click events.
     */
    const AnimalGridContent = () => {
        return (
            <>
                <h1>Animals</h1>
                {loading ? (<CircularProgress/>) : null}
                <AnimalsGrid triggerRefresh={true} onAnimalClick={handleAnimalClick}/>
            </>
        );
    }

    /**
     * AnimalDetailsContent is a functional React component that displays details about a selected animal.
     * It renders the AnimalDetails component, passing necessary props for managing the selected animal's ID and the active tab.
     *
     * @component
     *
     * @returns {JSX.Element} The rendered AnimalDetails component with the currently selected animal and active tab.
     *
     * @param {number} selectedAnimalId - The ID of the currently selected animal.
     * @param {string} activeTab - The currently active tab in the AnimalDetails component.
     * @param {function} setActiveTab - Function to update the active tab.
     * @param {function} setSelectedAnimalId - Function to update the selected animal ID.
     */
    const AnimalDetailsContent = () => {
        return (<AnimalDetails animalId={selectedAnimalId} activeTab={activeTab} setActiveTab={setActiveTab}
                               setSelectedAnimalId={setSelectedAnimalId}/>);
    }

    //TODO: Reset selected tab when navigating to animals page from animal details
    return (
        <div style={{width: '100%'}}>
            <Box display="flex" flexDirection="column" alignItems="center" paddingTop={2}>

                {loading ? (<CircularProgress/>) : null}

                {(animals.length > 0 && activeTab) === 0 && (
                    <AnimalGridContent/>
                )}

                {(animals.length > 0 && activeTab === 1 && selectedAnimalId) && (
                    <AnimalDetailsContent/>
                )}

                {animals.length === 0 && (
                    <Box sx={{width: '100%'}}>
                        <Typography variant="h6">No animals found</Typography>
                        <Button variant="contained" color="secondary" onClick={fetchAnimalsData}>Reload</Button>
                    </Box>
                )}
            </Box>
        </div>
    );
}

export default Animals;
