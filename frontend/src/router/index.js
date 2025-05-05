import ViewEmployeeProfileEmployer from '../views/ViewCompaniesList.vue'
import { createRouter, createWebHistory } from 'vue-router'
import BenefitList from '@/components/BenefitList.vue' 
import Benefit from '@/components/BenefitView.vue'

const routes =
[
    {
        path: '/benefits',
        name: 'Benefits',
        component: BenefitList
    },

const routes = [
    {
        path: '/view-companies-list',
        name: 'ViewCompaniesList',
        component: ViewEmployeeProfileEmployer
      }
]
const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
