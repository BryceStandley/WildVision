import React from "react";

import {Box, Card, CardContent} from '@mui/material';

import ModelRenderer from "./components/ModelRenderer";
import Typography from "@mui/material/Typography";

interface ModelViewerProps {
    modelPath: string | undefined;
}

/**
 * Component for displaying a 3D model viewer.
 *
 * The `ModelViewer` component takes `modelPath` as a prop and renders a 3D model
 * viewer if the `modelPath` is provided and valid. If no `modelPath` is given or it
 * is an empty string, the component will display a message indicating that no model
 * is selected.
 *
 * Props:
 * - `modelPath` (string): The path to the 3D model file that needs to be rendered.
 *
 * @param {ModelViewerProps} props - The props for the component.
 * @param {string} props.modelPath - The path to the 3D model file.
 */
const ModelViewer: React.FC<ModelViewerProps> = ({modelPath}) => {

    return (
        <Box>
            {modelPath && modelPath !== "" ? (
                    <Card variant="outlined">
                        <ModelRenderer modelPath={modelPath}/>
                    </Card>
                )
                :
                (
                    <CardContent>
                        <Typography>No model selected.</Typography>
                    </CardContent>
                )}
        </Box>
    );
}

export default ModelViewer;