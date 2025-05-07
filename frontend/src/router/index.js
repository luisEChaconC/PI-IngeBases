import AddEmployee from '@/views/AddEmployee.vue'
import MainMenu from '@/components/MainMenu.vue'
import { createRouter, createWebHistory } from 'vue-router'
import CompanyRegistration from '@/views/CompanyRegistration.vue'
import EmployerRegistration from '@/views/EmployerRegistration.vue'
import EmployeesList from '@/views/EmployeesList.vue'
import Login from '@/views/LoginForm.vue'
import BenefitList from '@/components/BenefitList.vue'
import Benefit from '@/components/BenefitView.vue'
import BenefitCreate from '@/components/BenefitCreate.vue'
import SelectChangeBenefit from '@/components/SelectChangeBenefit.vue'
import ViewCompanyInfo from '@/views/ViewCompanyInfo.vue'

import ViewEmployeeProfileEmployer from '../views/ViewCompaniesList.vue'
const routes =
    [
        { path: '/login', name: 'LoginForm', component: Login },
        { path: '/add-employee', name: 'AddEmployee', component: AddEmployee},
        { path: '/company-registration', name: 'CompanyRegistration', component: CompanyRegistration },
        { path: '/employer-registration/:companyId', name: 'EmployerRegistration', component: EmployerRegistration, props: true },
        { path: '/employer-registration', redirect: '/login' },
        { path: '/view-companies-list',name: 'ViewCompaniesList',component: ViewEmployeeProfileEmployer},
        { path: '/employees-list', name: 'EmployeesList', component: EmployeesList },
        { path: '/benefits', name: 'Benefits', component: BenefitList },
        { path: '/benefit/:id', name: 'Benefit', component: Benefit, props: true },
        { path: '/benefit/create', name: 'CreateBenefit', component: BenefitCreate },
        { path: '/select-change-benefit', name: 'SelectChangeBenefit', component: SelectChangeBenefit },
        { path: '/main-menu', name: 'MainMenu', component: MainMenu }
        { path: '/view-company-info', name: 'ViewCompanyInfo', component: ViewCompanyInfo },
    ]


const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
