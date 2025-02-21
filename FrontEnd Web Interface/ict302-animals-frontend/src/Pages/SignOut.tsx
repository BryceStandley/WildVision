import React, {useContext, useEffect} from 'react';

import {FrontendContext} from "../Internals/ContextStore";
import {useNavigate} from 'react-router-dom';

import UserProfile from '../Internals/UserProfile';

/**
 * A React functional component that handles the user sign-out process.
 *
 * This component utilizes the `FrontendContext` to reset the user's profile
 * and validity status upon sign-out. It navigates the user to the home
 * page (`'/'`) after signing out. The actual logic to communicate the
 * sign-out action to the server is yet to be implemented.
 *
 * Dependencies:
 * - `useContext` from React to access the `FrontendContext`.
 * - `useNavigate` from React Router for navigation.
 *
 * Effects:
 * - Resets the user's profile in the `FrontendContext`.
 * - Sets the user's validity status to `false` in the `FrontendContext`.
 * - Navigates to the home page (`'/'`) after signing out.
 */
const SignOut: React.FC = () => {
    const frontendContext = useContext(FrontendContext);

    const nav = useNavigate();

    // TODO: Add some sort of logic to tell the server to sign out the user

    useEffect(() => {
        frontendContext.user.contextRef.current = new UserProfile();
        frontendContext.user.valid = false;
        nav('/');
    }, [frontendContext.user, nav]);

    return (
        <>
        </>
    );
}

export default SignOut;