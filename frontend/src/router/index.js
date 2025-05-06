import { createRouter, createWebHistory } from 'vue-router'
import CompanyRegistration from '@/views/CompanyRegistration.vue'
import EmployeesList from '@/views/EmployeesList.vue'
import Login from '@/views/LoginForm.vue'
import BenefitList from '@/components/BenefitList.vue'
import Benefit from '@/components/BenefitView.vue'
import BenefitCreate from '@/components/BenefitCreate.vue'
import SelectChangeBenefit from '@/components/SelectChangeBenefit.vue'
import ViewCompanyInfo from '@/views/ViewCompanyInfo.vue'

const routes =
    [
        { path: '/login', name: 'LoginForm', component: Login },
        { path: '/company-registration', name: 'CompanyRegistration', component: CompanyRegistration },
        { path: '/employees-list', name: 'EmployeesList', component: EmployeesList },
        { path: '/benefits', name: 'Benefits', component: BenefitList },
        { path: '/benefit/:id', name: 'Benefit', component: Benefit, props: true },
        { path: '/benefit/create', name: 'CreateBenefit', component: BenefitCreate },
        { path: '/select-change-benefit', name: 'SelectChangeBenefit', component: SelectChangeBenefit },
        { path: '/view-company-info', name: 'ViewCompanyInfo', component: ViewCompanyInfo },
    ]


const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
