import ViewEmployeeProfileEmployer from '../views/ViewEmployeeProfileEmployer.vue'
import { createRouter, createWebHistory } from 'vue-router'
const routes = [
    {
        path: '/view-employee-profile-employer',
        name: 'ViewEmployeeProfileEmployer',
        component: ViewEmployeeProfileEmployer
      }
]
import BenefitList from '@/components/BenefitList.vue' 
import Benefit from '@/components/BenefitView.vue'

const routes =
[
    {
        path: '/benefits',
        name: 'Benefits',
        component: BenefitList
    },

    {
        path: '/benefit/:id',
        name: 'Benefit',
        component: Benefit,
        props: true
    }

]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
