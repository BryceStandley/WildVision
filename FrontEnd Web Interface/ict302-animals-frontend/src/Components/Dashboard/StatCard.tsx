import * as React from 'react';
import {useTheme} from '@mui/material/styles';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Chip from '@mui/material/Chip';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';
import {SparkLineChart} from '@mui/x-charts/SparkLineChart';
import {areaElementClasses} from '@mui/x-charts/LineChart';

/**
 * Represents the properties for a statistical card.
 *
 * @typedef {Object} StatCardProps
 * @property {string} title - The title of the stat card.
 * @property {string} value - The current value displayed on the stat card.
 * @property {string} interval - The time interval for which the value is applicable.
 * @property {'up'|'down'|'neutral'} trend - Indicates the trend direction of the value.
 * @property {number[]} data - An array of numerical data points related to the stat card.
 */
export type StatCardProps = {
    title: string;
    value: string;
    interval: string;
    trend: 'up' | 'down' | 'neutral';
    data: number[];
};

/**
 * Returns an array of strings representing the days in a given month and year.
 *
 * @param {number} month - The month for which the days are needed (1-12).
 * @param {number} year - The year for which the days are needed.
 * @return {string[]} An array of strings where each string represents a day in the format 'MMM D'.
 */
function getDaysInMonth(month: number, year: number) {
    const date = new Date(year, month, 0);
    const monthName = date.toLocaleDateString('en-US', {
        month: 'short',
    });
    const daysInMonth = date.getDate();
    const days = [];
    let i = 1;
    while (days.length < daysInMonth) {
        days.push(`${monthName} ${i}`);
        i += 1;
    }
    return days;
}

/**
 * Creates a linear gradient definition for SVG areas.
 *
 * @param {Object} props - The properties for creating the gradient.
 * @param {string} props.color - The color to be used in the gradient.
 * @param {string} props.id - The unique identifier for the gradient.
 *
 * @return {JSX.Element} The JSX element representing the linear gradient.
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
 * Component for displaying a static card with statistical information.
 *
 * @param {Object} props - The properties object.
 * @param {string} props.title - The title of the statistic.
 * @param {number|string} props.value - The value of the statistic.
 * @param {string} props.interval - The time interval over which the statistic is measured.
 * @param {'up'|'down'|'neutral'} props.trend - The trend indicator ('up', 'down', 'neutral').
 * @param {Array<Object>} props.data - The data points for the chart.
 * @return {JSX.Element} The rendered component.
 */
export default function StatCard({
                                     title,
                                     value,
                                     interval,
                                     trend,
                                     data,
                                 }: StatCardProps) {
    const theme = useTheme();
    const daysInWeek = getDaysInMonth(4, 2024);

    const trendColors = {
        up:
            theme.palette.mode === 'light'
                ? theme.palette.success.main
                : theme.palette.success.dark,
        down:
            theme.palette.mode === 'light'
                ? theme.palette.error.main
                : theme.palette.error.dark,
        neutral:
            theme.palette.mode === 'light'
                ? theme.palette.grey[400]
                : theme.palette.grey[700],
    };

    const labelColors = {
        up: 'success' as const,
        down: 'error' as const,
        neutral: 'default' as const,
    };

    const color = labelColors[trend];
    const chartColor = trendColors[trend];
    const trendValues = {up: '+25%', down: '-25%', neutral: '+5%'};

    return (
        <Card variant="outlined" sx={{height: '100%', flexGrow: 1}}>
            <CardContent>
                <Typography component="h2" variant="subtitle2" gutterBottom>
                    {title}
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
                            <Typography variant="h4" component="p">
                                {value}
                            </Typography>
                            <Chip size="small" color={color} label={trendValues[trend]}/>
                        </Stack>
                        <Typography variant="caption" sx={{color: 'text.secondary'}}>
                            {interval}
                        </Typography>
                    </Stack>
                    <Box sx={{width: '100%', height: 50}}>
                        <SparkLineChart
                            colors={[chartColor]}
                            data={data}
                            area
                            showHighlight
                            showTooltip
                            xAxis={{
                                scaleType: 'band',
                                data: daysInWeek, // Use the correct property 'data' for xAxis
                            }}
                            sx={{
                                [`& .${areaElementClasses.root}`]: {
                                    fill: `url(#area-gradient-${value})`,
                                },
                            }}
                        >
                            <AreaGradient color={chartColor} id={`area-gradient-${value}`}/>
                        </SparkLineChart>
                    </Box>
                </Stack>
            </CardContent>
        </Card>
    );
}
