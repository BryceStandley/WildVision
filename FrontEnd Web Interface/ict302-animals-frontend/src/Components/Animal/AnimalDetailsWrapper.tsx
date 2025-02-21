import React from 'react';
import {useParams} from 'react-router-dom';
import AnimalDetails from './AnimalDetails';

/**
 * A functional component that serves as a wrapper for displaying animal details. It manages the active tab state
 * and retrieves the animal ID from the URL parameters.
 *
 * @typedef {Object} Props
 * @property {number} activeTab - The currently active tab index.
 * @property {React.Dispatch<React.SetStateAction<number>>} setActiveTab - Function to set the active tab index.
 *
 * @param {Props} props - The input props for the component.
 * @returns {JSX.Element} The rendered component that either displays an error message if no animal ID is found
 * or renders the AnimalDetails component with the provided and retrieved information.
 */
const AnimalDetailsWrapper: React.FC<{
    activeTab: number;
    setActiveTab: React.Dispatch<React.SetStateAction<number>>
}> = ({activeTab, setActiveTab}) => {
    const {animalId} = useParams<{ animalId: string }>();

    if (!animalId) {
        return <div>No animal ID found</div>;
    }

    return <AnimalDetails animalId={animalId} activeTab={activeTab} setActiveTab={setActiveTab}
                          setSelectedAnimalId={() => {
                          }}/>;
};

export default AnimalDetailsWrapper;