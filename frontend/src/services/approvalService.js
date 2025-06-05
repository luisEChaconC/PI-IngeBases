import axios from 'axios';
import API_CONFIG from '@/config/api';

class ApprovalService {
    constructor() {
        this.apiBaseUrl = API_CONFIG.BASE_URL;
    }

    async getPendingApprovalsByEmployee() {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.APPROVAL.GET_PENDING_BY_EMPLOYEE;
            const response = await axios.get(`${this.apiBaseUrl}${endpoint}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching pending approvals:', error);
            throw this.handleApiError(error, 'Error al obtener las aprobaciones pendientes');
        }
    }

    async getPendingApprovalsByEmployeeWithInfo() {
        try {
            // Get companyId from localStorage
            const currentUserInformation = JSON.parse(localStorage.getItem('currentUserInformation'));
            const companyId = currentUserInformation?.companyId;

            if (!companyId) {
                throw new Error('No se pudo obtener el ID de la empresa del usuario actual');
            }

            const endpoint = API_CONFIG.ENDPOINTS.APPROVAL.GET_PENDING_BY_EMPLOYEE_WITH_INFO;
            const response = await axios.get(`${this.apiBaseUrl}${endpoint}`, {
                params: { companyId }
            });
            return response.data;
        } catch (error) {
            console.error('Error fetching pending approvals with info:', error);
            throw this.handleApiError(error, 'Error al obtener las aprobaciones pendientes con información del empleado');
        }
    }

    async getPendingDaysByEmployee(employeeId) {
        try {
            if (!employeeId) {
                throw new Error('EmployeeId is required');
            }

            const endpoint = API_CONFIG.ENDPOINTS.APPROVAL.GET_PENDING_DAYS_BY_EMPLOYEE(employeeId);
            const response = await axios.get(`${this.apiBaseUrl}${endpoint}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching pending days for employee:', error);
            throw this.handleApiError(error, 'Error al obtener los días pendientes del empleado');
        }
    }

    async approveDay(dayId, supervisorId) {
        try {
            if (!dayId) {
                throw new Error('DayId is required');
            }

            if (!supervisorId) {
                throw new Error('SupervisorId is required');
            }

            const endpoint = API_CONFIG.ENDPOINTS.APPROVAL.APPROVE_DAY(dayId);
            const response = await axios.post(`${this.apiBaseUrl}${endpoint}`, null, {
                params: { supervisorId }
            });
            return response.data;
        } catch (error) {
            console.error('Error approving day:', error);
            throw this.handleApiError(error, 'Error al aprobar el día');
        }
    }

    handleApiError(error, defaultMessage) {
        const apiError = new Error(defaultMessage);

        if (error.response) {
            // Server error with response
            apiError.message = error.response.data?.message || defaultMessage;
            apiError.status = error.response.status;

            // Handle specific status codes
            if (error.response.status === 400) {
                apiError.message = error.response.data?.message || 'Solicitud inválida';
            } else if (error.response.status === 404) {
                apiError.message = 'Recurso no encontrado';
            } else if (error.response.status === 500) {
                apiError.message = 'Error interno del servidor';
            }
        } else if (error.request) {
            // Network error
            apiError.message = 'Error de conexión. Por favor, verifica tu conexión a internet.';
        } else {
            // Configuration error
            apiError.message = 'Error interno. Por favor, intenta de nuevo.';
        }

        return apiError;
    }
}

export default new ApprovalService(); 