import React from 'react';
import About from '../../Pages/About';
import Account from '../../Pages/Account';
import Upload from '../../Pages/Upload';
import Settings from '../../Pages/Settings';
import MainGrid from './MainGrid';
import Animals from '../../Pages/Animals';


/**
 * Enumeration representing the different pages within a dashboard.
 *
 * @enum {number}
 * @readonly
 */
enum DashboardPages {
    Home, Upload, About, Settings, Help, Account, Animals, AnimalDetails
}

/**
 * Retrieves the corresponding DashboardPage enum based on the provided page name.
 *
 * @param {string} page - The name of the dashboard page.
 * @return {DashboardPages} - The DashboardPages enum value corresponding to the provided page name.
 */
export function getDashboardPageFromName(page: string): DashboardPages {
    switch (page) {
        case 'home':
            return DashboardPages.Home;
        case 'upload':
            return DashboardPages.Upload;
        case 'settings':
            return DashboardPages.Settings;
        case 'about':
            return DashboardPages.About;
        case 'account':
            return DashboardPages.Account;
        case 'animals':
            return DashboardPages.Animals;
        default:
            return DashboardPages.Home;
    }
}

/**
 * Retrieves the name associated with a specified dashboard page.
 *
 * @param {DashboardPages} page - The dashboard page whose name is to be retrieved.
 * @return {string} The name of the specified dashboard page.
 */
export function getNameFromDashboardPage(page: DashboardPages) {
    switch (page) {
        case DashboardPages.Home:
            return 'home';
        case DashboardPages.Upload:
            return 'upload';
        case DashboardPages.Settings:
            return 'settings';
        case DashboardPages.About:
            return 'about';
        case DashboardPages.Account:
            return 'Account';
        case DashboardPages.Animals:
            return 'animals';
    }
}

/**
 * Converts a DashboardPages enum value to its user-friendly string representation.
 *
 * @param {DashboardPages} page - The page enumerator from DashboardPages.
 * @return {string} The user-friendly name corresponding to the given DashboardPages enum value.
 */
export function getPrettyNameFromDashboardPage(page: DashboardPages) {
    switch (page) {
        case DashboardPages.Home:
            return 'Home';
        case DashboardPages.Upload:
            return 'Upload';
        case DashboardPages.Settings:
            return 'settings';
        case DashboardPages.About:
            return 'About';
        case DashboardPages.Account:
            return 'Account';
        case DashboardPages.Animals:
            return 'Animals';
        case DashboardPages.AnimalDetails:
            return "Animals";
    }
}

/**
 * Retrieves the dashboard page identifier from a given path.
 *
 * @param {string} path - The URL path from which to extract the dashboard page identifier.
 * @return {DashboardPages} The corresponding dashboard page identifier. Defaults to DashboardPages.Home if no valid page is found.
 */
export function getDashboardPageFromPath(path: string): DashboardPages {
    const page = path.split('/').pop();
    if (page) {
        return getDashboardPageFromName(page);
    }
    return DashboardPages.Home;
}

/**
 * Renders the appropriate dashboard page component based on the provided `DashboardPages` type.
 *
 * @param {DashboardPages} page - The specific dashboard page to render.
 * @param {() => void} [onUploadSuccess] - Optional callback function invoked upon a successful upload on the Upload page.
 * @return {React.Element} The React element corresponding to the specified dashboard page.
 */
export function getDashboardPageRenderFromDashboardPage(page: DashboardPages, onUploadSuccess?: () => void) {
    const handleUploadSuccess = onUploadSuccess ?? (() => {
    });
    switch (page) {
        case DashboardPages.Home:
            return <MainGrid/>;
        case DashboardPages.Upload:
            return <Upload onUploadSuccess={handleUploadSuccess}/>;
        case DashboardPages.Settings:
            return <Settings/>;
        case DashboardPages.About:
            return <About/>;
        case DashboardPages.Account:
            return <Account/>;
        case DashboardPages.Animals:
            return <Animals actTab={0}/>;
        default:
            return <MainGrid/>;
    }
}

export interface DashboardMenuProps {
    currentDashboardPage: DashboardPages;
    setCurrentDashboardPage: any;
}

export default DashboardPages;