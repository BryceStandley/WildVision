import * as React from 'react';
import dayjs, {Dayjs} from 'dayjs';
import Button from '@mui/material/Button';
import CalendarTodayRoundedIcon from '@mui/icons-material/CalendarTodayRounded';
import {AdapterDayjs} from '@mui/x-date-pickers/AdapterDayjs';
import {UseDateFieldProps} from '@mui/x-date-pickers/DateField';
import {LocalizationProvider} from '@mui/x-date-pickers/LocalizationProvider';
import {DatePicker} from '@mui/x-date-pickers/DatePicker';
import {BaseSingleInputFieldProps, DateValidationError, FieldSection,} from '@mui/x-date-pickers/models';

interface ButtonFieldProps
    extends UseDateFieldProps<Dayjs, false>,
        BaseSingleInputFieldProps<
            Dayjs | null,
            Dayjs,
            FieldSection,
            false,
            DateValidationError
        > {
    setOpen?: React.Dispatch<React.SetStateAction<boolean>>;
}

/**
 * Renders a button component with various properties related to state, styling,
 * and accessibility.
 *
 * @param {Object} props - The properties to configure the ButtonField component.
 * @param {function} props.setOpen - Callback function to toggle the open state when the button is clicked.
 * @param {string} [props.label] - Optional label to display on the button.
 * @param {string} props.id - The unique identifier for the button.
 * @param {boolean} [props.disabled] - Whether the button is disabled.
 * @param {Object} [props.InputProps] - Additional props to pass to the input element inside the button.
 * @param {Object} [props.inputProps] - Additional props to pass to the button element.
 * @param {string} [props.inputProps['aria-label']] - Aria-label for accessibility purposes.
 * @return {JSX.Element} The rendered ButtonField component.
 */
function ButtonField(props: ButtonFieldProps) {
    const {
        setOpen,
        label,
        id,
        disabled,
        InputProps: {ref} = {},
        inputProps: {'aria-label': ariaLabel} = {},
    } = props;

    return (
        <Button
            variant="outlined"
            id={id}
            disabled={disabled}
            ref={ref}
            aria-label={ariaLabel}
            size="small"
            onClick={() => setOpen?.((prev) => !prev)}
            startIcon={<CalendarTodayRoundedIcon fontSize="small"/>}
            sx={{minWidth: 'fit-content'}}
        >
            {label ? `${label}` : 'Pick a date'}
        </Button>
    );
}

/**
 * CustomDatePicker is a React component that renders a date picker using the Material-UI library.
 * It utilizes the Dayjs date library for date manipulation and formatting.
 *
 * @return {JSX.Element} The rendered date picker component.
 */
export default function CustomDatePicker() {
    const [value, setValue] = React.useState<Dayjs | null>(dayjs(new Date()));
    const [open, setOpen] = React.useState(false);

    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DatePicker
                value={value}
                label={value == null ? null : value.format('MMM DD, YYYY')}
                onChange={(newValue) => setValue(newValue)}
                slots={{field: ButtonField}}
                slotProps={{
                    field: {setOpen} as any,
                    nextIconButton: {size: 'small'},
                    previousIconButton: {size: 'small'},
                }}
                open={open}
                onClose={() => setOpen(false)}
                onOpen={() => setOpen(true)}
                views={['day', 'month', 'year']}
            />
        </LocalizationProvider>
    );
}
