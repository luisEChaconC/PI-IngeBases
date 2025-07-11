import axios from 'axios';
import API_CONFIG from '@/config/api';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class PayrollService {
    async getPayrollsSummaryByCompanyId(companyId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.PAYROLL.GET_SUMMARY_BY_COMPANY_ID(companyId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching payrolls summary:', error);
            throw handleApiError(error, 'Error al obtener el resumen de planillas');
        }
    }

    async getPayrollsByCompanyId(companyId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.PAYROLL.GET_BY_COMPANY_ID(companyId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching payrolls:', error);
            throw handleApiError(error, 'Error al obtener las planillas');
        }
    }

    async createPayroll(payload) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.PAYROLL.CREATE;
            const response = await axios.post(buildApiUrl(endpoint), payload);
            return response.data;
        } catch (error) {
            console.error('Error creating payroll:', error);
            throw handleApiError(error, 'Error al crear la planilla');
        }
    }
}

export default new PayrollService(); 