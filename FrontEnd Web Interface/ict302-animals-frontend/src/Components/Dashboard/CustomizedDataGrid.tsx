import * as React from 'react';
import {DataGrid} from '@mui/x-data-grid';
import {columns, rows} from '../../Internals/data/gridData';

/**
 * Renders a customized DataGrid component with predefined settings for rows, columns, and appearance.
 * The DataGrid features auto height, checkbox selection, and a custom row class name based on row index.
 * The grid's initial state includes pagination with a default page size of 20.
 * Page size options include 10, 20, and 50. Column resizing is disabled, and the grid uses a compact density.
 * Additionally, custom properties for the filter panel are defined, including settings for input components.
 *
 * @return {JSX.Element} A DataGrid component with customized settings.
 */
export default function CustomizedDataGrid() {
    return (
        <DataGrid
            autoHeight
            checkboxSelection
            rows={rows}
            columns={columns}
            getRowClassName={(params) =>
                params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd'
            }
            initialState={{
                pagination: {paginationModel: {pageSize: 20}},
            }}
            pageSizeOptions={[10, 20, 50]}
            disableColumnResize
            density="compact"
            slotProps={{
                filterPanel: {
                    filterFormProps: {
                        logicOperatorInputProps: {
                            variant: 'outlined',
                            size: 'small',
                        },
                        columnInputProps: {
                            variant: 'outlined',
                            size: 'small',
                            sx: {mt: 'auto'},
                        },
                        operatorInputProps: {
                            variant: 'outlined',
                            size: 'small',
                            sx: {mt: 'auto'},
                        },
                        valueInputProps: {
                            InputComponentProps: {
                                variant: 'outlined',
                                size: 'small',
                            },
                        },
                    },
                },
            }}
        />
    );
}
