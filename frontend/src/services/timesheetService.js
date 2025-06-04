import axios from 'axios';
import API_CONFIG from '@/config/api';
import currentUserService from '@/services/currentUserService';

class TimesheetService {
    constructor() {
        this.apiBaseUrl = API_CONFIG.BASE_URL;
    }

    async getEmployeeTimesheetByDate(employeeId, date) {
        try {
            const dateString = date.toISOString().split('T')[0]; // YYYY-MM-DD
            const endpoint = API_CONFIG.ENDPOINTS.TIMESHEET.GET_BY_EMPLOYEE_AND_DATE(employeeId);

            const response = await axios.get(`${this.apiBaseUrl}${endpoint}`, {
                params: { date: dateString }
            });
            return response.data;
        } catch (error) {
            console.error('Error fetching employee timesheet:', error);

            if (error.response?.status === 404) {
                return null; // No timesheet found
            }

            throw this.handleApiError(error, 'Error al obtener el timesheet del empleado');
        }
    }

    async getDaysByTimesheetId(timesheetId) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.TIMESHEET.GET_DAYS_BY_TIMESHEET_ID(timesheetId);
            const response = await axios.get(`${this.apiBaseUrl}${endpoint}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching timesheet days:', error);
            throw this.handleApiError(error, 'Error al obtener los días del timesheet');
        }
    }

    mapBackendDaysToFrontend(backendDays, weekDays, timesheetStartDate, timesheetEndDate) {
        return weekDays.map(weekDay => {
            const backendDay = backendDays.find(day => {
                const backendDate = new Date(day.date);
                return this.isSameDate(backendDate, weekDay.date);
            });

            const isOutOfPeriod = weekDay.date < timesheetStartDate || weekDay.date > timesheetEndDate;

            if (backendDay && !isOutOfPeriod) {
                return {
                    name: weekDay.name,
                    date: weekDay.date,
                    hours: backendDay.hoursWorked || 0,
                    description: backendDay.workDescription || '',
                    status: backendDay.isApproved ? 'Aceptado' : 'Sin revisión',
                    isCurrent: false, // Determined in the parent component
                    isOutOfPeriod: false,
                    isWeekend: weekDay.isWeekend,
                    backendId: backendDay.id // Save ID for future updates
                };
            } else {
                // Day does not exist in backend or is out of period
                return {
                    name: weekDay.name,
                    date: weekDay.date,
                    hours: 0,
                    description: '',
                    status: 'Sin revisión',
                    isCurrent: false,
                    isOutOfPeriod: isOutOfPeriod,
                    isWeekend: weekDay.isWeekend,
                    backendId: null
                };
            }
        });
    }

    handleApiError(error, defaultMessage) {
        const apiError = new Error(defaultMessage);

        if (error.response) {
            // Server error
            apiError.message = error.response.data?.message || defaultMessage;
            apiError.status = error.response.status;
        } else if (error.request) {
            // Network error
            apiError.message = 'Connection error. Please check your internet connection.';
        } else {
            // Configuration error
            apiError.message = 'Internal error. Please try again.';
        }

        return apiError;
    }

    isSameDate(date1, date2) {
        return date1.getFullYear() === date2.getFullYear() &&
            date1.getMonth() === date2.getMonth() &&
            date1.getDate() === date2.getDate();
    }

    getCurrentEmployeeId() {
        return currentUserService.getCurrentEmployeeId();
    }

    async saveDay(dayData) {
        try {
            if (!dayData.backendId) {
                throw new Error('Cannot save: day must exist in the backend to be updated');
            }

            return await this.updateExistingDay(dayData);
        } catch (error) {
            console.error('Error saving day:', error);
            throw this.handleApiError(error, 'Error al guardar el día');
        }
    }

    async updateExistingDay(dayData) {
        try {
            const endpoint = API_CONFIG.ENDPOINTS.TIMESHEET.UPDATE_DAY(dayData.backendId);
            const fullUrl = `${this.apiBaseUrl}${endpoint}`;

            // Map frontend properties to backend DTO
            const payload = {
                WorkedHours: dayData.hours || 0,
                Description: (dayData.description && dayData.description.trim()) ? dayData.description.trim() : 'Trabajo realizado' // Backend requires non-empty description
            };

            console.log('Attempting to update day:', {
                url: fullUrl,
                payload: payload,
                dayData: dayData
            });

            const response = await axios.put(fullUrl, payload);

            console.log('Day updated successfully:', response.data);
            return response.data;
        } catch (error) {
            console.error('Error in updateExistingDay:', {
                url: `${this.apiBaseUrl}${API_CONFIG.ENDPOINTS.TIMESHEET.UPDATE_DAY(dayData.backendId)}`,
                payload: {
                    WorkedHours: dayData.hours || 0,
                    Description: (dayData.description && dayData.description.trim()) ? dayData.description.trim() : 'Trabajo realizado'
                },
                error: error,
                response: error.response?.data,
                status: error.response?.status
            });
            throw error; // Re-throw to be handled by saveDay
        }
    }
}

export default new TimesheetService(); 