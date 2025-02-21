import React, {useContext} from 'react';

import {FrontendContext} from "../Internals/ContextStore";

import QueueCard from '../Components/Queue/QueueCard';
import {alpha, Box, Grid2 as Grid} from '@mui/material';

/**
 * Functional React component that displays the current items in the queue and their respective details.
 *
 * This component consumes the `FrontendContext`, which is expected to provide information
 * about the user's context, including the current items in the queue and their processing state.
 *
 * The component renders the number of items in the queue and, if there are items, creates a grid
 * layout displaying `QueueCard` components for each item with its respective details such as name
 * and progress.
 *
 * @component
 */
const Queue: React.FC = () => {
    const frontendContext = useContext(FrontendContext);

    return (
        <Box sx={{display: 'flex'}}>
            <Box
                component="main"
                sx={(theme) => ({
                    flexGrow: 1,
                    backgroundColor: alpha(theme.palette.background.default, 1),
                    overflow: 'auto',
                })}
            >
                Items in Queue : {frontendContext.user.contextRef?.current.currentItemsInQueue}
                <Grid container
                      spacing={2}
                      sx={{
                          alignItems: 'center',
                          mx: 3,
                          pb: 10,
                          mt: {xs: 8, md: 0},
                      }}
                >

                    {frontendContext.user.contextRef?.current.currentItemsInQueue > 0 && frontendContext.user.contextRef?.current.currentItemsInProcessQueue.map((item, index) => (
                        <Grid>
                            <QueueCard key={index} title={item.name} position={index} progress={item.progress}/>
                        </Grid>
                    ))}

                </Grid>
            </Box>
        </Box>
    );
};

export default Queue;
