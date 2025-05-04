import ViewEmployeeProfile from '../views/ViewEmployeeProfile.vue'
import { createRouter, createWebHistory } from 'vue-router'
import BenefitList from '@/components/BenefitList.vue' 

const routes = [
    {
        path: '/view-employee-profile',
        name: 'ViewEmployeeProfile',
        component: ViewEmployeeProfile
      }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
