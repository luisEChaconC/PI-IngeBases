import AddEmployee from '../views/AddEmployee.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    {
        path: '/add-employee',
        name: 'AddEmployee',
        component: AddEmployee
      }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
