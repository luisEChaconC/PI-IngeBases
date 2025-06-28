import userService from '@/services/userService';

class CurrentUserService {
    async fetchAndSaveCurrentUserInformationToLocalStorage(email) {
        try {
            const userInformation = await userService.getUserInformationByEmail(email);
            console.log("User info recibido:", userInformation);

            localStorage.setItem('currentUserInformation', JSON.stringify(userInformation));
        } catch (error) {
            console.error('Error fetching and saving current user information to localStorage:', error);
        }
    }

    getCurrentUserInformationFromLocalStorage() {
        const userInformation = localStorage.getItem('currentUserInformation');
        return userInformation ? JSON.parse(userInformation) : null;
    }

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

export default new CurrentUserService();
