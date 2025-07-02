import axios from 'axios';
import API_CONFIG from '@/config/api';
import currentUserService from '@/services/currentUserService';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class EmployeeService {
    async createEmployeeWithDependencies(employeeData) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.EMPLOYEE.CREATE_WITH_DEPENDENCIES;

            const formattedData = this.formatEmployeeDataForBackend(employeeData);

            console.log('Creating employee with data:', formattedData);

            const response = await axios.post(buildApiUrl(endpoint), formattedData);
            return response.data;
        } catch (error) {
            console.error('Error creating employee:', error);
            throw handleApiError(error, 'Error al crear el empleado');
        }
    }

    async getEmployeesByCompanyId(companyId) {
        try {
            if (!companyId) {
                throw new Error('Company ID is required');
            }

            const endpoint = API_CONFIG.ENDPOINTS.EMPLOYEE.GET_BY_COMPANY_ID(companyId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching employees by company:', error);

            if (error.response?.status === 404) {
                return []; // No employees found
            }

            throw handleApiError(error, 'Error al obtener los empleados de la empresa');
        }
    }

    async getEmployeeById(employeeId) {
        try {
            if (!employeeId) {
                throw new Error('Employee ID is required');
            }

            const endpoint = API_CONFIG.ENDPOINTS.EMPLOYEE.GET_BY_ID(employeeId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching employee by ID:', error);

            if (error.response?.status === 404) {
                return null; // Employee not found
            }

            throw handleApiError(error, 'Error al obtener la información del empleado');
        }
    }

    async updateEmployeeAsEmployer(employeeData) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.EMPLOYEE.UPDATE_AS_EMPLOYER;
            const response = await axios.patch(buildApiUrl(endpoint), employeeData);
            return response.data;
        } catch (error) {
            console.error('Error updating employee:', error);
            throw handleApiError(error, 'Error al actualizar el empleado');
        }
    }

    async checkEmployeeHasPayments(employeeId) {
        try {
            if (!employeeId) {
                throw new Error('Employee ID is required');
            }

            const endpoint = API_CONFIG.ENDPOINTS.EMPLOYEE.HAS_PAYMENTS(employeeId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data.hasPayments;
        } catch (error) {
            console.error('Error checking employee payments:', error);
            return false; // Assume no payments if error
        }
    }

    formatEmployeeDataForBackend(formData) {
        const password = `${formData.naturalPerson.firstSurname}${formData.person.legalId.slice(-3)}`;

        const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();

        const formattedLegalId = formData.person.legalId.replace(/-/g, "");

        return {
            person: {
                ...formData.person,
                legalId: formattedLegalId
            },
            user: {
                ...formData.user,
                password: password
            },
            naturalPerson: formData.naturalPerson,
            contact: formData.contact,
            employee: {
                ...formData.employee,
                companyId: currentUserInformation.companyId
            },
            employeeRole: formData.employeeRole
        };
    }

    async deleteEmployee(employeeId) {
    try {
        if (!employeeId) {
            throw new Error('Employee ID is required');
        }

        const endpoint = API_CONFIG.ENDPOINTS.EMPLOYEE.DELETE(employeeId);
        const response = await axios.delete(buildApiUrl(endpoint));
        return response.data;
    } catch (error) {
        console.error('Error deleting employee:', error);
        throw handleApiError(error, 'Error al eliminar el empleado');
    }
}

}

export default new EmployeeService(); 