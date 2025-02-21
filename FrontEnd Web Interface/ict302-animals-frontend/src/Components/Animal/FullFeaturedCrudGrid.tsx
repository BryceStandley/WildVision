import * as React from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/DeleteOutlined';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Close';
import {
  DataGrid,
  GridActionsCellItem,
  GridColDef,
  GridEventListener,
  GridRowEditStopReasons,
  GridRowId,
  GridRowModel,
  GridRowModes,
  GridRowModesModel,
  GridRowsProp,
  GridSlots,
  GridToolbarContainer,
} from '@mui/x-data-grid';

const ModelType = ['Mesh', 'Nurbs', 'Volumetric', 'Rigged', 'low-polygons'];


const initialRows: GridRowsProp = [
    {id: 1, DateGen: new Date(), ModelType: 'Mesh', Note: 'note'},
    {id: 2, DateGen: new Date(), ModelType: 'Volumetric', Note: 'note'},
    {id: 3, DateGen: new Date(), ModelType: 'low-polygons', Note: 'note'},
    //{ id: 1, name: 'Alice', age: 25, joinDate: new Date(), role: 'Market' },
    //{ id: 2, name: 'Bob', age: 36, joinDate: new Date(), role: 'Finance' },
    //{ id: 3, name: 'Charlie', age: 19, joinDate: new Date(), role: 'Development' },
];

/**
 * Interface representing the properties required for the EditToolbar component.
 *
 * @property {Function} setRows - Function to update the rows state, taking a callback that receives the old rows and returns the new rows.
 * @property {Function} setRowModesModel - Function to update the row modes model, taking a callback that receives the old model and returns the new model.
 * @property {GridRowsProp} rows - Current rows data used within the grid.
 */
interface EditToolbarProps {
    setRows: (newRows: (oldRows: GridRowsProp) => GridRowsProp) => void;
    setRowModesModel: (
        newModel: (oldModel: GridRowModesModel) => GridRowModesModel,
    ) => void;
    rows: GridRowsProp;
}

interface CrudGridProps {
    triggerRefresh: boolean;
}

interface AnimalData {
    animalID: string;
    modelType: string;
    videoGenDate: string;

}

/**
 * A custom toolbar component for adding new rows to a data grid.
 *
 * @param {Object} props - The properties passed to the EditToolbar component.
 * @param {Function} props.setRows - Function to update the rows state.
 * @param {Function} props.setRowModesModel - Function to update the row modes model state.
 * @param {Array} [props.rows=[]] - Array of existing rows in the data grid.
 * @return {JSX.Element} The toolbar component containing a button to add new rows.
 */
function EditToolbar(props: EditToolbarProps) {
    const {setRows, setRowModesModel, rows = []} = props;

    const handleClick = () => {
        const id = rows.length + 1;
        setRows((oldRows) => [
            ...oldRows,
            {id, DateGen: '', ModelType: '', Note: '', isNew: true},
        ]);
        setRowModesModel((oldModel) => ({
            ...oldModel,
            [id]: {mode: GridRowModes.Edit, fieldToFocus: 'DateGen'},
        }));
    };

    return (
        <GridToolbarContainer>
            <Button color="primary" startIcon={<AddIcon/>} onClick={handleClick}>
                Add record
            </Button>
        </GridToolbarContainer>
    );
}

/**
 * FullFeaturedCrudGrid is a React component that provides a fully featured CRUD (Create, Read, Update, Delete) data grid with row editing capabilities.
 * It utilizes the DataGrid component to present and manage a grid of data, allowing users to perform actions such as editing, saving, and deleting rows.
 *
 * @return {JSX.Element} A JSX Element representing the data grid containing rows of data with CRUD actions and editing functionality.
 */
export default function FullFeaturedCrudGrid() {
    const [rows, setRows] = React.useState(initialRows);
    const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>({});

    const handleRowEditStop: GridEventListener<'rowEditStop'> = (params, event) => {
        if (params.reason === GridRowEditStopReasons.rowFocusOut) {
            event.defaultMuiPrevented = true;
        }
    };

    const handleEditClick = (id: GridRowId) => () => {
        setRowModesModel({...rowModesModel, [id]: {mode: GridRowModes.Edit}});
    };

    const handleSaveClick = (id: GridRowId) => () => {
        setRowModesModel({...rowModesModel, [id]: {mode: GridRowModes.View}});
    };

    const handleDeleteClick = (id: GridRowId) => () => {
        setRows(rows.filter((row) => row.id !== id));
    };

    const handleCancelClick = (id: GridRowId) => () => {
        setRowModesModel({
            ...rowModesModel,
            [id]: {mode: GridRowModes.View, ignoreModifications: true},
        });

        const editedRow = rows.find((row) => row.id === id);
        if (editedRow!.isNew) {
            setRows(rows.filter((row) => row.id !== id));
        }
    };

    const processRowUpdate = (newRow: GridRowModel) => {
        const updatedRow = {...newRow, isNew: false};
        setRows(rows.map((row) => (row.id === newRow.id ? updatedRow : row)));
        return updatedRow;
    };

    const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
        setRowModesModel(newRowModesModel);
    };

    const columns: GridColDef[] = [
        {
            field: 'DateGen',
            headerName: 'Generated Date',
            type: 'date',
            width: 150,
            editable: false,
        },
        {
            field: 'ModelType',
            headerName: 'Model Type',
            width: 130,
            editable: false,
            valueOptions: ['Mesh', 'Nurbs', 'Volumetric', 'Rigged', 'low-polygons'],
        },
        {
            field: 'Note',
            headerName: 'Note',
            width: 160,
            editable: true
        },
        {
            field: 'actions',
            type: 'actions',
            headerName: 'Actions',
            width: 100,
            cellClassName: 'actions',
            getActions: ({id}) => {
                const isInEditMode = rowModesModel[id]?.mode === GridRowModes.Edit;

                if (isInEditMode) {
                    return [
                        <GridActionsCellItem
                            icon={<SaveIcon/>}
                            label="Save"
                            sx={{
                                color: 'primary.main',
                            }}
                            onClick={handleSaveClick(id)}
                        />,
                        <GridActionsCellItem
                            icon={<CancelIcon/>}
                            label="Cancel"
                            className="textPrimary"
                            onClick={handleCancelClick(id)}
                            color="inherit"
                        />,
                    ];
                }

                return [
                    <GridActionsCellItem
                        icon={<EditIcon/>}
                        label="Edit"
                        className="textPrimary"
                        onClick={handleEditClick(id)}
                        color="inherit"
                    />,
                    <GridActionsCellItem
                        icon={<DeleteIcon/>}
                        label="Delete"
                        onClick={handleDeleteClick(id)}
                        color="inherit"
                    />,
                ];
            },
        },
    ];

    return (
        <Box
            sx={{
                height: 300,
                width: '100%',
                '& .actions': {
                    color: 'text.secondary',
                },
                '& .textPrimary': {
                    color: 'text.primary',
                },
            }}
        >
            <DataGrid
                rows={rows}
                columns={columns}
                editMode="row"
                rowModesModel={rowModesModel}
                onRowModesModelChange={handleRowModesModelChange}
                onRowEditStop={handleRowEditStop}
                processRowUpdate={processRowUpdate}
                slots={{
                    toolbar: EditToolbar as GridSlots['toolbar'],
                }}
                slotProps={{
                    toolbar: {setRows, setRowModesModel},
                }}
            />
        </Box>
    );
}