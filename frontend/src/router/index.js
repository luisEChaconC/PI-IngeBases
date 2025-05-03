import ViewEmployeeProfileEmployer from '../views/ViewCompaniesList.vue'
import { createRouter, createWebHistory } from 'vue-router'

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
