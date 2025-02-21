import React from 'react';
import {Box, CircularProgress, Typography} from '@mui/material';

interface CircularProgressWithLabelProps {
    value: number;
}

/**
 * CircularProgressWithLabel is a React functional component that displays a circular progress indicator
 * with a label showing the current progress percentage.
 *
 * @param {Object} props - The properties object.
 * @param {number} props.value - The current value of the progress, as a percentage.
 *
 * @returns {JSX.Element} A component that wraps a CircularProgress component and a Typography component
 * to show the progress percentage in the center of the circular indicator.
 */
const CircularProgressWithLabel: React.FC<CircularProgressWithLabelProps> = ({value}) => {
    return (
        <Box position="relative" display="inline-flex">
            <CircularProgress variant="determinate" value={value}/>
            <Box
                top={0}
                left={0}
                bottom={0}
                right={0}
                position="absolute"
                display="flex"
                alignItems="center"
                justifyContent="center"
            >
                <Typography variant="caption" component="div"
                            color="textSecondary">{`${Math.round(value)}%`}</Typography>
            </Box>
        </Box>
    );
};

export default CircularProgressWithLabel;