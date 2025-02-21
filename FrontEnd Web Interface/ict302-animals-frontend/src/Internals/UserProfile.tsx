import QueueItem from './data/QueueItem';
import CompletedItem from './data/CompletedItem';
import {Animal} from '../Components/Animal/AnimalInterfaces';

/**
 * Represents a user's profile with various attributes and associated methods.
 */
export default class UserProfile {
    userId: string
    username: string
    email: string
    initials: string
    loggedInState: boolean = false;
    currentItemsInQueue: number = 0;
    currentItemsInProcessQueue: QueueItem[] = [];
    currentCompletedItems: number = 0;
    completedItems: CompletedItem[] = [];
    userAnimals: Animal[] = [];

    /**
     * Constructor for creating a new User instance.
     *
     * @param {string} [username='No Username'] - The username of the user.
     * @param {string} [email='No Email'] - The email address of the user.
     * @param {string} [initials='NN'] - The initials of the user.
     * @param {boolean} [loggedInState=false] - The login state of the user.
     * @return {User} A new User instance.
     */
    constructor(username: string = 'No Username', email: string = 'No Email', initials: string = 'NN', loggedInState: boolean = false) {
        this.username = username;
        this.email = email;
        this.initials = initials;
        this.loggedInState = loggedInState;
    }

    setLogedInState(loggedInState: boolean) {
        this.loggedInState = loggedInState;
    }
};