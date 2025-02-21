import React, {Suspense, useContext, useState} from 'react';
import {
  Alert,
  Box,
  Button,
  Card as MuiCard,
  Checkbox,
  Divider,
  FormControl,
  FormControlLabel,
  FormLabel,
  Snackbar,
  Stack,
  styled,
  TextField,
  Typography
} from '@mui/material';
import {useNavigate} from 'react-router-dom';
import CircularProgressWithLabel from '../Components/User/CircularProgressWithLabel';

import {GoogleIcon} from '../Components/SignUp/CustomIcons';

import {ProjectLogoMin} from '../Components/UI/ProjectLogo';
import {FrontendContext} from "../Internals/ContextStore";

import {getAnalytics} from "firebase/analytics";

import {GoogleAuthProvider, signInWithEmailAndPassword, signInWithPopup} from 'firebase/auth';
import {storeUserInBackend, updateFrontendContext, validateInput} from '../Components/User/userUtils';

// Dynamically import the SignUp component
const SignUp = React.lazy(() => import('../Pages/SignUp'));

/**
 * Styled component that customizes the appearance of MuiCard according to the given theme.
 *
 * This card component is designed to be centered with a column flex direction. It adapts
 * its styling based on the theme's spacing and breakpoints, including specific adaptations
 * for 'dark' mode.
 *
 * @param {object} theme - The theme object containing styling information.
 */
const Card = styled(MuiCard)(({theme}) => ({
    display: 'flex',
    flexDirection: 'column',
    alignSelf: 'center',
    width: '100%',
    padding: theme.spacing(4),
    gap: theme.spacing(2),
    margin: 'auto',
    [theme.breakpoints.up('sm')]: {
        maxWidth: '450px',
    },
    boxShadow:
        'hsla(220, 30%, 5%, 0.05) 0px 5px 15px 0px, hsla(220, 25%, 10%, 0.05) 0px 15px 35px -5px',
    ...theme.applyStyles('dark', {
        boxShadow:
            'hsla(220, 30%, 5%, 0.5) 0px 5px 15px 0px, hsla(220, 25%, 10%, 0.08) 0px 15px 35px -5px',
    }),
}));

/**
 * SignInContainer is a styled component based on the Stack layout component.
 * This container takes up the full height of its parent and has padding
 * applied to its content.
 *
 * It features a default radial gradient background, transitioning from a
 * light blue to white, radiating from the center.
 *
 * When the theme mode is set to 'dark', the radial gradient changes to a
 * dark blue to nearly black gradient, with partial transparency.
 *
 * The component makes use of the theme's `applyStyles` function to apply
 * customized styles based on the current theme mode.
 *
 * @name SignInContainer
 * @type {StyledComponent<StackProps, Theme>}
 */
const SignInContainer = styled(Stack)(({theme}) => ({
    height: '100%',
    padding: 20,
    backgroundImage:
        'radial-gradient(ellipse at 50% 50%, hsl(210, 100%, 97%), hsl(0, 0%, 100%))',
    backgroundRepeat: 'no-repeat',
    ...theme.applyStyles('dark', {
        backgroundImage:
            'radial-gradient(at 50% 50%, hsla(210, 100%, 16%, 0.5), hsl(220, 30%, 5%))',
    }),
}));

/**
 * SignIn React Functional Component.
 *
 * This component handles user authentication via email/password and Google sign-in.
 * It utilizes React hooks to manage state and context to connect with a Firebase backend.
 *
 * Component State:
 * - emailError: Boolean to handle email error state.
 * - emailErrorMessage: String to store the email error message.
 * - passwordError: Boolean to handle password error state.
 * - passwordErrorMessage: String to store the password error message.
 * - progress: Number to manage the progress of the authentication process.
 * - loading: Boolean to handle loading state.
 * - open: Boolean to manage the state of a modal dialog.
 * - notification: Object to handle notification state providing message and severity.
 * - isSignUp: Boolean to toggle between sign-in and sign-up forms.
 *
 * Event Handlers:
 * - handleClickOpen: Opens the modal dialog.
 * - handleClose: Closes the modal dialog.
 * - HandleSubmit: Handles form submission for email/password sign-in.
 * - validateInputs: Validates user email and password inputs.
 * - handleGoogleSignIn: Initiates Google sign-in process.
 * - handleCloseNotification: Closes the notification snackbar.
 *
 * Render:
 * - Displays sign-in or sign-up form based on `isSignUp` state.
 * - Displays progress indicator during ongoing authentication.
 * - Presents a notification snackbar for error messages.
 * - Provides Google sign-in option with a button.
 * - Toggles between sign-in and sign-up forms via a button.
 */
const SignIn: React.FC = () => {
    const frontendContext = useContext(FrontendContext);
    const nav = useNavigate();

    const [emailError, setEmailError] = React.useState(false);
    const [emailErrorMessage, setEmailErrorMessage] = React.useState('');
    const [passwordError, setPasswordError] = React.useState(false);
    const [progress, setProgress] = useState(0);
    const [loading, setLoading] = useState(false);
    const [passwordErrorMessage, setPasswordErrorMessage] = React.useState('');
    const [open, setOpen] = React.useState(false);
    const [notification, setNotification] = useState({open: false, message: '', severity: 'error'});

    // State to toggle between sign-in and sign-up
    const [isSignUp, setIsSignUp] = useState(false);
    const googleProvider = new GoogleAuthProvider();

    /**
     * handleClickOpen is an event handler function that sets the state
     * variable 'open' to true. This is typically used to control the visibility of modal dialogs,
     * pop-ups, or other UI components that require an open/close state.
     */
    const handleClickOpen = () => {
        setOpen(true);
    };

    /**
     * handleClose is a function that sets the state variable 'open' to false.
     * This is typically used to close a modal, dropdown, or any other UI component
     * that is conditionally rendered based on the 'open' state.
     */
    const handleClose = () => {
        setOpen(false);
    };

    /**
     * Handles the submission of a form for user authentication.
     *
     * @param {React.FormEvent<HTMLFormElement>} event - The form event triggered by user submission.
     *
     * This function:
     * - Prevents the default form submission.
     * - Extracts the email and password from the form data.
     * - Validates the email and password against predefined whitelists of allowed characters.
     * - Sets error states and messages when validation fails.
     * - Initiates the authentication process with progress updates.
     * - Navigates to a dashboard or handles errors appropriately based on the response.
     *
     * Dependencies:
     * - validateInput: A function to validate the input against a whitelist.
     * - setEmailError, setEmailErrorMessage, setPasswordError, setPasswordErrorMessage: State setters for error handling.
     * - setLoading, setProgress: Functions to manage loading state and progress indication.
     * - signInWithEmailAndPassword: Firebase authentication function.
     * - storeUserInBackend: Function to store user data in the backend.
     * - updateFrontendContext: Function to update the frontend context with user data.
     * - nav: Function to navigate to a different route.
     * - setNotification: Function to set notification messages.
     */
    const HandleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const email = data.get('email') as string;
        const password = data.get('password') as string;

        // Define whitelist characters (e.g., alphanumeric and basic symbols)
        const emailWhitelist = 'a-zA-Z0-9@._-';
        const passwordWhitelist = 'a-zA-Z0-9!@#$%^&*()_+-=';

        // Validate email and password
        if (!validateInput(email, emailWhitelist)) {
            setEmailError(true);
            setEmailErrorMessage('Invalid characters in email.');
            return;
        }

        if (!validateInput(password, passwordWhitelist)) {
            setPasswordError(true);
            setPasswordErrorMessage('Invalid characters in password.');
            return;
        }

        setLoading(true);
        setProgress(10); // Start progress

        // Continue with authentication logic
        try {
            const userCredential = await signInWithEmailAndPassword(frontendContext.firebaseAuth.current, email, password);
            const user = userCredential.user;

            setProgress(50);

            const idToken = await user.getIdToken();

            await storeUserInBackend(frontendContext, user, idToken).then(
                async () => {
                    setProgress(75);
                    await updateFrontendContext(frontendContext, user).then(() => {
                        setProgress(100);
                    })
                })


            nav('/dashboard/upload');

        } catch (error: any) {
            console.error("Error signing in with email and password:", error);

            setLoading(false);
            setProgress(0);

            // Handle specific Firebase errors
            switch (error.code) {
                case 'auth/user-not-found':
                    setEmailError(true);
                    setEmailErrorMessage('No account found with this email.');
                    setNotification({
                        open: true,
                        message: 'No account found with this email.',
                        severity: 'error',
                    });
                    break;

                case 'auth/wrong-password':
                    setPasswordError(true);
                    setPasswordErrorMessage('Incorrect password.');
                    setNotification({
                        open: true,
                        message: 'Incorrect password.',
                        severity: 'error',
                    });
                    break;

                case 'auth/invalid-email':
                    setEmailError(true);
                    setEmailErrorMessage('Invalid email format.');
                    setNotification({
                        open: true,
                        message: 'Invalid email format.',
                        severity: 'error',
                    });
                    break;

                case 'auth/too-many-requests':
                    setNotification({
                        open: true,
                        message: 'Too many unsuccessful login attempts. Please try again later.',
                        severity: 'error',
                    });
                    break;

                default:
                    setNotification({
                        open: true,
                        message: 'An unexpected error occurred. Please try again later. Development note: Check database connection.',
                        severity: 'error',
                    });
                    console.error('Unhandled error during sign-in:', error);
                    break;
            }
        }
    };

    /**
     * Validates the input fields for email and password.
     *
     * - Checks if the email field contains a valid email address. If not, it sets the email error state and the corresponding error message.
     * - Checks if the password field contains at least 6 characters. If not, it sets the password error state and the corresponding error message.
     *
     * @returns {boolean} True if both the email and the password are valid, false otherwise.
     */
    const validateInputs = () => {
        const email = document.getElementById('email') as HTMLInputElement;
        const password = document.getElementById('password') as HTMLInputElement;

        let isValid = true;

        if (!email.value || !/\S+@\S+\.\S+/.test(email.value)) {
            setEmailError(true);
            setEmailErrorMessage('Please enter a valid email address.');
            isValid = false;
        } else {
            setEmailError(false);
            setEmailErrorMessage('');
        }

        if (!password.value || password.value.length < 6) {
            setPasswordError(true);
            setPasswordErrorMessage('Password must be at least 6 characters long.');
            isValid = false;
        } else {
            setPasswordError(false);
            setPasswordErrorMessage('');
        }

        return isValid;
    };

    /**
     * Handles the Google Sign-In process. This asynchronous function initiates a sign-in
     * flow using a Google Auth Provider and updates various UI elements to indicate progress.
     *
     * The function performs the following steps:
     * - Sets the loading state and initial progress value.
     * - Initiates the sign-in with Google process using a popup dialog.
     * - Retrieves the authenticated user and their ID token.
     * - Stores the user information in the backend.
     * - Updates the frontend context with the authenticated user's information.
     * - Navigates the user to the dashboard's upload section.
     *
     * If any error occurs during the sign-in process, it logs the error to the console
     * and updates the UI to hide the loading indicator.
     *
     * @async
     * @function handleGoogleSignIn
     * @returns {Promise<void>} A promise that resolves when the sign-in process completes.
     */
    const handleGoogleSignIn = async () => {
        const googleProvider = new GoogleAuthProvider();
        setLoading(true);
        setProgress(10);

        try {
            // Sign in with Google
            const result = await signInWithPopup(frontendContext.firebaseAuth.current, googleProvider);
            setProgress(40);
            const user = result.user; // This is the authenticated user

            //console.log('Google sign-in successful:', user);

            // Retrieve the ID token
            const idToken = await user.getIdToken();
            setProgress(60);
            // Store the user in the backend
            await storeUserInBackend(frontendContext, user, idToken);
            setProgress(80);

            updateFrontendContext(frontendContext, user);
            setProgress(100);

            // Navigate to the dashboard
            nav('/dashboard/upload');
        } catch (error) {
            console.error("Error signing in with Google:", error);
            setLoading(false); //Hides loading symbol
        }
    };

    //console.log(process.env);
    const analytics = getAnalytics(frontendContext.firebaseRef.current);
    //console.log(analytics);

    /**
     * Handles the action to close the notification.
     *
     * This function updates the `notification` state by setting the `open` property to `false`.
     * It is intended to be used as a callback for dismissing notifications in the user interface.
     *
     * @returns {void}
     */
    const handleCloseNotification = () => {
        setNotification({...notification, open: false});
    };

    return (
        <SignInContainer direction="column" justifyContent="space-between">
            <Card variant="outlined">
                <ProjectLogoMin/>
                <Typography component="h1" variant="h4">
                    {isSignUp ? 'Sign up' : 'Sign in'}
                </Typography>
                {loading && (
                    <Box sx={{my: 2, display: 'flex', justifyContent: 'center'}}>
                        <CircularProgressWithLabel value={progress}/>
                    </Box>
                )}

                {isSignUp ? (
                    // Render SignUp Form (loaded lazily)
                    <Suspense fallback={<div>Loading...</div>}>
                        <SignUp/>
                    </Suspense>
                ) : (
                    // Render SignIn Form
                    <Box component="form" onSubmit={HandleSubmit} noValidate
                         sx={{display: 'flex', flexDirection: 'column', width: '100%', gap: 2}}>
                        <FormControl>
                            <FormLabel htmlFor="email">Email</FormLabel>
                            <TextField id="email" type="email" name="email" placeholder="your@email.com" required
                                       fullWidth error={emailError} helperText={emailError ? emailErrorMessage : ''}/>
                        </FormControl>
                        <FormControl>
                            <FormLabel htmlFor="password">Password</FormLabel>
                            <TextField id="password" type="password" name="password" placeholder="••••••" required
                                       fullWidth error={passwordError}
                                       helperText={passwordError ? passwordErrorMessage : ''}/>
                        </FormControl>
                        <FormControlLabel control={<Checkbox value="remember" color="primary"/>} label="Remember me"/>
                        <Button type="submit" fullWidth variant="contained">Sign in</Button>
                    </Box>
                )}

                {/* Snackbar for error notifications */}
                <Snackbar
                    open={notification.open}
                    autoHideDuration={6000}
                    onClose={handleCloseNotification}
                    anchorOrigin={{vertical: 'top', horizontal: 'center'}}
                >
                    <Alert onClose={handleCloseNotification} severity={notification.severity as any}
                           sx={{width: '100%'}}>
                        {notification.message}
                    </Alert>
                </Snackbar>

                <Divider sx={{my: 2}}>
                    <Typography sx={{color: 'text.secondary'}}>or</Typography>
                </Divider>

                <Box sx={{display: 'flex', flexDirection: 'column', gap: 2}}>
                    <Button
                        type="submit"
                        fullWidth
                        variant="outlined"
                        onClick={handleGoogleSignIn}
                        startIcon={<GoogleIcon/>}
                    >
                        Sign In with Google
                    </Button>
                </Box>

                <Typography sx={{textAlign: 'center', mt: 2}}>
                    {isSignUp ? (
                        <>
                            Already have an account?{' '}
                            <Button onClick={() => setIsSignUp(false)} sx={{color: 'black'}}>
                                Sign in
                            </Button>
                        </>
                    ) : (
                        <>
                            Don&apos;t have an account?{' '}
                            <Button onClick={() => setIsSignUp(true)} sx={{color: 'black'}}>
                                Sign-up
                            </Button>
                        </>
                    )}
                </Typography>


            </Card>
        </SignInContainer>
    );
}

export default SignIn;