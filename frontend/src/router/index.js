import AddEmployee from '@/views/AddEmployee.vue'
import MainMenu from '@/components/MainMenu.vue'
import { createRouter, createWebHistory } from 'vue-router'
import CompanyRegistration from '@/views/CompanyRegistration.vue'
import EmployeesList from '@/views/EmployeesList.vue'
import Login from '@/views/LoginForm.vue'
import BenefitList from '@/components/BenefitList.vue'
import Benefit from '@/components/BenefitView.vue'
import BenefitCreate from '@/components/BenefitCreate.vue'
import SelectChangeBenefit from '@/components/SelectChangeBenefit.vue'
import ViewEmployeeProfileEmployer from '../views/ViewEmployeeProfileEmployer.vue'

import ViewCompaniesList from '../views/ViewCompaniesList.vue'
const routes =
    [
        { path: '/login', name: 'LoginForm', component: Login },
        { path: '/add-employee', name: 'AddEmployee', component: AddEmployee},
        { path: '/company-registration', name: 'CompanyRegistration', component: CompanyRegistration },
        { path: '/view-companies-list',name: 'ViewCompaniesList',component: ViewCompaniesList},
        { path: '/employees-list', name: 'EmployeesList', component: EmployeesList },
        { path: '/view-employee-profile-employer/:id', name: 'ViewEmployeeProfileEmployer', component: ViewEmployeeProfileEmployer},
        { path: '/benefits', name: 'Benefits', component: BenefitList },
        { path: '/benefit/:id', name: 'Benefit', component: Benefit, props: true },
        { path: '/benefit/create', name: 'CreateBenefit', component: BenefitCreate },
        { path: '/select-change-benefit', name: 'SelectChangeBenefit', component: SelectChangeBenefit },
        { path: '/main-menu', name: 'MainMenu', component: MainMenu }
    ]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
