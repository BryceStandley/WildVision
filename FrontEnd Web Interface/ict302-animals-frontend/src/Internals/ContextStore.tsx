import React, {createContext, ReactNode, useRef, useState} from 'react';
import UserProfile from './UserProfile';
import {FirebaseApp, initializeApp} from 'firebase/app';
import {Auth, getAuth} from 'firebase/auth';


/**
 * Represents the context of the application, encapsulating information and references related to the current user and Firebase configuration.
 *
 * @typedef {Object} Context
 * @property {User} user - The current user object containing user information.
 * @property {React.MutableRefObject<FirebaseApp>} firebaseRef - A mutable reference to the Firebase application instance.
 * @property {React.MutableRefObject<Auth>} firebaseAuth - A mutable reference to the Firebase authentication instance.
 */
export type Context = {
    user: User;
    firebaseRef: React.MutableRefObject<FirebaseApp>;
    firebaseAuth: React.MutableRefObject<Auth>;
}

/**
 * Represents a user object with validation status and a reference to user profile context.
 *
 * @typedef {Object} User
 * @property {boolean} valid - Indicates if the user object is valid or not.
 * @property {React.MutableRefObject<UserProfile>} contextRef - A mutable reference to the UserProfile context associated with the user.
 */
export type User = {
    valid: boolean;
    contextRef: React.MutableRefObject<UserProfile>;
}

/**
 * FrontendContext
 *
 * A React context created using createContext for managing frontend-specific state.
 * This context is instantiated with a default empty object cast to the Context type.
 *
 * Typical use cases include providing a shared state or configuration accessible across
 * different components in a React application, specifically tailored for frontend
 * operations or user interface-related data.
 *
 * Initialization:
 *   The FrontendContext is initialized with an empty object, complying with the
 *   Context type defined elsewhere in the application. Any component subscribing
 *   to this context should expect properties as defined in the Context type.
 */
export const FrontendContext = createContext<Context>({} as Context);

/**
 * Interface representing the properties for a ContextStore component.
 *
 * @interface ContextStoreProps
 *
 * @property {ReactNode} children - The child elements or components that this ContextStore will render.
 */
interface ContextStoreProps {
    children: ReactNode;
}

/**
 * ContextStore is a functional component that initializes and provides the
 * application context to its children components. The context includes
 * user information and Firebase configuration and instances.
 *
 * @param {Object} props - The properties object.
 * @param {React.ReactNode} props.children - The child components that will
 * receive the context values.
 *
 * @returns {JSX.Element} The provider component that wraps the children with
 * the application context.
 */
const ContextStore: React.FC<ContextStoreProps> = ({children}) => {
    const userRef = useRef<UserProfile>(new UserProfile());
    const firebaseConfig = {
        apiKey: process.env.REACT_APP_FIREBASE_PROJECT_API_KEY,
        authDomain: process.env.REACT_APP_FIREBASE_AUTH_DOMAIN,
        projectId: process.env.REACT_APP_FIREBASE_PROJECT_ID,
        storageBucket: process.env.REACT_APP_FIREBASE_STORAGE_BUCKET,
        messagingSenderId: process.env.REACT_APP_FIREBASE_MESSAGING_SENDER_ID,
        appId: process.env.REACT_APP_FIREBASE_APP_ID,
        measurementId: process.env.REACT_APP_FIREBASE_MEASUREMENT_ID
    };
    const firebaseRef = useRef<FirebaseApp>(initializeApp(firebaseConfig));
    const firebaseAuth = useRef<Auth>(getAuth(firebaseRef.current));

    const [context, setContext] = useState<Context>({
        user: {valid: false, contextRef: userRef} as User,
        firebaseRef: firebaseRef,
        firebaseAuth: firebaseAuth
    });


    return (
        <FrontendContext.Provider value={context}>
            {children}
        </FrontendContext.Provider>
    )
}

export default ContextStore;