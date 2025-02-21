import * as React from 'react';
import {useTheme} from '@mui/material/styles';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Chip from '@mui/material/Chip';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';

import ModelViewer from '../../Components/ModelViewer/ModelViewer';

/**
 * Represents the properties of a completed card.
 *
 * Additionally, this interface encapsulates the necessary information required to describe a completed card, including:
 * - title: The title of the completed card.
 * - status: Current status of the completed card.
 * - modelPath: Path to the model associated with the completed card.
 */
export type CompletedCardProps = {
    title: string;
    status: string;
    modelPath: string;
};

/**
 * The `CompletedCard` component displays a card with information about a completed item.
 *
 * @param {CompletedCardProps} props - The properties for the component.
 * @param {string} props.title - The title of the item.
 * @param {string} props.status - The status of the item, either "Complete" or "In Progress".
 * @param {string} props.modelPath - The path to the 3D model to be displayed in the card.
 *
 * @returns {JSX.Element} A React component that renders a card with the item's title, status, and a 3D model viewer.
 */
const CompletedCard: React.FC<CompletedCardProps> = ({title, status, modelPath}) => {
    const theme = useTheme();

    return (
        <Card variant="outlined" sx={{height: '100%', minWidth: 450, flexGrow: 1}}>
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
                                Status: {status === "Complete" ? <Chip label="Complete" color="success"/> :
                                <Chip label="In Progress" color="warning"/>}
                            </Typography>
                        </Stack>
                        <Stack direction="row" sx={{justifyContent: 'center'}}>

                            <ModelViewer modelPath={modelPath}/>
                        </Stack>
                    </Stack>
                </Stack>
            </CardContent>
        </Card>
    );
}

export default CompletedCard;