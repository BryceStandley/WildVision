import * as React from 'react';
import {useTheme} from '@mui/material/styles';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';

export type QueueCardProps = {
    title: string;
    position: number;
    progress: number;
};


/**
 * Renders a linear gradient for area charts with a specified color and id.
 *
 * @param {Object} props - The properties object.
 * @param {string} props.color - The color to be used for the gradient.
 * @param {string} props.id - The unique identifier for the gradient.
 * @return {JSX.Element} The JSX representation of the linear gradient definition.
 */
function AreaGradient({color, id}: { color: string; id: string }) {
    return (
        <defs>
            <linearGradient id={id} x1="50%" y1="0%" x2="50%" y2="100%">
                <stop offset="0%" stopColor={color} stopOpacity={0.3}/>
                <stop offset="100%" stopColor={color} stopOpacity={0}/>
            </linearGradient>
        </defs>
    );
}

/**
 * QueueCard is a functional React component that displays information about a specific queue item.
 * It renders a card containing the title, position within the queue, and the progress of the generation.
 *
 * @param {QueueCardProps} props - The properties object that stores key information to be displayed within the card.
 * @param {string} props.title - The title of the queue item.
 * @param {number} props.position - The position of the queue item within the queue.
 * @param {number} props.progress - The progress percentage of the queue item generation.
 *
 * @returns {React.FC<QueueCardProps>} A React functional component that renders the queue card.
 */
const QueueCard: React.FC<QueueCardProps> = ({title, position, progress}) => {
    const theme = useTheme();

    return (
        <Card variant="outlined" sx={{height: '100%', flexGrow: 1}}>
            <CardContent>
                <Typography component="h2" variant="subtitle2" gutterBottom>
                    Item: {title}
                </Typography>
                <Stack
                    direction="column"
                    sx={{justifyContent: 'space-between', flexGrow: '1', gap: 1}}
                >
                    <Stack sx={{justifyContent: 'space-between'}}>
                        <Stack
                            direction="row"
                            sx={{justifyContent: 'space-between', alignItems: 'center'}}
                        >
                            <Typography variant="h6" component="p">
                                QueuePosition: {position}
                            </Typography>
                        </Stack>
                        <Typography variant="caption" sx={{color: 'text.secondary'}}>
                            Generation Progress: {progress}
                        </Typography>
                    </Stack>
                </Stack>
            </CardContent>
        </Card>
    );
}

export default QueueCard;