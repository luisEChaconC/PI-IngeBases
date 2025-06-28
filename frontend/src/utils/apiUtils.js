import API_CONFIG from '@/config/api';

export const handleApiError = (error, defaultMessage) => {
    const apiError = new Error(defaultMessage);

    if (error.response) {
        apiError.message = error.response.data?.message || defaultMessage;
        apiError.status = error.response.status;
        apiError.details = error.response.data;
    } else if (error.request) {
        apiError.message = 'Error de conexión. Por favor, verifica tu conexión a internet.';
    } else {
        apiError.message = 'Error interno. Por favor, inténtalo de nuevo.';
    }

    return apiError;
};

export const buildApiUrl = (endpoint) => {
    return `${API_CONFIG.BASE_URL}${endpoint}`;
};
