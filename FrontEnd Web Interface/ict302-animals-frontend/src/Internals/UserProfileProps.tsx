import {createContext} from 'react';
import UserProfile from './UserProfile';

/**
 * UserProfileContext provides a context for managing and accessing
 * the state of the user profile throughout the component tree.
 *
 * Used for sharing the user profile information and updates
 * across different components without the need to pass props manually
 * at every level.
 *
 * Typically utilized within a context provider component that wraps
 * parts of the application where user profile information is required.
 *
 * Initialized with a new instance of UserProfile when first created.
 *
 * @type {React.Context<UserProfile>}
 */
export const UserProfileContext = createContext(new UserProfile());