const API_CONFIG = {
    BASE_URL: 'https://localhost:5000',

    ENDPOINTS: {
        TIMESHEET: {
            BASE: '/timesheet',
            GET_BY_EMPLOYEE_AND_DATE: (employeeId) => `/api/timesheet/employee/${employeeId}/timesheet-by-date`,
            GET_DAYS_BY_TIMESHEET_ID: (timesheetId) => `/api/timesheet/${timesheetId}/days`,
            UPDATE_DAY: (dayId) => `/day/${dayId}`,
        },
        USER: {
            BASE: '/user',
            GET_BY_EMAIL: '/api/User/GetUserInformationByEmail'
        }
    }
};

export default API_CONFIG;