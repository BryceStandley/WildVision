import React, {useContext} from 'react';

import {FrontendContext} from "../../Internals/ContextStore";

interface DashboardPageDisplayProps {
    children?: React.ReactNode;
    pageDisplaying?: string;
}

/**
 * DashboardPageDisplay is a React functional component that renders its children
 * and leverages the FrontendContext.
 *
 * @param {DashboardPageDisplayProps} props - The properties object for the component.
 * @param {React.ReactNode} props.children - The child elements to be rendered inside this component.
 * @param {string} props.pageDisplaying - The current page being displayed.
 *
 * @constant {React.FC<DashboardPageDisplayProps>} DashboardPageDisplay - The DashboardPageDisplay component.
 */
const DashboardPageDisplay: React.FC<DashboardPageDisplayProps> = ({children, pageDisplaying}) => {
    const frontendContext = useContext(FrontendContext);
    return (
        <div>
            {children}
        </div>
    );
}

export default DashboardPageDisplay;