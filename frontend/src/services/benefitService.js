import axios from 'axios';
import API_CONFIG from '@/config/api';
import currentUserService from '@/services/currentUserService';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class BenefitService {
    async createBenefit(benefitData) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.BENEFIT.CREATE;
            const userInfo = currentUserService.getCurrentUserInformationFromLocalStorage();

            const formattedData = {
                ...benefitData,
                companyId: userInfo.companyId
            };

            const response = await axios.post(buildApiUrl(endpoint), formattedData);
            return response.data;
        } catch (error) {
            console.error('Error creating benefit:', error);
            throw handleApiError(error, 'Error al crear el beneficio');
        }
    }

    async getBenefitsByCompanyId(companyId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.BENEFIT.GET_BY_COMPANY_ID(companyId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching benefits:', error);
            throw handleApiError(error, 'Error al obtener los beneficios');
        }
    }

    async getBenefitById(benefitId, companyId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.BENEFIT.GET_BY_ID(benefitId, companyId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching benefit:', error);
            if (error.response?.status === 404) {
                return null;
            }
            throw handleApiError(error, 'Error al obtener el beneficio');
        }
    }

    async updateBenefit(benefitId, benefitData) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.BENEFIT.UPDATE(benefitId);
            const response = await axios.put(buildApiUrl(endpoint), benefitData);
            return response.data;
        } catch (error) {
            console.error('Error updating benefit:', error);
            throw handleApiError(error, 'Error al actualizar el beneficio');
        }
    }

    async benefitIsAssigned(benefitId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.BENEFIT.IS_ASSIGNED(benefitId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error checking if benefit is assigned:', error);
            throw handleApiError(error, 'Error al verificar si el beneficio está asignado');
        }
    }

    async getAssignedBenefits(employeeId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.BENEFIT.GET_ASSIGNED(employeeId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching assigned benefits:', error);
            throw handleApiError(error, 'Error al obtener los beneficios asignados');
        }
    }

    async assignBenefitsToEmployee(employeeId, benefitIds) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.BENEFIT.ASSIGN;
            const payload = { employeeId, benefitIds };
            const response = await axios.post(buildApiUrl(endpoint), payload);
            return response.data;
        } catch (error) {
            console.error('Error assigning benefits:', error);
            throw handleApiError(error, 'Error al asignar beneficios');
        }
    }

    async getAPIs() {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.API.GET_ALL;
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching APIs:', error);
            throw handleApiError(error, 'Error al obtener las APIs');
        }
    }

    async getAPIParameters(apiId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.API.GET_PARAMETERS(apiId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching API parameters:', error);
            throw handleApiError(error, 'Error al obtener los parámetros de la API');
        }
    }

    async saveParameterValues(parameterData) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.API.SAVE_PARAMETER_VALUES;
            const response = await axios.post(buildApiUrl(endpoint), parameterData);
            return response.data;
        } catch (error) {
            console.error('Error saving parameter values:', error);
            throw handleApiError(error, 'Error al guardar los valores de parámetros');
        }
    }
}

export default new BenefitService();
