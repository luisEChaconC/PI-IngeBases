import EmployeesList from '../views/EmployeesList.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    {
        path: '/employees-list',
        name: 'EmployeesList',
        component: EmployeesList
      }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
