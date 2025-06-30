const API_CONFIG = {
    BASE_URL: 'https://localhost:5000',

    ENDPOINTS: {
        TIMESHEET: {
            BASE: '/timesheet',
            GET_BY_EMPLOYEE_AND_DATE: (employeeId) => `/api/timesheet/employee/${employeeId}/timesheet-by-date`,
            GET_DAYS_BY_TIMESHEET_ID: (timesheetId) => `/api/timesheet/${timesheetId}/days`,
            UPDATE_DAY: (dayId) => `/day/${dayId}`,
        },
        EMPLOYEE: {
            BASE: '/employee',
            CREATE_WITH_DEPENDENCIES: '/api/Employee/CreateEmployeeWithDependencies',
            GET_BY_COMPANY_ID: (companyId) => `/api/Employee/GetEmployeesByCompanyId?companyId=${companyId}`,
            GET_BY_ID: (employeeId) => `/api/EmployeeGetID/GetEmployeeById/${employeeId}`,
            UPDATE: '/api/Employee/UpdateEmployee',
            UPDATE_AS_EMPLOYER: '/api/Employer/UpdateEmployee',
            HAS_PAYMENTS: (employeeId) => `/api/Employer/HasPayments/${employeeId}`
        },
        EMPLOYER: {
            BASE: '/employer',
            CREATE_WITH_DEPENDENCIES: '/api/Employer/CreateEmployerWithDependencies',
            GET_BY_ID: (employerId) => `/api/EmployerGetID/GetEmployerById/${employerId}`,
        },
        COMPANY: {
            BASE: '/company',
            CREATE_WITH_DEPENDENCIES: '/api/Company/CreateCompanyWithDependencies',
            GET_BY_ID: (companyId) => `/api/company/GetCompanyById/${companyId}`,
            GET_ALL: '/api/company/GetCompanies',
            GET_PAYROLLS: (companyId) => `/api/Payroll/company/${companyId}`,
            UPDATE: (companyId) => `/api/Company/${companyId}`,
            DELETE: (companyId) => `/api/Company/${companyId}`
        },
        BENEFIT: {
            BASE: '/benefit',
            CREATE: '/api/benefit',
            GET_BY_COMPANY_ID: (companyId) => `/api/benefit?companyId=${companyId}`,
            GET_BY_ID: (benefitId, companyId) => `/api/benefit/${benefitId}?companyId=${companyId}`,
            UPDATE: (benefitId) => `/api/benefit/${benefitId}`,
            ASSIGN: '/api/benefit/assign',
            GET_ASSIGNED: (employeeId) => `/api/benefit/assigned?employeeId=${employeeId}`,
            DISABLE: '/api/benefit/disable',
            IS_ASSIGNED: (benefitId) => `/api/benefit/benefits/${benefitId}/is-assigned`,
            DELETE: (benefitId) => `/api/benefit?benefitId=${benefitId}`
        },
        PAYROLL: {
            BASE: '/payroll',
            CREATE: '/api/Payroll',
            GET_BY_COMPANY_ID: (companyId) => `/api/payroll/company/${companyId}`,
            GET_SUMMARY_BY_COMPANY_ID: (companyId) => `/api/payroll/company/${companyId}/summary`,
            UPDATE: (payrollId) => `/api/payroll/${payrollId}`
        },
        API: {
            BASE: '/api',
            GET_ALL: '/api/API',
            GET_PARAMETERS: (apiId) => `/api/API/${apiId}/parameters`,
            SAVE_PARAMETER_VALUES: '/api/api/parameters/values'
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
            GET_BY_EMAIL: '/api/User/GetUserByEmail',
            GET_BY_EMAIL_DETAILED: '/api/User/GetUserInformationByEmail'
        },
        PAYSLIP: {
            BASE: '/payslip',
            GET_BY_ID: (employeeId) => `/api/payslip/employee/${employeeId}`,
            GET_BY_ID_AND_DATE: (employeeId) => `/api/payslip/employee/${employeeId}/by-start-date`
        }
    }
};

export default API_CONFIG;