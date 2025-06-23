import axios from 'axios';

class CurrentUserService {
    constructor() {
        this.apiBaseUrl = 'https://localhost:5000/api'; // Base URL for the API
    }

    /**
     * Fetches the current user information by email, calls the API endpoint,
     * and saves the information in localStorage.
     * @param {string} email - The email address of the user.
     * @returns {Promise<void>}
     */
    async fetchAndSaveCurrentUserInformationToLocalStorage(email) {
        try {
            const response = await axios.get(`${this.apiBaseUrl}/User/GetUserInformationByEmail`, {
                params: { email }
            });


            const userInformation = response.data;
          

            localStorage.setItem('currentUserInformation', JSON.stringify(userInformation));
        } catch (error) {
            console.error('Error fetching and saving current user information to localStorage:', error);
        }
    }

    /**
     * Retrieves the current user information from localStorage.
     * @returns {object|null} The user information object or null if not found.
     */
    getCurrentUserInformationFromLocalStorage() {
        const userInformation = localStorage.getItem('currentUserInformation');
        return userInformation ? JSON.parse(userInformation) : null;
    }

    /**
     * Removes the current user information from localStorage.
     */
    removeCurrentUserInformationFromLocalStorage() {
        localStorage.removeItem('currentUserInformation');
    }

    getCurrentEmployeeId() {
        const userInfo = this.getCurrentUserInformationFromLocalStorage();

        console.log("User info recibido:", userInfo);
        if (userInfo && userInfo.idNaturalPerson) {
            return userInfo.idNaturalPerson;
        } else {
            throw new Error('Employee ID not found in user information.');
        }
    }
}

// Export a pre-instantiated object of the service
export default new CurrentUserService();