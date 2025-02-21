import React, {useState} from "react";
import {Box, Button, Typography} from "@mui/material";
import ModelViewer from "../ModelViewer/ModelViewer";

/**
 * ViewGenerateButton component provides a button to toggle the visibility
 * of a model viewer. When the button is clicked, it toggles between showing
 * and hiding the model viewer. If the model viewer is visible, it displays
 * a 3D model loaded from a specified path.
 *
 * @return {JSX.Element} The ViewGenerateButton component.
 */
export default function ViewGenerateButton() {
    const [showModelViewer, setShowModelViewer] = useState(false);


    const modelPath = "/3d_test_files/horse_walk.glb"; //will obvs have to change


    const ToggleViewer = () => {
        if (showModelViewer) {
            setShowModelViewer(false);
        } else {
            setShowModelViewer(true);
        }

    }

    return (
        <>
            {!showModelViewer ?
                (
                    <Button component="label"
                            variant="contained" onClick={ToggleViewer}>
                        View the Generation from this Video
                    </Button>
                ) : (
                    <Button component="label"
                            variant="contained" onClick={ToggleViewer}>
                        Close Generation
                    </Button>)}


            {showModelViewer && (
                <Box>
                    <Typography variant="h6" sx={{marginBottom: 2}}>
                        Generated Video:
                    </Typography>
                    <ModelViewer modelPath={modelPath}/>
                </Box>
            )}
        </>
    );
}
