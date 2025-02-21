import React from 'react';
import {GraphicsOptionMenuProps} from "./AnimalInterfaces";
import {Button, Divider, MenuItem} from "@mui/material";
import {Delete, Download, KeyboardArrowDown} from "@mui/icons-material";
import StyledMenu from "./StyledMenu";
import API from "../../Internals/API";

/**
 * GraphicOptionsMenu is a functional React component that renders a button
 * which allows users to access a dropdown menu with various options for a
 * graphic, such as "Download" and "Delete".
 *
 * @param {Object} props - The component props.
 * @param {Object} props.graphic - The graphic data object containing information
 * such as file path and graphics property configuration ID (gpcid).
 */
const GraphicOptionsMenu: React.FC<GraphicsOptionMenuProps> = ({graphic}) => {
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const showGraphicSettings = Boolean(anchorEl);
    const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleMenuClose = () => {
        setAnchorEl(null);
    };


    const handleDownload = () => {
        window.location.href = graphic.filePath;
        handleMenuClose();
    }

    const handleDelete = () => {
        const del = async () => {
            try {
                const response = await fetch(API.Graphic() + "/" + graphic.gpcid, {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });
                if (response.ok) {
                    console.log('Graphic deleted successfully');
                } else {
                    console.error('Failed to delete graphic');
                }
            } catch (error) {
                console.error('Failed to delete graphic:', error);
            }
        }
        del();
        handleMenuClose();
    }

    return (
        <div style={{display: 'inline', paddingLeft: '20px'}}>
            <Button
                aria-controls={showGraphicSettings ? 'graphic details menu' : undefined}
                aria-haspopup="true"
                aria-expanded={showGraphicSettings ? 'true' : undefined}
                variant="contained"
                disableElevation
                onClick={handleMenuOpen}
                endIcon={<KeyboardArrowDown/>}>
                Options
            </Button>
            <StyledMenu
                key={graphic.gpcid}
                id={'graphic-options-menu'}
                MenuListProps={{'aria-labelledby': 'graphics-option-button'}}
                anchorEl={anchorEl}
                open={showGraphicSettings}
                onClose={handleMenuClose}>
                <MenuItem onClick={handleDownload} disableRipple>
                    <Download/>
                    Download
                </MenuItem>
                <Divider sx={{my: 0.5}}/>
                <MenuItem onClick={handleDelete} disableRipple sx={{color: 'red'}}>
                    <Delete/>
                    Delete
                </MenuItem>
            </StyledMenu>
        </div>
    );
}

export default GraphicOptionsMenu;