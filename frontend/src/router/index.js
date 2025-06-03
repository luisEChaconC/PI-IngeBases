import MainLayout from '@/layouts/MainLayout.vue'
import SelectChangeBenefit from '../views/SelectChangeBenefit.vue'
import AddEmployee from '@/views/AddEmployee.vue'
import ViewEmployeeProfile from '../views/ViewEmployeeProfile.vue'
import { createRouter, createWebHistory } from 'vue-router'
import CompanyRegistration from '@/views/CompanyRegistration.vue'
import EmployerRegistration from '@/views/EmployerRegistration.vue'
import EmployeesList from '@/views/EmployeesList.vue'
import Login from '@/views/LoginForm.vue'
import BenefitList from '@/views/BenefitList.vue'
import Benefit from '@/views/BenefitView.vue'
import BenefitCreate from '@/views/BenefitCreate.vue'
import ViewEmployeeProfileEmployer from '../views/ViewEmployeeProfileEmployer.vue'
import ViewCompanyInfo from '@/views/ViewCompanyInfo.vue'
import ViewCompaniesList from '../views/ViewCompaniesList.vue'
import HomeView from '../views/HomeView.vue'
import PayrollsList from '@/views/PayrollsList.vue'
import EmployeeTimesheet from '@/views/EmployeeTimesheet.vue'

const routes = [
    { path: '/', redirect: '/login' },
    { path: '/login', name: 'LoginForm', component: Login },
    { path: '/company-registration', name: 'CompanyRegistration', component: CompanyRegistration },
    { path: '/employer-registration/:companyId', name: 'EmployerRegistration', component: EmployerRegistration, props: true },
    { path: '/employer-registration', redirect: '/login' },

    {
        path: '/',
        component: MainLayout,  
        children: [
   
            { path: '/home-view', name: 'HomeView', component: HomeView },
            { path: '/add-employee', name: 'AddEmployee', component: AddEmployee},
            { path: '/view-employee-profile/:id', name: 'ViewEmployeeProfile', component: ViewEmployeeProfile },
            { path: '/view-companies-list', name: 'ViewCompaniesList', component: ViewCompaniesList},
            { path: '/employees-list', name: 'EmployeesList', component: EmployeesList },
            { path: '/view-employee-profile-employer/:id', name: 'ViewEmployeeProfileEmployer', component: ViewEmployeeProfileEmployer},
            { path: '/benefits', name: 'Benefits', component: BenefitList },
            { path: '/benefit/:id', name: 'Benefit', component: Benefit, props: true },
            { path: '/benefit/create', name: 'CreateBenefit', component: BenefitCreate },
            { path: '/select-change-benefit', name: 'SelectChangeBenefit', component: SelectChangeBenefit },
            { path: '/view-employee-profile', name: 'ViewEmployeeProfile', component: ViewEmployeeProfile },
            { path: '/view-company-info', name: 'ViewCompanyInfo', component: ViewCompanyInfo },
            { path: '/payrolls-list', name: 'PayrollsList', component: PayrollsList },
            { path: '/employee-timesheet', name: 'EmployeeTimesheet', component: EmployeeTimesheet}
        ],
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router