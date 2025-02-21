import React, {useEffect} from "react";

import {Box, Button, ButtonGroup, Slider, Stack, Tooltip} from '@mui/material';


import {MinusCircle, Pause, Play, PlusCircle, Repeat, RotateCw, Settings} from 'react-feather';

interface ControlsProps {
    isAnimating: boolean;
    setIsAnimating?: any;
    toggleWireframe: boolean;
    setToggleWireframe?: any;
    isRotating: boolean;
    setIsRotating: any;
    isLooping: boolean;
    setIsLooping: any;
    animationSpeed: number;
    setAnimationSpeed: any;
    isSkeleton: boolean;
    setIsSkeleton: any;
    animationPosition: number;
    animationMaxPos: number;

}

/**
 * Controls component for managing various states and settings related to animation
 * and 3D model viewing.
 *
 * @param {Object} props - The properties object.
 * @param {boolean} props.isAnimating - Indicates if the animation is currently running.
 * @param {Function} props.setIsAnimating - Function to set the animation state.
 * @param {boolean} props.toggleWireframe - Flag to toggle wireframe visibility.
 * @param {Function} props.setToggleWireframe - Function to set the wireframe visibility state.
 * @param {number} props.animationSpeed - Current speed of the animation.
 * @param {Function} props.setAnimationSpeed - Function to set the animation speed.
 * @param {boolean} props.isLooping - Indicates if the animation is looping.
 * @param {Function} props.setIsLooping - Function to set the looping state of the animation.
 * @param {boolean} props.isSkeleton - Flag to toggle skeleton visibility.
 * @param {Function} props.setIsSkeleton - Function to set the skeleton visibility state.
 * @param {boolean} props.isRotating - Indicates if the 3D model is rotating.
 * @param {Function} props.setIsRotating - Function to set the rotating state of the 3D model.
 * @param {number} props.animationPosition - Current position of the animation.
 * @param {number} props.animationMaxPos - Maximum position of the animation.
 */
const Controls: React.FC<ControlsProps> = ({
                                               isAnimating,
                                               setIsAnimating,
                                               toggleWireframe,
                                               setToggleWireframe,
                                               animationSpeed,
                                               setAnimationSpeed,
                                               isLooping,
                                               setIsLooping,
                                               isSkeleton,
                                               setIsSkeleton,
                                               setIsRotating,
                                               isRotating,
                                               animationPosition,
                                               animationMaxPos
                                           }) => {

    function increaseAnimSpeed() {
        // Clamp the animation speed to a maximum of 2
        if (animationSpeed + 0.1 > 2) {
            setAnimationSpeed(2);
        } else {
            setAnimationSpeed(animationSpeed + 0.1);
        }

    }

    function decreaseAnimSpeed() {
        if (animationSpeed - 0.1 < 0.1) {
            setAnimationSpeed(0.1);
        } else {
            setAnimationSpeed(animationSpeed - 0.1);
        }
    }

    function toggleRotation() {
        setIsRotating(!isRotating);
    }

    function toggleLoop() {
        setIsLooping(!isLooping);
    }

    function toggleSkelleton() {
        setIsSkeleton(!isSkeleton);
    }

    function toggleWireframeVisible() {
        setToggleWireframe(!toggleWireframe);
    }

    function toggleAnimation() {
        setIsAnimating(!isAnimating);
    }

    function updateAnimationPosition(event: any, value: number | number[]) {
        //setAnimationPosition({currentPos: value as number, maxPos: animationPosition.maxPos});
    }

    useEffect(() => {
        //setLocalAnimationPosition(animationPosition.current.currentPos);
        //console.log(animationPosition);
    }, [animationPosition]);

    return (
        <Box sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
        }}>
            <Stack spacing={2} direction="column" sx={{alignItems: 'center', mb: 1}}>
                <Slider defaultValue={0} value={animationPosition} min={0} max={animationMaxPos} disabled
                        aria-label="Animition Position"/>
                <ButtonGroup variant="contained" aria-label="3D Model View Controls">
                    {isAnimating &&
                        <Button onClick={toggleAnimation}><Tooltip title={'Pause'} arrow><Pause/></Tooltip></Button>}
                    {!isAnimating &&
                        <Button onClick={toggleAnimation}><Tooltip title={'Play'} arrow><Play/></Tooltip></Button>}
                    <Button onClick={increaseAnimSpeed}><Tooltip title={'Speed Up Animation'}
                                                                 arrow><PlusCircle/></Tooltip></Button>
                    <Button onClick={decreaseAnimSpeed}><Tooltip title={'Slow Down Animation'}
                                                                 arrow><MinusCircle/></Tooltip></Button>
                    <Button onClick={toggleLoop}><Tooltip title={'Toggle Animation Loop'}
                                                          arrow><Repeat/></Tooltip></Button>
                    <Button onClick={toggleRotation}><Tooltip title={'Toggle Rotation'}
                                                              arrow><RotateCw/></Tooltip></Button>
                    {toggleWireframe ?
                        <Button variant="contained" color='secondary' onClick={toggleWireframeVisible}><Tooltip
                            title={'Toggle Wireframe'} arrow><Settings/></Tooltip></Button> :
                        <Button variant="outlined" color='secondary' onClick={toggleWireframeVisible}><Tooltip
                            title={'Toggle Wireframe'} arrow><Settings/></Tooltip></Button>}
                </ButtonGroup>
            </Stack>
        </Box>
    );
}

export default Controls;