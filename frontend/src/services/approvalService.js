import axios from 'axios';
import API_CONFIG from '@/config/api';
import { handleApiError, buildApiUrl } from '@/utils/apiUtils';

class ApprovalService {
    async getPendingApprovalsByEmployee() {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.APPROVAL.GET_PENDING_BY_EMPLOYEE;
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching pending approvals:', error);
            throw handleApiError(error, 'Error al obtener las aprobaciones pendientes');
        }
    }

    async getPendingApprovalsByEmployeeWithInfo() {
        try {
            const currentUserInformation = JSON.parse(localStorage.getItem('currentUserInformation'));
            const companyId = currentUserInformation?.companyId;

            if (!companyId) {
                throw new Error('No se pudo obtener el ID de la empresa del usuario actual');
            }

            const endpoint = API_CONFIG.ENDPOINTS.APPROVAL.GET_PENDING_BY_EMPLOYEE_WITH_INFO;
            const response = await axios.get(buildApiUrl(endpoint), {
                params: { companyId }
            });
            return response.data;
        } catch (error) {
            console.error('Error fetching pending approvals with info:', error);
            throw handleApiError(error, 'Error al obtener las aprobaciones pendientes con información del empleado');
        }
    }

    async getPendingDaysByEmployee(employeeId) {
        try {
            if (!employeeId) {
                throw new Error('EmployeeId is required');
            }

            const endpoint = API_CONFIG.ENDPOINTS.APPROVAL.GET_PENDING_DAYS_BY_EMPLOYEE(employeeId);
            const response = await axios.get(buildApiUrl(endpoint));
            return response.data;
        } catch (error) {
            console.error('Error fetching pending days for employee:', error);
            throw handleApiError(error, 'Error al obtener los días pendientes del empleado');
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
            const response = await axios.post(buildApiUrl(endpoint), null, {
                params: { supervisorId }
            });
            return response.data;
        } catch (error) {
            console.error('Error approving day:', error);
            throw handleApiError(error, 'Error al aprobar el día');
        }
    }
}

export default new ApprovalService();
