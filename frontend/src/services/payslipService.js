import axios from 'axios';
import API_CONFIG from '@/config/api';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class PayslipService {
    async getPayslipsByEmployeeId(employeeId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.PAYSLIP.GET_BY_ID(employeeId);
            const url = buildApiUrl(endpoint);

            const response = await axios.get(url);
            return response.data;
        } catch (error) {
            console.error('Error fetching payslip by employee ID:', error);
            throw handleApiError(error, 'Error al obtener la colilla de pago');
        }
    }

    async getPayslipByEmployeeIdAndDate(employeeId, startDate) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.PAYSLIP.GET_BY_ID_AND_DATE(employeeId);
            const url = buildApiUrl(endpoint);

            const response = await axios.get(url, {
                params: { startDate }
            });

            return response.data;
        } catch (error) {
            console.error('Error fetching payslip by employee and date:', error);
            throw handleApiError(error, 'Error al obtener la colilla de pago');
        }
    }
}

export default new PayslipService();
