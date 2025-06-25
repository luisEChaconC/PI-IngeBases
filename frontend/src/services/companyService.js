import axios from 'axios';
import API_CONFIG from '@/config/api';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class CompanyService {
    async createCompanyWithDependencies(companyData) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.COMPANY.CREATE_WITH_DEPENDENCIES;
            const formattedData = this.formatCompanyDataForBackend(companyData);

            const response = await axios.post(buildApiUrl(endpoint), formattedData);
            return response.data;
        } catch (error) {
            console.error('Error creating company:', error);
            throw handleApiError(error, 'Error al crear la empresa');
        }
    }

    async getCompanyById(companyId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.COMPANY.GET_BY_ID(companyId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching company:', error);
            if (error.response?.status === 404) {
                return null;
            }
            throw handleApiError(error, 'Error al obtener la empresa');
        }
    }

    async getAllCompanies() {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.COMPANY.GET_ALL;
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching companies:', error);
            throw handleApiError(error, 'Error al obtener las empresas');
        }
    }

    async updateCompany(companyId, companyData) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.COMPANY.UPDATE(companyId);
            const formattedData = this.formatCompanyDataForUpdate(companyData);
            const response = await axios.put(buildApiUrl(endpoint), formattedData);
            return response.data;
        } catch (error) {
            console.error('Error updating company:', error);
            throw handleApiError(error, 'Error al actualizar la empresa');
        }
    }

    async deleteCompany(companyId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.COMPANY.DELETE(companyId);
            const response = await axios.delete(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error deleting company:', error);
            throw handleApiError(error, 'Error al eliminar la empresa');
        }
    }

    async checkCompanyHasPayrolls(companyId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.COMPANY.GET_PAYROLLS(companyId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data && response.data.length > 0;
        } catch (error) {
            return false;
        }
    }

    formatCompanyDataForBackend(formData) {
        const formattedLegalId = formData.person.legalId.replace(/-/g, "");

        return {
            person: {
                ...formData.person,
                legalId: formattedLegalId
            },
            company: formData.company,
            contacts: formData.contacts
        };
    }

    formatCompanyDataForUpdate(companyData) {
        const formatted = { ...companyData };

        if (formatted.person && formatted.person.legalId) {
            formatted.person.legalId = formatted.person.legalId.replace(/-/g, '');
        }

        if (formatted.contact && formatted.contact.phoneNumber) {
            formatted.contact.phoneNumber = formatted.contact.phoneNumber.replace(/-/g, '');
        }

        return formatted;
    }
}

export default new CompanyService();
