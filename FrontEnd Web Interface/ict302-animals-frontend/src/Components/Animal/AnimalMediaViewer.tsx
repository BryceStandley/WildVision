import React from "react";
import {Box, Card, CardContent, CardMedia, Grid2 as Grid, Typography} from "@mui/material";
import ReactPlayer from "react-player";
import {AccessTime, Folder} from "@mui/icons-material";
import moment from 'moment';

/**
 * Interface representing properties for an animal media viewer component.
 *
 * @interface AnimalMediaViewerProps
 */
interface AnimalMediaViewerProps {
    graphicId: string;
    graphicFilePath: string;
    isImage: boolean;
    uploadedDate: string;
    fileSize: string;
}

/**
 * AnimalMediaViewer is a React functional component that renders a media file (either an image or a video)
 * related to animals. It uses the file's metadata to display information such as its upload date and file size.
 *
 * The component distinguishes between images and videos to render them appropriately using
 * different UI elements.
 *
 * @param {Object} props - The properties object.
 * @param {string} props.graphicId - The unique identifier for the media file.
 * @param {string} props.graphicFilePath - The file path or URL for the media file.
 * @param {boolean} props.isImage - A boolean indicating if the media file is an image (`true`) or a video (`false`).
 * @param {string} props.uploadedDate - The date and time when the media file was uploaded in the ISO format 'YYYY-MM-DDTHH:mm:ss'.
 * @param {string} props.fileSize - The size of the media file.
 */
const AnimalMediaViewer: React.FC<AnimalMediaViewerProps> = ({
                                                                 graphicId,
                                                                 graphicFilePath,
                                                                 isImage,
                                                                 uploadedDate,
                                                                 fileSize
                                                             }) => {

    const formatUploadedDate = () => {
        let date = moment(uploadedDate, 'YYYY-MM-DDTHH:mm:ss');
        return date.format('DD/MM/YYYY');
    }

    return (
        <div key={graphicId}>
            {isImage ? (
                <Box
                    component="img"
                    src={graphicFilePath}
                    alt={`Media ${graphicId}`}
                    sx={{width: '100%', height: 'auto'}}
                />
            ) : (
                <Card>
                    <CardMedia>
                        <ReactPlayer key={graphicId} width="100%" height="100%" url={graphicFilePath} controls/>
                    </CardMedia>
                    <CardContent>
                        <Grid container spacing={4}>
                            <Grid size={6}>
                                <Typography variant="subtitle1" sx={{color: 'text.secondary', textAlign: 'center'}}>
                                    <AccessTime sx={{paddingTop: '6px'}}/>Uploaded: {formatUploadedDate()}
                                </Typography>
                            </Grid>
                            <Grid size={6}>
                                <Typography variant="subtitle1" sx={{color: 'text.secondary', textAlign: 'center'}}>
                                    <Folder sx={{paddingTop: '6px'}}/>Size: {fileSize}
                                </Typography>
                            </Grid>
                        </Grid>
                    </CardContent>
                </Card>
            )}
        </div>
    );
};

export default AnimalMediaViewer;