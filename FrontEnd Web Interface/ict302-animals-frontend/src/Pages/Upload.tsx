import React, {useContext} from 'react';
import {Box} from '@mui/material';
import {FrontendContext} from '../Internals/ContextStore';
import UploadPrompt from '../Components/Upload/UploadPrompt';
import {UploadProps} from '../Components/Upload/UploadProps';
import RecentlyUploaded from '../Components/Upload/RecentlyUploaded';

/**
 * The Upload component is responsible for handling file uploads and displaying recently uploaded items.
 *
 * @component
 * @param {UploadProps} props - The component props
 * @param {function} props.onUploadSuccess - Callback function to execute when an upload is successful
 *
 * @returns {JSX.Element} The rendered Upload component
 *
 * @description
 * This component manages the upload functionality and displays a list of recently uploaded items.
 * It includes a button for uploading files, and upon a successful upload, it triggers a refresh
 * of the thumbnails to reflect the newly uploaded files. This is achieved by toggling the
 * refreshThumbnails state which is passed down to the RecentlyUploaded component.
 *
 * @example
 * <Upload onUploadSuccess={handleUploadSuccess} />
 */
const Upload: React.FC<UploadProps> = ({onUploadSuccess}) => {

    const [refreshThumbnails, setRefreshThumbnails] = React.useState(false);
    const frontendContext = useContext(FrontendContext);


    // Function to trigger a refresh of the thumbnails
    /**
     * A function that toggles the state of `refreshThumbnails` to trigger a refresh of thumbnail images.
     *
     * When invoked, this function switches the boolean value of `refreshThumbnails` to its opposite,
     * thereby signaling that the thumbnails should be re-fetched or refreshed.
     *
     * Usage of this function is intended for scenarios where there is a need to update or reload thumbnail images,
     * for example, after changes to the content they represent have occurred.
     */
    const triggerThumbnailRefresh = () => {
        setRefreshThumbnails(!refreshThumbnails);  // Toggle state to trigger re-fetch
    };

    return (
        <Box sx={{
            width: '100%', display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
        }}>
            <Box sx={{width: '100vw', padding: '20px', textAlign: 'center',}}>
                {/* Upload Button */}
                <Box
                    sx={{
                        background: 'linear-gradient(125deg, rgba(255,105,105,0.9), rgba(173,216,230,0.6))',
                        padding: '10px',
                        borderRadius: '10px',
                        width: '100%',
                        boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
                        marginBottom: '30px',
                        color: '#000',
                    }}
                >
                    <UploadPrompt onUploadSuccess={triggerThumbnailRefresh}/>
                </Box>

                {/* Recently Uploaded Section */}
                <Box sx={{maxWidth: '1050px', margin: '0 auto'}}>
                    <RecentlyUploaded triggerRefresh={refreshThumbnails}/>
                </Box>

            </Box>
        </Box>
    );
};

export default Upload;
