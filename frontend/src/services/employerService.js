import axios from 'axios';
import API_CONFIG from '@/config/api';
import currentUserService from '@/services/currentUserService';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class EmployerService {
    async createEmployerWithDependencies(employerData) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.EMPLOYER.CREATE_WITH_DEPENDENCIES;
            const formattedData = this.formatEmployerDataForBackend(employerData);

            const response = await axios.post(buildApiUrl(endpoint), formattedData);
            return response.data;
        } catch (error) {
            console.error('Error creating employer:', error);
            throw handleApiError(error, 'Error al crear el empleador');
        }
    }

    async getEmployerById(employerId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.EMPLOYER.GET_BY_ID(employerId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching employer:', error);
            if (error.response?.status === 404) {
                return null;
            }
            throw handleApiError(error, 'Error al obtener el empleador');
        }
    }

    formatEmployerDataForBackend(formData) {
        const formattedLegalId = formData.legalId.replace(/-/g, "");

        return {
            person: {
                legalId: formattedLegalId,
                type: "Natural Person",
                province: "",
                canton: "",
                neighborhood: "",
                additionalDirectionDetails: ""
            },
            user: {
                email: formData.email,
                password: formData.password,
                isAdmin: false
            },
            naturalPerson: {
                firstName: formData.name.firstName,
                firstSurname: formData.name.firstSurname,
                secondSurname: formData.name.secondSurname,
                gender: formData.gender || "M"
            },
            contact: {
                type: "Phone Number",
                phoneNumber: formData.phoneNumber
            },
            employer: {
                companyId: formData.companyId || currentUserService.getCurrentUserInformationFromLocalStorage()?.companyId || ""
            }
        };
    }
}

export default new EmployerService();
