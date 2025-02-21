import * as React from 'react';
import Badge, {badgeClasses} from '@mui/material/Badge';
import IconButton, {IconButtonProps} from '@mui/material/IconButton';

export interface MenuButtonProps extends IconButtonProps {
    showBadge?: boolean;
}

/**
 * MenuButton component renders a button with an optional badge indicator.
 *
 * @param {Object} props - The properties passed to the MenuButton component.
 * @param {boolean} props.showBadge - Determines whether the badge is shown.
 * @param {...Object} props - Additional properties to be passed to the IconButton component.
 *
 * @return {JSX.Element} A Badge component wrapping an IconButton.
 */
export default function MenuButton({
                                       showBadge = false,
                                       ...props
                                   }: MenuButtonProps) {
    return (
        <Badge
            color="error"
            variant="dot"
            invisible={!showBadge}
            sx={{[`& .${badgeClasses.badge}`]: {right: 2, top: 2}}}
        >
            <IconButton size="small" {...props} />
        </Badge>
    );
}
