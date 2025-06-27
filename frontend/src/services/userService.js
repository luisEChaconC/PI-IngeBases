import axios from 'axios';
import API_CONFIG from '@/config/api';
import currentUserService from '@/services/currentUserService';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class UserService {
    async login(email, password) {
        try {
            const user = await this.getUserByEmail(email);

            if (password !== user.password) {
                throw new Error('Correo electrónico o contraseña incorrectos.');
            }

            await currentUserService.fetchAndSaveCurrentUserInformationToLocalStorage(email);
            return user;
        } catch (error) {
            console.error('Error during login:', error);

            if (error.message === 'Correo electrónico o contraseña incorrectos.') {
                throw error;
            }

            if (error.response?.status === 404) {
                // User not found
                throw new Error('Correo electrónico o contraseña incorrectos.');
            } else if (error.response?.status === 500) {
                // Server error
                throw new Error('Ocurrió un error en el servidor. Inténtelo más tarde.');
            }

            throw handleApiError(error, 'No se pudo conectar con el servidor.');
        }
    }

    async getUserByEmail(email) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.USER.GET_BY_EMAIL;
            const response = await axios.get(buildApiUrl(endpoint), {
                params: { email }
            });

            return response.data;
        } catch (error) {
            console.error('Error fetching user by email:', error);
            throw handleApiError(error, 'Error al obtener el usuario');
        }
    }

    async getUserInformationByEmail(email) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.USER.GET_BY_EMAIL_DETAILED;
            const response = await axios.get(buildApiUrl(endpoint), {
                params: { email }
            });

            return response.data;
        } catch (error) {
            console.error('Error fetching user information by email:', error);
            throw handleApiError(error, 'Error al obtener la información del usuario');
        }
    }
}

export default new UserService();
