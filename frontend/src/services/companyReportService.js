import axios from 'axios';
import API_CONFIG from '@/config/api';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class CompanyReportService {
    async getAllCompanyReports() {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.COMPANYREPORT.GET_ALL;
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching all company reports:', error);
            throw handleApiError(error, 'Error al obtener los reportes de la empresa');
        }
    }

    async getCompanyReportsByDate(startDate, endDate) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.COMPANYREPORT.BASE;
            const response = await axios.get(buildApiUrl(endpoint), {
                params: { startDate, endDate }
            });
            return response.data;
        } catch (error) {
            console.error('Error fetching company reports by date:', error);
            throw handleApiError(error, 'Error al obtener los reportes de la empresa por fecha');
        }
    }
}

export default new CompanyReportService();