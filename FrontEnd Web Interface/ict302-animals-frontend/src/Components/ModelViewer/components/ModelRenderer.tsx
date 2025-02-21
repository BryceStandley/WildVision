import React, {useState} from 'react';

import Controls from "./Controls";
import Scene from './Scene';
import Model from './Model';

import {Stack} from '@mui/material';

interface ModelRendererProps {
    modelPath: string | undefined;
}

/**
 * ModelRenderer component.
 *
 * The ModelRenderer component is a React functional component designed to
 * render a 3D model and provide interactive controls for manipulating various model states.
 *
 * Properties:
 * - modelPath: Path to the 3D model file to be rendered.
 *
 * This component manages the following states:
 * - isAnimating: Determines whether the model should be animated.
 * - isWireframe: Determines whether the model is displayed in wireframe mode.
 * - isRotating: Determines whether the model should rotate.
 * - isLooping: Determines whether the model's animation should loop.
 * - animationSpeed: Sets the speed of the animation.
 * - isSkeleton: Determines whether only the skeleton of the model should be displayed.
 * - animationPosition: Controls the current position of the animation.
 * - animationMaxPos: Sets the maximum position of the animation.
 *
 * The component consists of two main parts:
 * - A Scene component which renders the 3D model using various provided states.
 * - A Controls component which provides user controls to manipulate the model's states.
 *
 * @param {ModelRendererProps} props - Properties to configure the model renderer.
 * @returns {JSX.Element} The rendered ModelRenderer component.
 */
const ModelRenderer: React.FC<ModelRendererProps> = ({modelPath}) => {
    // Control states between the model and the viewer controls
    const [isAnimating, setIsAnimating] = useState(true);
    const [isWireframe, setIsWireframe] = useState(false);
    const [isRotating, setIsRotating] = useState(false);
    const [isLooping, setIsLooping] = useState(true);
    const [animationSpeed, setAnimationSpeed] = useState(1.0);
    const [isSkeleton, setIsSkeleton] = useState(false);
    const [animationPosition, setAnimationPosition] = useState(0)
    const [animationMaxPos, setAnimationMaxPos] = useState(0);


    return (
        <Stack>
            <Scene>
                <Model url={modelPath} isAnimating={isAnimating} wireframe={isWireframe}
                       animationSpeed={animationSpeed} isLooping={isLooping} isRotating={isRotating}
                       isSkeleton={isSkeleton} animationPosition={animationPosition}
                       setAnimationPosition={setAnimationPosition} animationMaxPos={animationMaxPos}
                       setAnimationMaxPos={setAnimationMaxPos}/>
            </Scene>
            <Controls
                isAnimating={isAnimating}
                setIsAnimating={setIsAnimating}
                toggleWireframe={isWireframe}
                setToggleWireframe={setIsWireframe}
                isSkeleton={isSkeleton}
                isRotating={isRotating}
                animationSpeed={animationSpeed}
                isLooping={isLooping}
                setAnimationSpeed={setAnimationSpeed}
                setIsLooping={setIsLooping}
                setIsRotating={setIsRotating}
                setIsSkeleton={setIsSkeleton}
                animationPosition={animationPosition}
                animationMaxPos={animationMaxPos}
            />
        </Stack>
    );
}

export default ModelRenderer;