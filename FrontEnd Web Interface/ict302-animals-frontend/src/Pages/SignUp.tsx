import React, {useContext, useEffect, useState} from 'react';
import {Box, Button, FormControl, FormLabel, TextField, Typography} from '@mui/material';
import {createUserWithEmailAndPassword, updateProfile} from 'firebase/auth';
import {useNavigate} from 'react-router-dom';
import ReCAPTCHA from 'react-google-recaptcha';
import {FrontendContext} from "../Internals/ContextStore";
import {
  checkPasswordStrength,
  passwordFeedback,
  sanitizeInput,
  storeUserInBackend,
  updateFrontendContext,
  validateInput
} from '../Components/User/userUtils';

/**
 * Represents the properties for the SignUp component.
 * The component expects a callback function for handling the success of the sign-up process.
 *
 * @typedef {Object} SignUpProps
 *
 * @property {Function} [onSignUpSuccess] -
 * Callback function to be called upon successful sign-up.
 */
interface SignUpProps {
    onSignUpSuccess?: () => void;
}

/**
 * The SignUp component provides a user interface for new users to create an account.
 * This component includes fields for email, password, confirm password, username, and
 * a reCAPTCHA verification. It also performs validation and sanitization of user inputs.
 *
 * Props:
 * - onSignUpSuccess: A callback function that is triggered upon successful sign-up.
 *
 * State:
 * - email: The entered email address.
 * - password: The entered password.
 * - confirmPassword: The password confirmation.
 * - username: The chosen username.
 * - recaptchaToken: Token from reCAPTCHA verification.
 * - error: Error message displayed if validation or sign-up fails.
 * - passwordStrength: Indicates the strength of the entered password.
 * - passwordFeedbackMessages: Feedback messages for the entered password.
 *
 * The component uses various functionalities such as:
 * - `handleSignUpSubmit`: Handler for form submission to validate and create a user account.
 * - `handleRecaptchaChange`: Handler for reCAPTCHA token changes.
 *
 * The component performs input sanitization and validation for username, email, and password,
 * and provides real-time feedback on password strength.
 * It utilizes external services for user creation, such as Firebase Authentication and reCAPTCHA.
 * On successful sign-up, it navigates the user to the dashboard.
 */
const SignUp: React.FC<SignUpProps> = ({onSignUpSuccess}) => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [username, setUsername] = useState('');
    const [recaptchaToken, setRecaptchaToken] = useState<string | null>(null);
    const [error, setError] = useState('');
    const [passwordStrength, setPasswordStrength] = useState(''); // Password strength state
    const [passwordFeedbackMessages, setPasswordFeedbackMessages] = useState<string[]>([]); // Password feedback state
    const frontendContext = useContext(FrontendContext);
    const nav = useNavigate();

    useEffect(() => {
        const strength = checkPasswordStrength(password);
        setPasswordStrength(strength);

        const feedback = passwordFeedback(password);
        setPasswordFeedbackMessages(feedback);
    }, [password]);

    /**
     * Asynchronous function to handle sign-up form submission.
     *
     * This function validates the input fields for username, email, and password
     * against specified whitelists. It also ensures that passwords match and
     * the reCAPTCHA is completed. After successful client-side validation,
     * it creates a new user with the provided email and password using Firebase
     * Authentication, updates the user profile, and stores the user details in
     * the backend. On successful sign-up, the user is redirected to the dashboard.
     *
     * @param {React.FormEvent<HTMLFormElement>} event - The form submission event.
     * @returns {Promise<void>} - A Promise that resolves when the sign-up process is complete.
     * @throws Will throw an error if there is an issue with creating the user account.
     */
    const handleSignUpSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        const usernameWhitelist = 'a-zA-Z0-9';
        const emailWhitelist = 'a-zA-Z0-9@._-';
        const passwordWhitelist = 'a-zA-Z0-9!@#$%^&*()_+-=';

        const isUsernameValid = validateInput(username, usernameWhitelist);
        const isEmailValid = validateInput(email, emailWhitelist);
        const isPasswordValid = validateInput(password, passwordWhitelist);

        if (!isUsernameValid) {
            setError('Username contains invalid characters. Please use only letters and numbers.');
            return;
        }

        if (!isEmailValid) {
            setError('Email contains invalid characters.');
            return;
        }

        if (!isPasswordValid) {
            setError('Password contains invalid characters.');
            return;
        }

        if (password !== confirmPassword) {
            setError('Passwords do not match');
            return;
        }

        if (!recaptchaToken) {
            setError('Please complete the reCAPTCHA');
            return;
        }

        const sanitizedUsername = sanitizeInput(username, usernameWhitelist);
        const sanitizedEmail = sanitizeInput(email, emailWhitelist);
        const sanitizedPassword = sanitizeInput(password, passwordWhitelist);

        try {
            const userCredential = await createUserWithEmailAndPassword(frontendContext.firebaseAuth.current, sanitizedEmail, sanitizedPassword);
            const user = userCredential.user;

            await updateProfile(user, {
                displayName: sanitizedUsername,
            });

            const idToken = await user.getIdToken();

            await storeUserInBackend(frontendContext, user, idToken);

            if (onSignUpSuccess) onSignUpSuccess();

            await updateFrontendContext(frontendContext, user);

            nav('/dashboard');
        } catch (error) {
            setError('Error creating account: ' + (error as Error).message);
        }
    };

    /**
     * Handles the change event for the reCAPTCHA widget.
     *
     * This function is triggered when the reCAPTCHA verification token is received or updated.
     * It updates the recaptcha token state with the provided token.
     *
     * @param {string | null} token - The reCAPTCHA token as a string or null if the token is not available.
     */
    const handleRecaptchaChange = (token: string | null) => {
        setRecaptchaToken(token);
    };

    return (
        <Box component="form" onSubmit={handleSignUpSubmit} noValidate
             sx={{display: 'flex', flexDirection: 'column', width: '100%', gap: 2}}>
            <FormControl>
                <FormLabel htmlFor="username">Username</FormLabel>
                <TextField
                    id="username"
                    type="text"
                    name="username"
                    placeholder="Your Username"
                    required
                    fullWidth
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />
            </FormControl>

            <FormControl>
                <FormLabel htmlFor="email">Email</FormLabel>
                <TextField
                    id="email"
                    type="email"
                    name="email"
                    placeholder="your@email.com"
                    required
                    fullWidth
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
            </FormControl>

            <FormControl>
                <FormLabel htmlFor="password">Create Password</FormLabel>
                <TextField
                    id="password"
                    type="password"
                    name="password"
                    placeholder="••••••"
                    required
                    fullWidth
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                {passwordStrength && (
                    <Typography
                        color={passwordStrength === 'Weak' ? 'error' : passwordStrength === 'Medium' ? 'warning' : 'success'}>
                        Password Strength: {passwordStrength}
                    </Typography>
                )}
                {passwordFeedbackMessages.length > 0 && (
                    <ul>
                        {passwordFeedbackMessages.map((message, index) => (
                            <li key={index} style={{color: 'red'}}>{message}</li>
                        ))}
                    </ul>
                )}
            </FormControl>

            <FormControl>
                <FormLabel htmlFor="confirmPassword">Re-enter Password</FormLabel>
                <TextField
                    id="confirmPassword"
                    type="password"
                    name="confirmPassword"
                    placeholder="••••••"
                    required
                    fullWidth
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                />
            </FormControl>

            {error && <Typography color="error">{error}</Typography>}

            <FormControl>
                <ReCAPTCHA
                    sitekey="6LdD8VgqAAAAAFZ2fzniAJ9rmDc_es3C0fp9P9Ma"
                    onChange={handleRecaptchaChange}
                />
            </FormControl>

            <Button type="submit" fullWidth variant="contained">
                Sign up
            </Button>
        </Box>
    );
};

export default SignUp;
