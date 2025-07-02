import axios from 'axios';
import API_CONFIG from '@/config/api';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class EmailService {
    async sendEmail(emailData) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.EMAIL.SEND;
            const response = await axios.post(buildApiUrl(endpoint), emailData);
            return response.data;
        } catch (error) {
            console.error('Error sending email:', error);
            throw handleApiError(error, 'Error al enviar el correo electrónico');
        }
    }
}

export default new EmailService();
