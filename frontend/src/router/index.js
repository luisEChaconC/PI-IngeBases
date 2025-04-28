import ViewEmployeeProfile from '../views/ViewEmployeeProfile.vue'
import { createRouter, createWebHistory } from 'vue-router'

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
