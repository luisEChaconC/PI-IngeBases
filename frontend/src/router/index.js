import ViewEmployeeProfileEmployer from '../views/ViewEmployeeProfileEmployer.vue'
import { createRouter, createWebHistory } from 'vue-router'
const routes = [
    {
        path: '/view-employee-profile-employer',
        name: 'ViewEmployeeProfileEmployer',
        component: ViewEmployeeProfileEmployer
      }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
