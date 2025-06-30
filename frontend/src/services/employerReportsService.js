import axios from 'axios';
import API_CONFIG from '@/config/api';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class EmployerReportsService {
    async getEmployeePayrollReport(employerId, companyId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.EMPLOYER_REPORTS.GET_EMPLOYEE_PAYROLL_REPORT(employerId, companyId);
            const url = buildApiUrl(endpoint);

            const response = await axios.get(url);
            return response.data;
        } catch (error) {
            console.error('Error fetching employee payroll report:', error);
            throw handleApiError(error, 'Error al obtener el reporte de planilla de empleados');
        }
    }
}

export default new EmployerReportsService();