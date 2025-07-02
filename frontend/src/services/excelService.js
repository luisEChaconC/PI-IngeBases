import axios from 'axios';
import API_CONFIG from '@/config/api';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class ExcelService {
    async generateExcel(payload) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.EXCEL.GENERATE_EXCEL;
            const url = buildApiUrl(endpoint);

            const response = await axios.post(url, payload, {
                responseType: 'blob'
            });
            return response.data;
        } catch (error) {
            console.error('Error generating Excel report:', error);
            throw handleApiError(error, 'Error al generar el archivo Excel');
        }
    }
}

export default new ExcelService();