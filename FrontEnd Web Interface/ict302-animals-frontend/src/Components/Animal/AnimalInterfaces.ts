import React from "react";

/**
 * Interface representing a Graphic entity.
 *
 * @property {string} gpcid - Unique identifier for the graphic.
 * @property {string} gpcName - Name of the graphic.
 * @property {string} gpcDateUpload - Date when the graphic was uploaded.
 * @property {string} filePath - File path where the graphic is stored.
 * @property {string} animalID - Identifier of the associated animal.
 * @property {number} gpcSize - Size of the graphic file in bytes.
 */
export interface Graphic {
    gpcid: string;
    gpcName: string;
    gpcDateUpload: string;
    filePath: string;
    animalID: string;
    gpcSize: number;
}

/**
 * The `Animal` interface represents an animal entity with essential properties
 * such as ID, name, type, date of birth, associated graphics, and an optional
 * photo file name.
 *
 * @property {string} animalID - Unique identifier for the animal.
 * @property {string} animalName - The name of the animal.
 * @property {string} animalType - The type/species of the animal.
 * @property {string} animalDOB - Date of birth of the animal in ISO format.
 * @property {Graphic[]} graphics - Array of graphic objects representing media files.
 * @property {string} [photoFileName] - Optional filename for the animal's photo.
 */
export interface Animal {
    animalID: string;
    animalName: string;
    animalType: string;
    animalDOB: string;
    graphics: Graphic[]; // Updated to include media files as graphics array
    photoFileName?: string;
}

/**
 * Properties for the AnimalDetails component.
 *
 * @interface AnimalDetailsProps
 * @property {string | null} animalId - The unique identifier of the animal or null if no animal is selected.
 * @property {number} activeTab - Index of the currently active tab in the AnimalDetails component.
 * @property {React.Dispatch<React.SetStateAction<number>>} setActiveTab - Function to set the active tab.
 * @property {React.Dispatch<React.SetStateAction<string | null>>} setSelectedAnimalId - Function to set the selected animal's unique identifier or null.
 */
export interface AnimalDetailsProps {
    animalId: string | null;
    activeTab: number;
    setActiveTab: React.Dispatch<React.SetStateAction<number>>;
    setSelectedAnimalId: React.Dispatch<React.SetStateAction<string | null>>;
}

/**
 * This interface defines the properties for the GraphicsOptionMenu component.
 *
 * @typedef {Object} GraphicsOptionMenuProps
 * @property {Graphic} graphic - The graphic object to be used in the menu.
 */
export interface GraphicsOptionMenuProps {
    graphic: Graphic;
}

/**
 * Represents the properties for the options menu component of an animal card.
 *
 * @interface
 * @property {string} animalId - The unique identifier for the animal.
 * @property {() => void} [onDeleteSuccess] - An optional callback function to be executed upon successful deletion of the animal.
 */
export interface AnimalCardOptionsMenuProps {
    animalId: string;
    onDeleteSuccess?: () => void;  // Optional callback for success
}