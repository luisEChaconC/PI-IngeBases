const API_CONFIG = {
    BASE_URL: 'https://localhost:5000',

    ENDPOINTS: {
        TIMESHEET: {
            BASE: '/timesheet',
            GET_BY_EMPLOYEE_AND_DATE: (employeeId) => `/api/timesheet/employee/${employeeId}/timesheet-by-date`,
            GET_DAYS_BY_TIMESHEET_ID: (timesheetId) => `/api/timesheet/${timesheetId}/days`,
            UPDATE_DAY: (dayId) => `/day/${dayId}`,
        },
        APPROVAL: {
            BASE: '/approval',
            GET_PENDING_BY_EMPLOYEE: '/api/approval/pending-by-employee',
            GET_PENDING_BY_EMPLOYEE_WITH_INFO: '/api/approval/pending-by-employee-with-info',
            GET_PENDING_DAYS_BY_EMPLOYEE: (employeeId) => `/api/approval/employee/${employeeId}/pending-days`,
            APPROVE_DAY: (dayId) => `/api/approval/day/${dayId}/approve`,
        },
        USER: {
            BASE: '/user',
            GET_BY_EMAIL: '/api/User/GetUserInformationByEmail'
        }
    }
};

export default API_CONFIG;